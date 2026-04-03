from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select

from sqlalchemy import select, func

from app.modules.journal_entries.models import JournalEntry, JournalStatusHistory, ActAuditLog
from app.modules.acts.schemas import (
    ActStatusOut, 
    StatusTimelineItem, 
    VoidActRequest, 
    AuditLogsResponse,
    LegalHoldRequest
)
from app.modules.users.models import User

class ActService:
    """Business logic for querying Act (Journal) lifecycle status."""
    
    async def get_act_status(self, db: AsyncSession, act_id: int) -> ActStatusOut | None:
        """
        Retrieve the current act's status, timeline, and legal hold flag.
        Logic: Map the internal journal ID and its status_history relationship.
        """
        # Note: We reuse JournalEntry as the core domain for an 'Act'
        result = await db.execute(select(JournalEntry).filter(JournalEntry.id == act_id))
        entry = result.scalars().first()
        
        if not entry:
            return None
            
        # Convert internal status_history to timeline items
        timeline = [
            StatusTimelineItem(
                status=history.status,
                timestamp=history.timestamp,
                is_active=history.is_active
            ) for history in entry.status_history
        ]
        
        # Format the act_id for display (e.g., ACT001)
        display_id = f"ACT{act_id:03}"
        
        return ActStatusOut(
            act_id=display_id,
            timeline=timeline,
            is_legal_hold=entry.is_legal_hold
        )

    async def void_act(self, db: AsyncSession, act_id: int, current_user: User, data: VoidActRequest) -> bool:
        """
        Void a specific act.
        Logic: 
        1. Set status to VOIDED.
        2. Record history snapshot.
        3. Record audit log entry.
        """
        result = await db.execute(select(JournalEntry).filter(JournalEntry.id == act_id))
        entry = result.scalars().first()
        
        if not entry:
            return False

        # 1. Update Status
        entry.status = "VOIDED"
        
        # 2. Record Status History
        history = JournalStatusHistory(
            journal_id=act_id,
            status="Voided",
            is_active=True
        )
        db.add(history)
        
        # 3. Record Audit Log
        audit = ActAuditLog(
            journal_id=act_id,
            user_id=current_user.id,
            user_name=current_user.full_name,
            action="Voided",
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
        """
        Update the legal hold flag for an act.
        Logic: Update the Master flag + Record Audit Log.
        """
        result = await db.execute(select(JournalEntry).filter(JournalEntry.id == act_id))
        entry = result.scalars().first()
        
        if not entry:
            return False

        # 1. Update Flag
        entry.is_legal_hold = data.is_legal_hold
        
        # 2. Record Audit Log
        status_text = "ENABLED" if data.is_legal_hold else "DISABLED"
        audit = ActAuditLog(
            journal_id=act_id,
            user_id=current_user.id,
            user_name=current_user.full_name,
            action="LEGAL_HOLD",
            details=f"Legal hold status updated to {status_text}."
        )
        db.add(audit)
        
        await db.commit()
        return True

    async def get_audit_logs(self, db: AsyncSession, act_id: int, page: int, size: int) -> AuditLogsResponse:
        """Fetch paginated audit logs for a specific act."""
        offset = (page - 1) * size
        
        # Query items
        items_result = await db.execute(
            select(ActAuditLog)
            .filter(ActAuditLog.journal_id == act_id)
            .order_by(ActAuditLog.timestamp.desc())
            .offset(offset)
            .limit(size)
        )
        items = items_result.scalars().all()
        
        # Query total count
        count_result = await db.execute(
            select(func.count()).select_from(ActAuditLog).filter(ActAuditLog.journal_id == act_id)
        )
        total = count_result.scalar() or 0
        
        return AuditLogsResponse(items=list(items), total=total)

act_service = ActService()
