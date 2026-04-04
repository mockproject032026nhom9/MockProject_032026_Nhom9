from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import String, Integer, DateTime, Boolean, DECIMAL, ForeignKey, func
from datetime import datetime
from typing import List, TYPE_CHECKING

from app.core.database import Base

if TYPE_CHECKING:
    from app.modules.acts.models import NotaryAct

class JournalEntry(Base):
    """
    Specific Journal Entry record. 
    N : 1 Relationship with NotaryAct.
    1 : N Relationship with Signers.
    1 : 1 Relationship with Fees and Compliance.
    """
    __tablename__ = "journal_entries"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    act_id: Mapped[int] = mapped_column(Integer, ForeignKey("notary_acts.id"), nullable=False)
    
    # Financial aggregate
    notarial_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    
    # Logic Metadata (Document Info)
    act_type: Mapped[str] = mapped_column(String(50), nullable=False) # e.g., ACKNOWLEDGMENT
    state: Mapped[str | None] = mapped_column(String(100))
    document_title: Mapped[str | None] = mapped_column(String(200))
    number_of_documents: Mapped[int | None] = mapped_column(Integer)
    
    created_at: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    location: Mapped[str | None] = mapped_column(String(255))
    notary_notes: Mapped[str | None] = mapped_column(String(500))

    # Relationships
    notary_act: Mapped["NotaryAct"] = relationship(
        "NotaryAct", 
        back_populates="journal_records",
        lazy="selectin"
    )
    
    signers: Mapped[List["JournalSigner"]] = relationship(
        "JournalSigner", 
        back_populates="journal_entry", 
        cascade="all, delete-orphan", 
        lazy="selectin"
    )
    fee_breakdown: Mapped["JournalFee"] = relationship(
        "JournalFee", 
        back_populates="journal_entry", 
        uselist=False, 
        cascade="all, delete-orphan", 
        lazy="selectin"
    )
    compliance: Mapped["JournalCompliance"] = relationship(
        "JournalCompliance", 
        back_populates="journal_entry", 
        uselist=False, 
        cascade="all, delete-orphan", 
        lazy="selectin"
    )

class JournalSigner(Base):
    """Many-to-One with JournalEntry. One-to-One with Biometric."""
    __tablename__ = "journal_signers"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    journal_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_entries.id"), nullable=False)
    full_name: Mapped[str] = mapped_column(String(100), nullable=False)
    role: Mapped[str] = mapped_column(String(50), nullable=False) # Grantor, Witness, etc.
    
    # ID Verification Fields according to Tester Spec
    id_type: Mapped[str | None] = mapped_column(String(100)) # e.g. State Driver's License
    id_number: Mapped[str | None] = mapped_column(String(100))
    id_authority: Mapped[str | None] = mapped_column(String(150))
    id_expiry_date: Mapped[datetime | None] = mapped_column(DateTime)
    verification_method: Mapped[str | None] = mapped_column(String(100)) # Physical Presence, RON, etc.
    
    status: Mapped[str] = mapped_column(String(50), default="Not Verified")

    journal_entry: Mapped["JournalEntry"] = relationship("JournalEntry", back_populates="signers")
    biometric: Mapped["JournalBiometric"] = relationship(
        "JournalBiometric", 
        back_populates="signer", 
        uselist=False, 
        cascade="all, delete-orphan", 
        lazy="selectin"
    )

    @property
    def signature_url(self) -> str | None:
        """Helper property to return the static URL for the signature image."""
        if self.biometric and self.biometric.signature_image:
            return f"/uploads/signatures/{self.biometric.signature_image}"
        return None

    @property
    def thumbprint_url(self) -> str | None:
        """Helper property to return the static URL for the thumbprint image."""
        if self.biometric and self.biometric.thumbprint_image:
            return f"/uploads/thumbprints/{self.biometric.thumbprint_image}"
        return None

class JournalBiometric(Base):
    """Biometric data point for a specific signer."""
    __tablename__ = "journal_biometrics"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    signer_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_signers.id"), nullable=False, unique=True)
    signature_image: Mapped[str | None] = mapped_column(String(255))
    thumbprint_image: Mapped[str | None] = mapped_column(String(255))

    signer: Mapped["JournalSigner"] = relationship("JournalSigner", back_populates="biometric")

class JournalFee(Base):
    """Financial audit trail for a specific journal entry."""
    __tablename__ = "journal_fees"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    journal_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_entries.id"), nullable=False, unique=True)
    base_notarial_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    service_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    travel_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    convenience_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    rush_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    total_amount: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    notary_share: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    company_share: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)

    journal_entry: Mapped["JournalEntry"] = relationship("JournalEntry", back_populates="fee_breakdown")

class JournalCompliance(Base):
    """Compliance ledger for audit purposes."""
    __tablename__ = "journal_compliance"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    journal_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_entries.id"), nullable=False, unique=True)
    identity_verification: Mapped[bool] = mapped_column(Boolean, default=False)
    mandatory_fields: Mapped[bool] = mapped_column(Boolean, default=False)
    final_notary_seal: Mapped[bool] = mapped_column(Boolean, default=False)

    journal_entry: Mapped["JournalEntry"] = relationship("JournalEntry", back_populates="compliance")
