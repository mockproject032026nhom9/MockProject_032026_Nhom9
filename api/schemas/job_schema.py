from pydantic import BaseModel
from typing import Optional
from datetime import datetime

class JobCreate(BaseModel):
    service_type: str
    location_type: str
    location_details: str
    requested_start_time: datetime
    requested_end_time: datetime
    signer_count: int

class JobResponse(BaseModel):
    id: int
    client_id: int
    service_type: str
    location_type: str
    location_details: str
    requested_start_time: datetime
    requested_end_time: datetime
    signer_count: int
    status: str

    class Config:
        from_attributes = True

class AssignmentCreate(BaseModel):
    notary_id: int

class JobStatusUpdate(BaseModel):
    status: str
    delay: Optional[str] = None
    exception_flags: Optional[str] = None
    note: Optional[str] = None