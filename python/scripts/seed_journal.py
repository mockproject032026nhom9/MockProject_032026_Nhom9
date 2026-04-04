import asyncio
import sys
from pathlib import Path
from datetime import datetime, timedelta

# Add project root to path to allow absolute imports
sys.path.append(str(Path(__file__).parent.parent))

from sqlalchemy import select
from app.core.database import SessionLocal, engine, Base
from app.core.logger import logger
from app.modules.acts.models import (
    NotaryAct,
    ActStatusHistory,
    ActAuditLog,
)
from app.modules.notaries.models import Notary
from app.modules.journal_entries.models import (
    JournalBiometric,
    JournalCompliance,
    JournalEntry,
    JournalFee,
    JournalSigner,
)
from app.modules.roles.models import Role
from app.modules.users.models import User
from app.modules.references.models import VoidReason

async def seed_journal_data():
    """Seed a comprehensive Notary Act and Journal Entry for testing."""
    async with SessionLocal() as db:
        logger.info("Starting database seeding...")
        
        # 1. Standard Roles
        from app.modules.roles.services import role_service
        await role_service.seed_roles(db)
        
        # 2. Default User & Notary Profile
        result = await db.execute(select(User).limit(1))
        user = result.scalars().first()
        
        if not user:
            role_result = await db.execute(select(Role).filter(Role.name == "Admin"))
            admin_role = role_result.scalars().first()
            
            user = User(
                email="admin@notary.com",
                password_hash="$2b$12$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36WQoeG6L65JG6C3V6Xm8f6", # 'password'
                full_name="James Smith",
                role_id=admin_role.id
            )
            db.add(user)
            await db.flush()

        # 3. Create professional Notary Profile
        notary_result = await db.execute(select(Notary).filter(Notary.user_id == user.id))
        notary_profile = notary_result.scalars().first()
        
        if not notary_profile:
            notary_profile = Notary(
                user_id=user.id,
                ssn="123-45-6789",
                full_name="James Smith",
                email="j.smith@mail.com",
                phone="(555) 123-4567",
                employment_type="FULL_TIME",
                start_date=datetime(2021, 6, 1).date(),
                internal_notes="Top performer 2022",
                status="ACTIVE",
                residential_address="123 Maple St, Seattle, WA 98101",
                photo_url="/img/jsmith.jpg"
            )
            db.add(notary_profile)
            await db.flush()

        # 4. Create the Master Notary Act (Session)
        act = NotaryAct(
            notary_id=notary_profile.id,
            status="COMPLETED",
            is_legal_hold=False
        )
        db.add(act)
        await db.flush()

        # 5. Seed Act Lifecycle (History & Audit)
        now = datetime.now()
        history = [
            ActStatusHistory(act_id=act.id, status="DRAFT", timestamp=now - timedelta(hours=2)),
            ActStatusHistory(act_id=act.id, status="IN_PROGRESS", timestamp=now - timedelta(hours=1)),
            ActStatusHistory(act_id=act.id, status="COMPLETED", timestamp=now),
        ]
        db.add_all(history)

        audit = ActAuditLog(
            act_id=act.id,
            user_id=user.id,
            user_name=user.full_name,
            action="CREATED",
            details="Notarial act session initialized from system template."
        )
        db.add(audit)

        # 6. Create a Journal Entry record within this Act
        journal = JournalEntry(
            act_id=act.id,
            notarial_fee=66.50,
            act_type="ACKNOWLEDGMENT",
            state="California",
            document_title="Property Deed Transfer",
            number_of_documents=1,
            location="123 Main St, Springfield",
            notary_notes="Standard acknowledgment for property transfer."
        )
        db.add(journal)
        await db.flush()
        
        # 7. Create Signers
        alice = JournalSigner(
            journal_id=journal.id,
            full_name="Alice Wonderland",
            role="Grantor",
            id_type="Driver License",
            id_number="DL123456789",
            status="Verified"
        )
        bob = JournalSigner(
            journal_id=journal.id,
            full_name="Bob The Builder",
            role="Witness",
            id_type="Passport",
            id_number="P987654321",
            status="Pending"
        )
        db.add_all([alice, bob])
        await db.flush()

        # 8. Create Biometrics
        alice_sign = JournalBiometric(signer_id=alice.id, signature_image="sign_alice.png")
        bob_sign = JournalBiometric(signer_id=bob.id, signature_image="sign_bob.png")
        db.add_all([alice_sign, bob_sign])

        # 9. Create Fee Breakdown
        fees = JournalFee(
            journal_id=journal.id,
            base_notarial_fee=15.00,
            service_fee=50.00,
            total_amount=65.00,
            notary_share=45.50
        )
        db.add(fees)

        # 10. Create Compliance Ledger
        compliance = JournalCompliance(
            journal_id=journal.id,
            identity_verification=True,
            mandatory_fields=True,
            final_notary_seal=True
        )
        db.add(compliance)

        # 11. Reference Data
        void_reasons = [
            VoidReason(code="DATA_ERROR", name="Data Entry Error"),
            VoidReason(code="CLIENT_REQUEST", name="Client Requested Cancel"),
        ]
        db.add_all(void_reasons)

        await db.commit()
        logger.info(f"Successfully seeded Notary Act ID: {act.id} with Journal Record.")

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
