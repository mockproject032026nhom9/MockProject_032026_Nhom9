from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import String, Integer, DateTime, Boolean, DECIMAL, ForeignKey, func
from datetime import datetime
from typing import List

from app.core.database import Base

class JournalEntry(Base):
    """
    Parent record for a Notary Journal entry. 
    1 : N Relationship with Signers.
    1 : 1 Relationship with Fees and Compliance.
    """
    __tablename__ = "journal_entries"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    notary_id: Mapped[int] = mapped_column(Integer, ForeignKey("users.id"), nullable=False)
    notarial_fee: Mapped[float] = mapped_column(DECIMAL(10, 2), default=0.0)
    status: Mapped[str] = mapped_column(String(20), default="COMPLETED") # DRAFT, COMPLETED, CANCELLED
    is_legal_hold: Mapped[bool] = mapped_column(Boolean, default=False)
    
    # Logic Metadata
    act_type: Mapped[str] = mapped_column(String(50), nullable=False) # e.g., ACKNOWLEDGMENT
    
    # Act Setup Configuration Details (Tester Spec)
    state: Mapped[str | None] = mapped_column(String(100))
    document_title: Mapped[str | None] = mapped_column(String(200))
    number_of_documents: Mapped[int | None] = mapped_column(Integer)
    number_of_signers_expected: Mapped[int | None] = mapped_column(Integer)
    oath_administered_required: Mapped[bool] = mapped_column(Boolean, default=False)
    thumbprint_required: Mapped[bool] = mapped_column(Boolean, default=False)

    date_time: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    location: Mapped[str | None] = mapped_column(String(255))
    notary_notes: Mapped[str | None] = mapped_column(String(500))

    # Relationships (Using selectin loading for high-performance aggregate retrieval)
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
    status_history: Mapped[List["JournalStatusHistory"]] = relationship(
        "JournalStatusHistory",
        back_populates="journal_entry",
        cascade="all, delete-orphan",
        lazy="selectin"
    )
    audit_logs: Mapped[List["ActAuditLog"]] = relationship(
        "ActAuditLog",
        back_populates="journal_entry",
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

class JournalStatusHistory(Base):
    """Traceable history of status changes for a Journal Entry."""
    __tablename__ = "journal_status_history"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    journal_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_entries.id"), nullable=False)
    status: Mapped[str] = mapped_column(String(20), nullable=False)
    timestamp: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    is_active: Mapped[bool] = mapped_column(Boolean, default=True)

    journal_entry: Mapped["JournalEntry"] = relationship("JournalEntry", back_populates="status_history")

class ActAuditLog(Base):
    """Permanent audit trail for actions performed on a Notary Act."""
    __tablename__ = "act_audit_logs"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    journal_id: Mapped[int] = mapped_column(Integer, ForeignKey("journal_entries.id"), nullable=False)
    user_id: Mapped[int | None] = mapped_column(Integer, ForeignKey("users.id"))
    user_name: Mapped[str] = mapped_column(String(100), nullable=False) # Denormalized for historical accuracy
    action: Mapped[str] = mapped_column(String(50), nullable=False) # e.g., VOIDED, CREATED
    details: Mapped[str] = mapped_column(String(500), nullable=False)
    timestamp: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())

    journal_entry: Mapped["JournalEntry"] = relationship("JournalEntry", back_populates="audit_logs")
