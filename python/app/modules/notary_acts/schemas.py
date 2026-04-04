from datetime import datetime
from pydantic import BaseModel, ConfigDict, Field, model_validator, field_validator
import re
from typing import Optional

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

class SetupActRequest(BaseModel):
    act_type: str
    state: str = Field(..., description="Please select a state")
    document_title: str
    number_of_documents: int
    number_of_signers: int
    oath_administered: bool = False
    thumbprint_required: bool = False

    @field_validator('state', mode='before')
    def validate_state(cls, v):
        if not v or not str(v).strip():
            raise ValueError("Please select a state")
        return v

    @field_validator('document_title', mode='before')
    def validate_doc_title(cls, v):
        if not v or not str(v).strip():
            raise ValueError("Please enter document information")
        title = str(v).strip()
        if len(title) > 200:
            raise ValueError("Document title exceeds the maximum allowed length")
        if re.search(r'[@#$^*]', title):
            raise ValueError("Invalid document title")
        return title

    @field_validator('number_of_documents', mode='before')
    def validate_num_docs(cls, v):
        if v is None or str(v).strip() == "":
            raise ValueError("Please enter the number of documents")
        try:
            val = int(v)
        except ValueError:
            raise ValueError("Invalid number of documents")
        
        if val <= 0:
            if val == 0:
                raise ValueError("Number of documents must be greater than 0")
            raise ValueError("Invalid number of documents")
        if val > 999:
            raise ValueError("Number of documents exceeds the allowed limit")
        return val

    @field_validator('number_of_signers', mode='before')
    def validate_num_signers(cls, v):
        if v is None or str(v).strip() == "":
            raise ValueError("Please enter the number of signers")
        try:
            val = int(v)
        except ValueError:
            raise ValueError("Invalid number of signers")
            
        if val <= 0:
            if val == 0:
                raise ValueError("Number of signers must be greater than 0")
            raise ValueError("Invalid number of signers")
        if val > 99:
            raise ValueError("Number of signers exceeds the allowed limit")
        return val

    @model_validator(mode='after')
    def validate_required_actions(self):
        if not self.oath_administered and not self.thumbprint_required:
            raise ValueError("Please select required actions")
        return self


class AddSignerRequest(BaseModel):
    full_name: str
    role: str
    id_type: Optional[str] = None
    id_number: Optional[str] = None
    id_authority: Optional[str] = None
    id_expiry_date: Optional[str] = None
    verification_method: Optional[str] = None

    @field_validator('full_name', 'role', mode='before')
    def validate_required(cls, v):
        if not v or not str(v).strip():
            raise ValueError("Singer creation failed due to missing required fields.")
        return v
        
    @field_validator('id_expiry_date', mode='before')
    def validate_expiry(cls, v):
        if v:
            try:
                if '/' in v:
                    dt = datetime.strptime(v, '%m/%d/%Y')
                else:
                    dt = datetime.strptime(v, '%Y-%m-%d')
                if dt < datetime.now():
                    raise ValueError("ID is expired. Please provide a valid ID.")
            except ValueError as e:
                if str(e).startswith("ID"):
                    raise e
        return v

    @model_validator(mode='after')
    def validate_id_verification(self):
        if self.id_type:
            if not self.id_number:
                raise ValueError("ID Number cannot be left blank")
            if not self.id_authority:
                raise ValueError("Issuing Authority cannot be left blank")
        return self

