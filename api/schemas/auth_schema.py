from pydantic import BaseModel, EmailStr


class LoginRequest(BaseModel):
    email: EmailStr
    password: str


class DeviceCreate(BaseModel):
    device_type: str
    device_identifier: str
    mfa_enabled: bool = True


class DeviceResponse(BaseModel):
    id: int
    user_id: int
    device_type: str
    status: str
    mfa_enabled: bool

    class Config:
        from_attributes = True