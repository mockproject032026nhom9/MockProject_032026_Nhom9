from fastapi import APIRouter, Depends
from sqlalchemy.ext.asyncio import AsyncSession
from typing import List

from app.core.database import get_db
from app.core.schemas import ApiResponse
from app.modules.references.schemas import VoidReasonOut
from app.modules.references.services import reference_service

router = APIRouter(prefix="/references", tags=["References"])

@router.get("/void-reasons", response_model=ApiResponse[List[VoidReasonOut]])
async def get_void_reasons(db: AsyncSession = Depends(get_db)):
    """Retrieve the standard list of reasons for voiding an act."""
    reasons = await reference_service.get_void_reasons(db)
    return ApiResponse(data=reasons)
    
# Logic note: This module handles all system-wide lookups (ACT_TYPES, ID_TYPES, etc. in future).
