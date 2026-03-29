from sqlalchemy import Column, Integer, String, Text, ForeignKey
from sqlalchemy.orm import relationship
from app.db.database import Base

class NotaryAct(Base):
    __tablename__ = "notary_acts"

    id = Column(Integer, primary_key=True, index=True)
    request_id = Column(Integer, nullable=False)
    notary_id = Column(Integer, nullable=False)
    jurisdiction_id = Column(Integer, nullable=False)
    type = Column(String(50), nullable=False)
    status = Column(String(50), default="PENDING") 

    # Mối quan hệ với bảng signature
    signatures = relationship("Signature", back_populates="act")

class Signature(Base):
    __tablename__ = "signature"

    id = Column(Integer, primary_key=True, index=True)
    act_id = Column(Integer, ForeignKey("notary_acts.id"), nullable=False)
    user_id = Column(Integer, nullable=False)
    order_index = Column(Integer, nullable=False)
    signature_data = Column(Text, nullable=True) # Lưu Base64
    status = Column(String(50), default="PENDING")

    # Mối quan hệ ngược lại
    act = relationship("NotaryAct", back_populates="signatures")