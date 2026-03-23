from pydantic import BaseModel, EmailStr
from typing import Optional
from datetime import datetime

class UserCreate(BaseModel):
    email: EmailStr
    password: str
    full_name: str
    phone_number: str
    dob: Optional[str] = None
    address: Optional[str] = None
    id_role: int

class UserResponse(BaseModel):
    id: int
    email: str
    full_name: str
    phone_number: str
    status: str
    id_role: int
    created_at: datetime

    class Config:
        from_attributes = True