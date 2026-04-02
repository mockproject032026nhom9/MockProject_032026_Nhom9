from pydantic import BaseModel
from typing import Optional, List
from datetime import date, datetime

# Setup Act (Step 1)
class SetupActRequest(BaseModel):
    request_id: int
    notary_id: int
    jurisdiction_id: int
    type: str # e.g. ACKNOWLEDGMENT, JURAT
    venue_state: Optional[str] = None
    venue_county: Optional[str] = None

# Add Signer (Step 2)
class AddSignerRequest(BaseModel):
    full_name: str
    role: str
    id_type: str
    id_number: str
    id_authority: str
    id_expiry_date: date
    id_photo_url: Optional[str] = None
    verification_method: str # PHYSICAL, RON

# Execute Act (Step 3)
class ExecuteActRequest(BaseModel):
    fee_charged: float = 0.0
    location: Optional[str] = None
    notes: Optional[str] = None
    thumbprint_url: Optional[str] = None
    oath_administered: bool = False
    personal_appearance_verified: bool = False

# Update Act (only status)
class UpdateActRequest(BaseModel):
    status: Optional[str] = None

# Update Signer
class UpdateSignerRequest(BaseModel):
    full_name: Optional[str] = None
    role: Optional[str] = None
    id_type: Optional[str] = None
    id_number: Optional[str] = None
    id_authority: Optional[str] = None
    id_expiry_date: Optional[date] = None
    id_photo_url: Optional[str] = None
    verification_method: Optional[str] = None

# Responses
class NotarialActResponse(BaseModel):
    id: int
    request_id: int
    notary_id: int
    jurisdiction_id: int
    type: str
    status: str
    locked_at: Optional[datetime] = None

    class Config:
        from_attributes = True

class ActSignerResponse(AddSignerRequest):
    id: int
    act_id: int
    verification_status: str

    class Config:
        from_attributes = True

class JournalEntryResponse(ExecuteActRequest):
    id: int
    act_id: int

    class Config:
        from_attributes = True
