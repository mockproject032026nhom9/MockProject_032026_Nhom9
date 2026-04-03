from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from typing import Sequence

from app.modules.references.models import VoidReason

class ReferenceService:
    """Business logic for system lookup and reference data."""
    
    async def get_void_reasons(self, db: AsyncSession) -> Sequence[VoidReason]:
        """Fetch all available reasons for voiding an act."""
        result = await db.execute(select(VoidReason))
        return result.scalars().all()

reference_service = ReferenceService()
