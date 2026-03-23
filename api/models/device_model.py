from sqlalchemy import Column, Integer, String, Boolean
from core.database import Base

class Device(Base):
    __tablename__ = "devices"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    user_id = Column(Integer, nullable=False, index=True)
    device_type = Column(String(100))
    device_identifier = Column(String(255), unique=True)
    status = Column(String(50), default="ACTIVE")
    mfa_enabled = Column(Boolean, default=True)