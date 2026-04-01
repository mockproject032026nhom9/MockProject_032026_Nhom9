from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from typing import Sequence

from app.modules.journal_entries.models import JournalEntry, JournalSigner, JournalFee, JournalCompliance, JournalBiometric
from app.modules.journal_entries.schemas import JournalEntryUpdate
from app.core.exceptions import AppException

class JournalService:
    """Business logic for Notary Journal management with aggregated views."""
    
    async def get_by_id(self, db: AsyncSession, journal_id: int) -> JournalEntry | None:
        """
        Fetch a single journal entry by ID.
        Uses selectin loading (already defined in model relationships) 
        to efficiently fetch signers, fees, and compliance data in a single logic block.
        """
        result = await db.execute(select(JournalEntry).filter(JournalEntry.id == journal_id))
        return result.scalars().first()

    async def get_all(self, db: AsyncSession) -> Sequence[JournalEntry]:
        """Fetch all journal entries."""
        result = await db.execute(select(JournalEntry))
        return result.scalars().all()

    async def update_entry(self, db: AsyncSession, journal_id: int, data: JournalEntryUpdate) -> JournalEntry | None:
        """
        Update a journal entry with manual inputs.
        Logic: 
        1. Update master record fields (Fee, Location, Notes).
        2. Extract filenames from biometric URLs and link to the primary signer.
        """
        entry = await self.get_by_id(db, journal_id)
        if not entry:
            return None

        # 1. Update Master Fields
        if data.fee_charged is not None:
            entry.notarial_fee = data.fee_charged
        if data.location is not None:
            entry.location = data.location
        if data.notary_notes is not None:
            entry.notary_notes = data.notary_notes

        # 2. Update Biometrics (Targeting the Primary/First Signer)
        if (data.signature_url or data.thumbprint_url) and entry.signers:
            primary_signer = entry.signers[0]
            
            # Ensure biometric record exists
            if not primary_signer.biometric:
                primary_signer.biometric = JournalBiometric(signer_id=primary_signer.id)
                db.add(primary_signer.biometric)

            # Link URLs by extracting the filename
            if data.signature_url:
                primary_signer.biometric.signature_image = data.signature_url.split("/")[-1]
            if data.thumbprint_url:
                primary_signer.biometric.thumbprint_image = data.thumbprint_url.split("/")[-1]

        await db.commit()
        await db.refresh(entry)
        return entry

journal_service = JournalService()
