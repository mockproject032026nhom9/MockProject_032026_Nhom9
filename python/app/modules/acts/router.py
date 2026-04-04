from fastapi import APIRouter, Depends, HTTPException, status, Query
from sqlalchemy.ext.asyncio import AsyncSession
from typing import Optional

from app.core.database import get_db
from app.core.dependencies import get_current_user
from app.core.schemas import ApiResponse
from app.modules.acts.schemas import (
    ActStatusOut, 
    VoidActRequest, 
    AuditLogsResponse, 
    LegalHoldRequest, 
    SetupActRequest,
    ActListResponse,
    ActTypeEnum,
    StatusEnum,
    DateRangeEnum
)
from app.modules.acts.services import act_service
from app.modules.users.models import User

router = APIRouter(prefix="/acts", tags=["Acts"])

@router.get("/", response_model=ApiResponse[ActListResponse])
async def get_acts(
    keyword: Optional[str] = Query(None, description="Search by ID, Notary Name, or Client Name"),
    act_type: Optional[ActTypeEnum] = Query(None, description="Filter by transaction type"),
    act_status: Optional[StatusEnum] = Query(None, description="Filter by session status"),
    date_range: Optional[DateRangeEnum] = Query(None, description="Filter by creation date range"),
    page: int = Query(1, ge=1),
    size: int = Query(10, ge=1, le=100),
    db: AsyncSession = Depends(get_db)
):
    """
    Retrieve a searchable, filtered list of all Notarial Acts.
    Used for the main Dashboard view.
    """
    acts_data = await act_service.get_acts_list(
        db, keyword, act_type, act_status, date_range, page, size
    )
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        data=acts_data
    )

@router.post("/setup", response_model=ApiResponse[dict])
async def setup_notarial_act(
    data: SetupActRequest,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """
    Initialize a new Notarial Act session and create the initial record.
    Usage: Starting a new notarial transaction.
    """
    new_act = await act_service.setup_notarial_act(db, current_user, data)
    
    return ApiResponse(
        status_code=status.HTTP_201_CREATED,
        message="Notary Act session initialized",
        data={"act_id": new_act.id, "status": new_act.status}
    )

@router.get("/{act_id}/status", response_model=ApiResponse[ActStatusOut])
async def get_act_status(act_id: int, db: AsyncSession = Depends(get_db)):
    """
    Retrieve the current status overview and history for a specific act.
    """
    status_data = await act_service.get_act_status(db, act_id)
    if not status_data:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Notary Act with ID {act_id} not found"
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
    """Void an administrative activity session and record reason."""
    success = await act_service.void_act(db, act_id, current_user, data)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Notary Act with ID {act_id} not found"
        )
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        message="Notary Act voided successfully"
    )

@router.put("/{act_id}/legal-hold", response_model=ApiResponse[dict])
async def update_legal_hold(
    act_id: int,
    data: LegalHoldRequest,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    """Toggle the legal hold status for the specific session."""
    success = await act_service.update_legal_hold(db, act_id, current_user, data)
    if not success:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail=f"Notary Act with ID {act_id} not found"
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
    """Retrieve paginated administrative audit logs for a session."""
    logs = await act_service.get_audit_logs(db, act_id, page, size)
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        data=logs
    )
