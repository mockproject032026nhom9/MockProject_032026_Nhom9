from pydantic import BaseModel
from typing import Optional
from datetime import datetime


class CertificateUpdate(BaseModel):
    venue: Optional[str] = None
    certificate_date: Optional[datetime] = None
    seal_type: Optional[str] = None


class CertificateDetailResponse(BaseModel):
    act_id: str
    act_type: str
    state: str
    venue: Optional[str]
    date: Optional[str]
    seal_type: Optional[str]
    is_locked: bool
