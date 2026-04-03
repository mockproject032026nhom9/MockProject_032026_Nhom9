from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.database import get_db
from app.core.dependencies import get_current_user
from app.core.schemas import ApiResponse
from app.modules.acts.schemas import (
    ActStatusOut, 
    VoidActRequest, 
    AuditLogsResponse,
    LegalHoldRequest,
    SetupActRequest, 
    AddSignerRequest
)
from app.modules.journal_entries.models import JournalEntry, JournalSigner
from app.modules.acts.services import act_service
from app.modules.users.models import User

router = APIRouter(prefix="/acts", tags=["Acts"])

@router.post("/setup", response_model=ApiResponse[dict])
async def setup_notarial_act(
    data: SetupActRequest,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """
    Configure a new Notarial Act (Journal Entry).
    Returns an Acts summary on success.
    """
    new_entry = JournalEntry(
        notary_id=current_user.id,
        status="IN PROGRESS",
        act_type=data.act_type,
        state=data.state,
        document_title=data.document_title,
        number_of_documents=data.number_of_documents,
        number_of_signers_expected=data.number_of_signers,
        oath_administered_required=data.oath_administered,
        thumbprint_required=data.thumbprint_required
    )
    try:
        db.add(new_entry)
        await db.commit()
        await db.refresh(new_entry)
    except Exception as e:
        import traceback
        return ApiResponse(status_code=500, success=False, message=str(e), data={"traceback": traceback.format_exc()})
    
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        success=True,
        message="Act configured successfully",
        data={"act_id": new_entry.id, "act_type": new_entry.act_type}
    )

@router.post("/{act_id}/signers", response_model=ApiResponse[dict])
async def add_notarial_signer(
    act_id: int,
    data: AddSignerRequest,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """
    Add a signer and verify identity to an existing notarial act.
    """
    # Create the signer record
    new_signer = JournalSigner(
        journal_id=act_id,
        full_name=data.full_name,
        role=data.role,
        id_type=data.id_type,
        id_number=data.id_number,
        id_authority=data.id_authority,
        verification_method=data.verification_method,
        status="Verified" if data.verification_method else "Not Verified"
    )
    if data.id_expiry_date:
        if '/' in data.id_expiry_date:
            from datetime import datetime
            new_signer.id_expiry_date = datetime.strptime(data.id_expiry_date, '%m/%d/%Y')
        else:
            from datetime import datetime
            new_signer.id_expiry_date = datetime.strptime(data.id_expiry_date, '%Y-%m-%d')
            
    db.add(new_signer)
    await db.commit()
    await db.refresh(new_signer)
    
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        success=True,
        message="Signer identity verified successfully" if data.verification_method else "Signer added successfully",
        data={"signer_id": new_signer.id, "status": new_signer.status}
    )


@router.get("/{act_id}/status", response_model=ApiResponse[ActStatusOut])
async def get_act_status(act_id: int, db: AsyncSession = Depends(get_db)):
    """
    Retrieve the status timeline and legal hold status for a specific act.
    Usage: Loading the act status overview or dashboard.
    """
    status_data = await act_service.get_act_status(db, act_id)
    if not status_data:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Act with ID {act_id} not found"
        )
        
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        data=status_data
    )

@router.post("/{act_id}/void", response_model=ApiResponse[dict])
async def void_act(
    act_id: int, 
    data: VoidActRequest, 
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """Void a specific act and record the audit log."""
    success = await act_service.void_act(db, act_id, current_user, data)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Act with ID {act_id} not found"
        )
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        message="Act voided successfully"
    )

@router.put("/{act_id}/legal-hold", response_model=ApiResponse[dict])
async def update_legal_hold(
    act_id: int,
    data: LegalHoldRequest,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """Update the legal hold flag and record the action in the audit log."""
    success = await act_service.update_legal_hold(db, act_id, current_user, data)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Act with ID {act_id} not found"
        )
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        message="Legal hold status updated"
    )

@router.get("/{act_id}/audit-logs", response_model=ApiResponse[AuditLogsResponse])
async def get_audit_logs(
    act_id: int, 
    page: int = 1, 
    size: int = 10, 
    db: AsyncSession = Depends(get_db)
):
    """Retrieve paginated audit logs for a specific act."""
    logs = await act_service.get_audit_logs(db, act_id, page, size)
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        data=logs
    )
