from datetime import datetime
from pydantic import BaseModel, ConfigDict, Field

class StatusTimelineItem(BaseModel):
    """Represents a single point in the Act's history."""
    status: str
    timestamp: datetime
    is_active: bool

    model_config = ConfigDict(from_attributes=True)

class ActStatusOut(BaseModel):
    """Aggregate response for act status overview."""
    act_id: str = Field(..., description="The unique identifier for the act (e.g., ACT001)")
    timeline: list[StatusTimelineItem]
    is_legal_hold: bool

    model_config = ConfigDict(from_attributes=True)

class VoidActRequest(BaseModel):
    """Schema for voiding an act (Cancellation)."""
    reason_code: str
    additional_notes: str | None = None
    approval_required: bool = False

class LegalHoldRequest(BaseModel):
    """Schema for updating the legal hold status."""
    is_legal_hold: bool

class AuditLogItem(BaseModel):
    """Represents a single historical action logged in the audit trail."""
    timestamp: datetime
    user: str = Field(..., alias="user_name", serialization_alias="user")
    action: str
    details: str

    model_config = ConfigDict(from_attributes=True, populate_by_name=True)

class AuditLogsResponse(BaseModel):
    """Paginated list of audit logs."""
    items: list[AuditLogItem]
    total: int
