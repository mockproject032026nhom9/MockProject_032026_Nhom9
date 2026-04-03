from fastapi import APIRouter, Depends, status
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.database import get_db
from app.core.schemas import ApiResponse
from app.modules.journal_entries.schemas import JournalEntryOut, JournalEntryUpdate
from app.modules.journal_entries.services import journal_service

router = APIRouter(prefix="/journal-entries", tags=["Journal Entries"])

@router.get("/{id}", response_model=ApiResponse[JournalEntryOut])
async def get_journal_entry(id: int, db: AsyncSession = Depends(get_db)):
    """
    Retrieve auto-populated and compliance data for a specific Journal Entry.
    Expected usage: Loading the Journal Entry details screen.
    """
    journal = await journal_service.get_by_id(db, id)
    if not journal:
        return ApiResponse(
            status_code=404,
            success=False,
            message="Journal entry not found"
        )
    
    return ApiResponse(data=journal)

@router.put("/{id}", response_model=ApiResponse[dict])
async def update_journal_entry(id: int, data: JournalEntryUpdate, db: AsyncSession = Depends(get_db)):
    """Update manual inputs (Fee, Location, Notes) and link biometrics."""
    journal = await journal_service.update_entry(db, id, data)
    if not journal:
        return ApiResponse(
            status_code=404,
            success=False,
            message="Journal entry not found"
        )
    
    return ApiResponse(message="Journal entry saved successfully")
