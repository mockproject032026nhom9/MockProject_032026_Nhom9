from sqlalchemy.orm import Mapped, mapped_column
from sqlalchemy import String, Integer

from app.core.database import Base

class VoidReason(Base):
    """System reference data for 'Void Reason' dropdowns."""
    __tablename__ = "void_reasons"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, index=True)
    code: Mapped[str] = mapped_column(String(50), nullable=False, unique=True)
    name: Mapped[str] = mapped_column(String(100), nullable=False)
