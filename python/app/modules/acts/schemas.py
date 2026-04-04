from datetime import datetime
from enum import Enum
from pydantic import BaseModel, ConfigDict, Field
from typing import List, Optional

class ActTypeEnum(str, Enum):
    REAL_ESTATE = "Real Estate Closing"
    WILLS_TRUSTS = "Wills & Trusts"
    POWER_OF_ATTORNEY = "Power of Attorney"
    AFFIDAVITS = "Affidavits"

class StatusEnum(str, Enum):
    COMPLETED = "Completed"
    INPROCESS = "Inprocess"
    VOIDED = "Voided"

class DateRangeEnum(str, Enum):
    TODAY = "Today"
    LAST_7_DAYS = "Last 7 Days"
    LAST_30_DAYS = "Last 30 Days"
    LAST_90_DAYS = "Last 90 Days"

class ActListItem(BaseModel):
    """Represents a single act in the dashboard list view."""
    id: int
    act_type: str
    notary_name: str
    client_name: str
    created_at: datetime
    state: str
    status: str

    model_config = ConfigDict(from_attributes=True)

class ActListResponse(BaseModel):
    """Paginated list of acts for the dashboard."""
    items: List[ActListItem]
    total: int

class StatusTimelineItem(BaseModel):
    """Represents a single point in the Act's history."""
    status: str
    timestamp: datetime
    is_active: bool

    model_config = ConfigDict(from_attributes=True)

class ActStatusOut(BaseModel):
    """Aggregate response for act status overview."""
    act_id: int
    status: str
    timeline: List[StatusTimelineItem]
    is_legal_hold: bool

    model_config = ConfigDict(from_attributes=True)

class VoidActRequest(BaseModel):
    """Schema for voiding an act (Cancellation)."""
    reason_code: str
    additional_notes: Optional[str] = None
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
    items: List[AuditLogItem]
    total: int

class SetupActRequest(BaseModel):
    """Initial configuration for a new Notarial Act session."""
    act_type: str
    state: str
    document_title: str
    number_of_documents: int
