from sqlalchemy import Column, Integer, String, DateTime, Text, Boolean
from datetime import datetime
from core.database import Base

class User(Base):
    __tablename__ = "users"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    email = Column(String(255), unique=True, index=True, nullable=False)
    password_hash = Column(String(255), nullable=False)
    phone_number = Column(String(50))
    status = Column(String(50), default="active")
    created_at = Column(DateTime, default=datetime.utcnow)
    full_name = Column(String(255))
    dob = Column(String(50))
    address = Column(Text)
    id_role = Column(Integer, nullable=False) 