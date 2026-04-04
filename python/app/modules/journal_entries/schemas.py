from pydantic import BaseModel, ConfigDict, Field, field_serializer, computed_field
from datetime import datetime
from typing import List

class SignerOut(BaseModel):
    name: str = Field(..., alias="full_name")
    role: str
    id_number: str | None = None
    status: str
    signature_url: str | None = None
    thumbprint_url: str | None = None

    model_config = ConfigDict(from_attributes=True, populate_by_name=True)

class ComplianceOut(BaseModel):
    identity_verification: bool
    mandatory_fields: bool
    final_notary_seal: bool

    model_config = ConfigDict(from_attributes=True)

class JournalEntryUpdate(BaseModel):
    """Schema for updating journal entry with manual inputs and biometric associations."""
    fee_charged: float | None = None
    state: str | None = None
    location: str | None = None
    notary_notes: str | None = None
    signature_url: str | None = None
    thumbprint_url: str | None = None

class JournalEntryOut(BaseModel):
    id: int
    created_at: datetime
    act_type: str
    notarial_fee: float
    signers: List[SignerOut]
    compliance: ComplianceOut

    model_config = ConfigDict(from_attributes=True)

    @computed_field
    @property
    def status(self) -> str:
        """Dynamically source status from the parent Notary Act session."""
        return self.notary_act.status if hasattr(self, "notary_act") else "UNKNOWN"

    @field_serializer('id')
    def serialize_id(self, v: int) -> str:
        """Serializing integer ID as string to match requested API format."""
        return str(v)
