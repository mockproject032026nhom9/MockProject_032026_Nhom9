from pydantic import BaseModel
from typing import Optional
from datetime import date, time

class NotaryResponse(BaseModel):
    id: int
    user_id: int
    full_name: str
    email: str
    phone: str
    employment_type: str
    status: str

    class Config:
        from_attributes = True

class NotaryCapabilityResponse(BaseModel):
    mobile: bool
    RON: bool
    loan_signing: bool
    max_distance: int

    class Config:
        from_attributes = True

class NotaryAvailabilityResponse(BaseModel):
    working_days_per_week: int
    start_time: time
    end_time: time
    fixed_days_off: str

    class Config:
        from_attributes = True