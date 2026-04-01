from pydantic import BaseModel, ConfigDict

class VoidReasonOut(BaseModel):
    """Schema for Void Reason lookup items."""
    code: str
    name: str

    model_config = ConfigDict(from_attributes=True)
