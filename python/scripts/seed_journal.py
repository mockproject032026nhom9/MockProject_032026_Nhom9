import asyncio
import sys
from pathlib import Path

# Add project root to path to allow absolute imports
sys.path.append(str(Path(__file__).parent.parent))

from sqlalchemy import select
from app.core.database import SessionLocal, engine, Base
from app.core.logger import logger
from app.modules.journal_entries.models import (
    JournalBiometric,
    JournalCompliance,
    JournalEntry,
    JournalFee,
    JournalSigner,
    JournalStatusHistory,
    ActAuditLog,
)
from app.modules.roles.models import Role
from app.modules.users.models import User
from app.modules.references.models import VoidReason

async def seed_journal_data():
    """Seed a comprehensive Notary Journal entry for testing aggregate APIs."""
    async with SessionLocal() as db:
        logger.info("Starting Journal seeding...")
        
        # 1. Ensure Roles and a Notary (User) exist
        from app.modules.roles.services import role_service
        await role_service.seed_roles(db)
        
        result = await db.execute(select(User).limit(1))
        notary = result.scalars().first()
        
        if not notary:
            logger.info("No users found. Creating a default Admin notary...")
            # Fetch Admin role
            role_result = await db.execute(select(Role).filter(Role.name == "Admin"))
            admin_role = role_result.scalars().first()
            
            notary = User(
                email="admin@notary.com",
                password_hash="hashed_password", # Dummy for seed
                full_name="Default Notary Admin",
                role_id=admin_role.id
            )
            db.add(notary)
            await db.flush()

        # 2. Create the Master Journal Entry
        journal = JournalEntry(
            notary_id=notary.id,
            notarial_fee=66.50,
            status="COMPLETED",
            act_type="ACKNOWLEDGMENT",
            is_legal_hold=False,
            location="123 Main St, Springfield",
            notary_notes="Standard acknowledgment for property transfer."
        )
        db.add(journal)
        await db.flush() # Get the ID for foreign keys
        
        # 2a. Seed Status History (Timeline)
        from datetime import datetime, timedelta
        now = datetime.now()
        history = [
            JournalStatusHistory(journal_id=journal.id, status="Draft", timestamp=now - timedelta(hours=1), is_active=True),
            JournalStatusHistory(journal_id=journal.id, status="Completed", timestamp=now, is_active=True),
            JournalStatusHistory(journal_id=journal.id, status="Locked", timestamp=now + timedelta(minutes=15), is_active=False),
        ]
        db.add_all(history)
        
        # 3. Create Signers (Alice & Bob)
        alice = JournalSigner(
            journal_id=journal.id,
            full_name="Alice Wonderland",
            role="Grantor",
            id_number="DL123456789",
            status="Verified"
        )
        bob = JournalSigner(
            journal_id=journal.id,
            full_name="Bob The Builder",
            role="Witness",
            id_number="PENDING",
            status="Pending"
        )
        db.add_all([alice, bob])
        await db.flush()

        # 4. Create Biometrics (Signature images)
        alice_sign = JournalBiometric(signer_id=alice.id, signature_image="sign_alice.png")
        bob_sign = JournalBiometric(signer_id=bob.id, signature_image="sign_bob.png")
        db.add_all([alice_sign, bob_sign])

        # 5. Create Fee Breakdown
        fees = JournalFee(
            journal_id=journal.id,
            base_notarial_fee=15.00,
            service_fee=50.00,
            travel_fee=25.00,
            convenience_fee=5.00,
            total_amount=95.00,
            notary_share=66.50,
            company_share=28.50
        )
        db.add(fees)

        # 6. Create Compliance Ledger
        compliance = JournalCompliance(
            journal_id=journal.id,
            identity_verification=True,
            mandatory_fields=False,
            final_notary_seal=False
        )
        db.add(compliance)
        
        # 6a. Record Initial Audit Log
        initial_log = ActAuditLog(
            journal_id=journal.id,
            user_id=notary.id,
            user_name=notary.full_name,
            action="CREATED",
            details="Draft act created from system template."
        )
        db.add(initial_log)

        # 7. Create Reference Data (Void Reasons)
        void_reasons = [
            VoidReason(code="DATA_ERROR", name="Data Entry Error"),
            VoidReason(code="CLIENT_REQUEST", name="Client Requested Cancel"),
        ]
        db.add_all(void_reasons)

        await db.commit()
        logger.info(f"Successfully seeded Journal ID: {journal.id} with all sub-entities.")

async def main():
    """Reset and Seed the database."""
    async with engine.begin() as conn:
        logger.info("Resetting database schema (Drop and Create)...")
        await conn.run_sync(Base.metadata.drop_all)
        await conn.run_sync(Base.metadata.create_all)
    
    await seed_journal_data()
    await engine.dispose()

if __name__ == "__main__":
    asyncio.run(main())
