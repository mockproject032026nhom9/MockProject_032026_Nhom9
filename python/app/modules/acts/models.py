from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import String, Integer, DateTime, Boolean, ForeignKey, func
from datetime import datetime
from typing import List, TYPE_CHECKING

from app.core.database import Base

if TYPE_CHECKING:
    from app.modules.journal_entries.models import JournalEntry
    from app.modules.notaries.models import Notary

class NotaryAct(Base):
    """
    High-level management table for a Notary session/activity. 
    Controls status, compliance flags, and aggregate lifecycle.
    """
    __tablename__ = "notary_acts"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    notary_id: Mapped[int] = mapped_column(Integer, ForeignKey("notaries.id"), nullable=False)
    
    # Session Management
    status: Mapped[str] = mapped_column(String(20), default="DRAFT") # DRAFT, IN_PROGRESS, COMPLETED, VOIDED
    is_legal_hold: Mapped[bool] = mapped_column(Boolean, default=False)
    
    created_at: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    updated_at: Mapped[datetime] = mapped_column(DateTime, server_default=func.now(), onupdate=func.now())

    # Relationships
    notary: Mapped["Notary"] = relationship("Notary", back_populates="acts", lazy="selectin")
    
    journal_records: Mapped[List["JournalEntry"]] = relationship(
        "JournalEntry", 
        back_populates="notary_act", 
        cascade="all, delete-orphan", 
        lazy="selectin"
    )
    status_history: Mapped[List["ActStatusHistory"]] = relationship(
        "ActStatusHistory",
        back_populates="notary_act",
        cascade="all, delete-orphan",
        lazy="selectin"
    )
    audit_logs: Mapped[List["ActAuditLog"]] = relationship(
        "ActAuditLog",
        back_populates="notary_act",
        cascade="all, delete-orphan",
        lazy="selectin"
    )

class ActStatusHistory(Base):
    """Traceable history of status changes for a Notary Act session."""
    __tablename__ = "act_status_history"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    act_id: Mapped[int] = mapped_column(Integer, ForeignKey("notary_acts.id"), nullable=False)
    status: Mapped[str] = mapped_column(String(20), nullable=False)
    timestamp: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    is_active: Mapped[bool] = mapped_column(Boolean, default=True)

    notary_act: Mapped["NotaryAct"] = relationship("NotaryAct", back_populates="status_history")

class ActAuditLog(Base):
    """Permanent audit trail for administrative actions performed on a Notary Act."""
    __tablename__ = "act_audit_logs"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    act_id: Mapped[int] = mapped_column(Integer, ForeignKey("notary_acts.id"), nullable=False)
    user_id: Mapped[int | None] = mapped_column(Integer, ForeignKey("users.id"))
    user_name: Mapped[str] = mapped_column(String(100), nullable=False) # Denormalized for historical accuracy
    action: Mapped[str] = mapped_column(String(50), nullable=False) # e.g., VOIDED, CREATED, LEGAL_HOLD
    details: Mapped[str] = mapped_column(String(500), nullable=False)
    timestamp: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())

    notary_act: Mapped["NotaryAct"] = relationship("NotaryAct", back_populates="audit_logs")
