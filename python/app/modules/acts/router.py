from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.database import get_db
from app.core.dependencies import get_current_user
from app.core.schemas import ApiResponse
from app.modules.acts.schemas import (
    ActStatusOut, 
    VoidActRequest, 
    AuditLogsResponse,
    LegalHoldRequest
)
from app.modules.acts.services import act_service
from app.modules.users.models import User

router = APIRouter(prefix="/acts", tags=["Acts"])

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
