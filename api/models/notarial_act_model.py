from sqlalchemy import Column, Integer, String, ForeignKey, Date, DateTime, Boolean, Float, Text
from core.database import Base

class NotarialAct(Base):
    __tablename__ = "notary_acts"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    request_id = Column(Integer, index=True) # Link to requests table
    notary_id = Column(Integer, index=True)
    jurisdiction_id = Column(Integer, index=True)
    type = Column(String(100)) # e.g., ACKNOWLEDGMENT, JURAT, LOAN_SIGNING
    status = Column(String(50), default="DRAFT") # DRAFT, IN_PROGRESS, COMPLETED, LOCKED, VOIDED
    
    # Execution specifics
    certificate_template = Column(String(255), nullable=True)
    venue_state = Column(String(100), nullable=True)
    venue_county = Column(String(100), nullable=True)
    locked_at = Column(DateTime, nullable=True)
    void_reason = Column(Text, nullable=True)
    seal_type = Column(String(50), nullable=True) # PHYSICAL, ELECTRONIC

class ActSigner(Base):
    __tablename__ = "act_signers"
    
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    act_id = Column(Integer, ForeignKey("notary_acts.id"), index=True)
    full_name = Column(String(255))
    role = Column(String(100)) # e.g., GRANTOR
    
    # ID Verification
    id_type = Column(String(100)) # Driver's License, Passport
    id_number = Column(String(100))
    id_authority = Column(String(100))
    id_expiry_date = Column(Date)
    id_photo_url = Column(String(255), nullable=True)
    
    verification_method = Column(String(50)) # PHYSICAL, RON
    verification_status = Column(String(50), default="PENDING") # PENDING, VERIFIED, FAILED

class JournalEntry(Base):
    __tablename__ = "journal_entries"
    
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    act_id = Column(Integer, ForeignKey("notary_acts.id"), unique=True)
    fee_charged = Column(Float, default=0.0)
    location = Column(String(255), nullable=True)
    notes = Column(Text, nullable=True)
    thumbprint_url = Column(String(255), nullable=True)
    
    # Compliance rules
    oath_administered = Column(Boolean, default=False)
    personal_appearance_verified = Column(Boolean, default=False)
