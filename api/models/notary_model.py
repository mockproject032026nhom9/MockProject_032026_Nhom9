from sqlalchemy import Column, Integer, String, Boolean, Date, Time, ForeignKey, Text
from core.database import Base

class Notary(Base):
    __tablename__ = "notaries"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    user_id = Column(Integer, nullable=False, index=True) # Link to users table
    ssn = Column(String(20), unique=True)
    full_name = Column(String(255))
    date_of_birth = Column(Date)
    photo_url = Column(String(255))
    phone = Column(String(50))
    email = Column(String(100))
    employment_type = Column(String(50)) # FULL_TIME, INDEPENDENT_CONTRACT
    start_date = Column(Date)
    internal_notes = Column(Text)
    status = Column(String(50), default="ACTIVE")
    residential_address = Column(Text)

class NotaryCapability(Base):
    __tablename__ = "notary_capabilities"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    notary_id = Column(Integer, ForeignKey("notaries.id"), nullable=False, index=True)
    mobile = Column(Boolean, default=False)
    RON = Column(Boolean, default=False)
    loan_signing = Column(Boolean, default=False)
    apostille_related_support = Column(Boolean, default=False)
    max_distance = Column(Integer)

class NotaryAvailability(Base):
    __tablename__ = "notary_availabilities"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    notary_id = Column(Integer, ForeignKey("notaries.id"), nullable=False, index=True)
    working_days_per_week = Column(Integer)
    start_time = Column(Time)
    end_time = Column(Time)
    fixed_days_off = Column(String(100)) 