from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import String, Integer, DateTime, Date, ForeignKey, func
from datetime import date, datetime
from typing import TYPE_CHECKING, List

from app.core.database import Base

if TYPE_CHECKING:
    from app.modules.users.models import User
    from app.modules.acts.models import NotaryAct

class Notary(Base):
    """
    Professional capacity profile for a Notary. 
    Maintains employment and legal identification records separate from user identity.
    """
    __tablename__ = "notaries"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    user_id: Mapped[int] = mapped_column(Integer, ForeignKey("users.id"), nullable=False, unique=True)
    
    # Identification
    ssn: Mapped[str | None] = mapped_column(String(20)) # Masked in most views
    full_name: Mapped[str] = mapped_column(String(100), nullable=False)
    date_of_birth: Mapped[date | None] = mapped_column(Date)
    photo_url: Mapped[str | None] = mapped_column(String(255))
    
    # Contact
    phone: Mapped[str | None] = mapped_column(String(20))
    email: Mapped[str] = mapped_column(String(100), nullable=False)
    residential_address: Mapped[str | None] = mapped_column(String(255))
    
    # Employment
    employment_type: Mapped[str] = mapped_column(String(50), default="FULL_TIME") # FULL_TIME, PART_TIME, CONTRACTOR
    start_date: Mapped[date] = mapped_column(Date, server_default=func.current_date())
    internal_notes: Mapped[str | None] = mapped_column(String(500))
    status: Mapped[str] = mapped_column(String(20), default="ACTIVE") # ACTIVE, INACTIVE, SUSPENDED
    
    created_at: Mapped[datetime] = mapped_column(DateTime, server_default=func.now())
    updated_at: Mapped[datetime] = mapped_column(DateTime, server_default=func.now(), onupdate=func.now())

    # Relationships
    user: Mapped["User"] = relationship("User", lazy="selectin")
    acts: Mapped[List["NotaryAct"]] = relationship(
        "NotaryAct", 
        back_populates="notary", 
        cascade="all, delete-orphan"
    )
