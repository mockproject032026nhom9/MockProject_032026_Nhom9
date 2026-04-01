from sqlalchemy import Boolean, Column, Integer, String, Text, ForeignKey, DateTime
from sqlalchemy.orm import relationship
from app.core.database import Base

class NotaryAct(Base):
    __tablename__ = "notary_acts"

    id = Column(Integer, primary_key=True, index=True)
    request_id = Column(Integer, nullable=False)
    notary_id = Column(Integer, nullable=False)
    jurisdiction_id = Column(Integer, nullable=False)
    type = Column(String(50), nullable=False)
    status = Column(String(50), default="PENDING") 

    # Mối quan hệ 
    signatures = relationship("Signature", back_populates="act")

class Signature(Base):
    __tablename__ = "signature"

    id = Column(Integer, primary_key=True, index=True)
    act_id = Column(Integer, ForeignKey("notary_acts.id"), nullable=False)
    user_id = Column(Integer, nullable=False)
    order_index = Column(Integer, nullable=False)
    signature_data = Column(Text, nullable=True) # Lưu Base64
    status = Column(String(50), default="PENDING")

    # Mối quan hệ 
    act = relationship("NotaryAct", back_populates="signatures")


class JournalEntry(Base):
    __tablename__ = "journal_entries"

    id = Column(Integer, primary_key=True, index=True)
    act_id = Column(Integer, ForeignKey("notary_acts.id"), nullable=False, unique=True)
    # Lưu các checklist của màn Execution
    personal_appearance_verified = Column(Boolean, default=False)
    oath_administered = Column(Boolean, default=False)
    notes = Column(Text, nullable=True)
    # F_06: Lưu thời gian bấm nút "Lock"
    locked_at = Column(DateTime, nullable=True) 

    # Mối quan hệ
    act = relationship("NotaryAct", backref="journal")