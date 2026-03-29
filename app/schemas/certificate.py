from pydantic import BaseModel
from typing import Optional

class CertificateUpdate(BaseModel):
    venue: str
    date: str
    seal_type: str

class CertificateDetailResponse(BaseModel):
    act_id: str
    act_type: str
    state: str
    venue: Optional[str]
    date: Optional[str]
    seal_type: Optional[str]
    is_locked: bool