from datetime import date, datetime
from enum import Enum

from pydantic import BaseModel, ConfigDict, EmailStr, Field

class UserRole(str, Enum):
    """Enumeration for standardized user role names."""
    ADMIN = "Admin"
    DISPATCHER = "Dispatcher"
    CUSTOMER = "Customer"

class UserBase(BaseModel):
    full_name: str = Field(..., min_length=2, max_length=100)
    email: EmailStr
    phone_number: str | None = None
    status: str = "active"
    dob: date | None = None
    address: str | None = None

class UserCreate(UserBase):
    password: str = Field(..., min_length=6, max_length=100)
    role_id: int = 3  # Default to Customer (3)

class LoginSchema(BaseModel):
    """User login credentials."""
    email: EmailStr
    password: str

class UserUpdate(BaseModel):
    """Optional fields for user updates."""
    full_name: str | None = Field(None, min_length=2, max_length=100)
    email: EmailStr | None = None
    password: str | None = Field(None, min_length=6, max_length=100)
    phone_number: str | None = None
    status: str | None = None
    role_id: int | None = None
    dob: date | None = None
    address: str | None = None

class UserOut(UserBase):
    """Public user profile for API responses."""
    id: int
    created_at: datetime
    role_id: int
    
    model_config = ConfigDict(from_attributes=True)
