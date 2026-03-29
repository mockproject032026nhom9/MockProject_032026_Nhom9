from pydantic import BaseModel
from typing import Optional

class ExecutionUpdate(BaseModel):
    personal_appearance_verified: bool
    oath_administered: bool
    notes: Optional[str] = None

class ElectronicSignatureCreate(BaseModel):
    signature_data: str