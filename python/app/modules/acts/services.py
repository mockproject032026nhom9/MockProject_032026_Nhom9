from datetime import datetime, timedelta
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select, func, or_, cast, String
from typing import Optional, Tuple, Sequence

from app.modules.acts.models import NotaryAct, ActStatusHistory, ActAuditLog
from app.modules.notaries.models import Notary
from app.modules.journal_entries.models import JournalEntry, JournalSigner
from app.modules.acts.schemas import (
    SetupActRequest, 
    ActStatusOut, 
    VoidActRequest, 
    AuditLogsResponse,
    LegalHoldRequest,
    ActTypeEnum,
    StatusEnum,
    DateRangeEnum,
    ActListResponse,
    ActListItem
)
from app.modules.users.models import User

class ActService:
    """Business logic for managing Notary Acts and their integrated lifecycles."""

    async def setup_notarial_act(
        self, 
        db: AsyncSession, 
        current_user: User, 
        data: SetupActRequest
    ) -> NotaryAct:
        """
        Configure a new Notary Act linked to the professional profile.
        """
        # 1. Resolve Notary Profile for the current user
        notary_result = await db.execute(select(Notary).filter(Notary.user_id == current_user.id))
        notary = notary_result.scalars().first()
        
        if not notary:
            raise ValueError("Professional Notary profile not found for current user.")

        # 2. Create Notary Act (The session)
        new_act = NotaryAct(
            notary_id=notary.id,
            status="IN_PROGRESS"
        )
        db.add(new_act)
        await db.flush()

        # 3. Record initial status history
        history = ActStatusHistory(
            act_id=new_act.id,
            status="IN_PROGRESS"
        )
        db.add(history)

        # 4. Create initial Audit Log
        audit = ActAuditLog(
            act_id=new_act.id,
            user_id=current_user.id,
            user_name=current_user.full_name,
            action="CREATED",
            details=f"Notary Act session initialized for: {data.document_title}"
        )
        db.add(audit)

        # 5. Create the first Journal Entry tied to this Act
        first_entry = JournalEntry(
            act_id=new_act.id,
            act_type=data.act_type,
            state=data.state,
            document_title=data.document_title,
            number_of_documents=data.number_of_documents
        )
        db.add(first_entry)

        await db.commit()
        await db.refresh(new_act)
        return new_act

    async def get_acts_list(
        self,
        db: AsyncSession,
        keyword: Optional[str] = None,
        act_type: Optional[ActTypeEnum] = None,
        status: Optional[StatusEnum] = None,
        date_range: Optional[DateRangeEnum] = None,
        page: int = 1,
        size: int = 10
    ) -> ActListResponse:
        """
        Retrieves a paginated list of acts with filtering and search.
        Joins: NotaryAct -> Notary -> JournalEntry -> JournalSigner (Client)
        """
        # Subquery for client_name to get the first signer
        client_sub = (
            select(JournalSigner.full_name)
            .where(JournalSigner.journal_id == JournalEntry.id)
            .limit(1)
            .correlate(JournalEntry)
            .scalar_subquery()
        )

        query = (
            select(
                NotaryAct.id,
                JournalEntry.act_type,
                Notary.full_name.label("notary_name"),
                client_sub.label("client_name"),
                NotaryAct.created_at,
                JournalEntry.state,
                NotaryAct.status
            )
            .join(Notary, NotaryAct.notary_id == Notary.id)
            .outerjoin(JournalEntry, NotaryAct.id == JournalEntry.act_id)
        )

        # Filtering logic
        if keyword:
            query = query.where(
                or_(
                    cast(NotaryAct.id, String).contains(keyword),
                    Notary.full_name.contains(keyword),
                    client_sub.contains(keyword)
                )
            )

        if act_type:
            query = query.where(JournalEntry.act_type == act_type.value)
        
        if status:
            query = query.where(NotaryAct.status == status.value)

        if date_range:
            now = datetime.now()
            if date_range == DateRangeEnum.TODAY:
                query = query.where(NotaryAct.created_at >= now.replace(hour=0, minute=0, second=0))
            elif date_range == DateRangeEnum.LAST_7_DAYS:
                query = query.where(NotaryAct.created_at >= now - timedelta(days=7))
            elif date_range == DateRangeEnum.LAST_30_DAYS:
                query = query.where(NotaryAct.created_at >= now - timedelta(days=30))
            elif date_range == DateRangeEnum.LAST_90_DAYS:
                query = query.where(NotaryAct.created_at >= now - timedelta(days=90))

        # Pagination and Execution
        offset = (page - 1) * size
        
        # Count total matches
        count_query = select(func.count()).select_from(query.subquery())
        total = await db.scalar(count_query) or 0

        # Execute page query
        query = query.order_by(NotaryAct.created_at.desc()).offset(offset).limit(size)
        result = await db.execute(query)
        items = result.all()

        act_items = [
            ActListItem(
                id=item.id,
                act_type=item.act_type or "N/A",
                notary_name=item.notary_name,
                client_name=item.client_name or "N/A",
                created_at=item.created_at,
                state=item.state or "N/A",
                status=item.status
            ) for item in items
        ]

        return ActListResponse(items=act_items, total=total)

    async def get_act_status(self, db: AsyncSession, act_id: int) -> Optional[ActStatusOut]:
        """Fetch the current status and history timeline for a session."""
        result = await db.execute(select(NotaryAct).filter(NotaryAct.id == act_id))
        act = result.scalars().first()
        
        if not act:
            return None

        return ActStatusOut(
            act_id=act.id,
            status=act.status,
            timeline=act.status_history,
            is_legal_hold=act.is_legal_hold
        )

    async def void_act(
        self, 
        db: AsyncSession, 
        act_id: int, 
        current_user: User, 
        data: VoidActRequest
    ) -> bool:
        """
        Void a session and record the reason.
        Logic: Update Master Status -> Add Status History -> Log Audit.
        """
        result = await db.execute(select(NotaryAct).filter(NotaryAct.id == act_id))
        act = result.scalars().first()
        
        if not act:
            return False

        # 1. Update Master Status
        act.status = "VOIDED"
        
        # 2. Add History
        history = ActStatusHistory(
            act_id=act_id,
            status="VOIDED"
        )
        db.add(history)
        
        # 3. Log Audit
        audit = ActAuditLog(
            act_id=act_id,
            user_id=current_user.id,
            user_name=current_user.full_name,
            action="VOIDED",
            details=f"Reason: {data.reason_code}. Notes: {data.additional_notes}"
        )
        db.add(audit)
        
        await db.commit()
        return True

    async def update_legal_hold(
        self, 
        db: AsyncSession, 
        act_id: int, 
        current_user: User, 
        data: LegalHoldRequest
    ) -> bool:
        """Toggle legal hold and record the administrative action."""
        result = await db.execute(select(NotaryAct).filter(NotaryAct.id == act_id))
        act = result.scalars().first()
        
        if not act:
            return False

        act.is_legal_hold = data.is_legal_hold
        
        status_text = "ENABLED" if data.is_legal_hold else "DISABLED"
        audit = ActAuditLog(
            act_id=act_id,
            user_id=current_user.id,
            user_name=current_user.full_name,
            action="LEGAL_HOLD",
            details=f"Legal hold status updated to {status_text}."
        )
        db.add(audit)
        
        await db.commit()
        return True

    async def get_audit_logs(self, db: AsyncSession, act_id: int, page: int, size: int) -> AuditLogsResponse:
        """Fetch paginated audit logs for a specific activity session."""
        offset = (page - 1) * size
        
        # Query items
        items_query = await db.execute(
            select(ActAuditLog)
            .filter(ActAuditLog.act_id == act_id)
            .order_by(ActAuditLog.timestamp.desc())
            .offset(offset)
            .limit(size)
        )
        items = items_query.scalars().all()

        # Count total
        count_query = await db.execute(
            select(func.count()).select_from(ActAuditLog).filter(ActAuditLog.act_id == act_id)
        )
        total = count_query.scalar_one()

        return AuditLogsResponse(items=list(items), total=total)

act_service = ActService()
