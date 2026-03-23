from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from core.database import get_db
from core.security import verify_password, create_access_token
from core.dependencies import get_current_user
from api.models.user_model import User
from api.models.device_model import Device
from api.schemas.auth_schema import LoginRequest, DeviceCreate, DeviceResponse
from api.schemas.user_schema import UserResponse
from api.schemas.response_schema import ResponseModel

router = APIRouter()

@router.post("/login", response_model=ResponseModel)
def login(credentials: LoginRequest, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.email == credentials.email).first()
    
    if not user or not verify_password(credentials.password, user.password_hash):
        return ResponseModel(success=False, status_code=401, message="Incorrect email or password")
    
    if user.status.lower() != "active":
        return ResponseModel(success=False, status_code=403, message="Account is locked")

    access_token = create_access_token(data={"sub": user.email, "role": user.id_role})
    
    return ResponseModel(
        success=True, 
        status_code=200, 
        data={"access_token": access_token, "user": UserResponse.model_validate(user).model_dump()}
    )

@router.post("/devices", response_model=ResponseModel)
def register_device(device_data: DeviceCreate, db: Session = Depends(get_db), current_user: User = Depends(get_current_user)):
    new_device = Device(
        user_id=current_user.id,
        device_type=device_data.device_type,
        device_identifier=device_data.device_identifier,
        mfa_enabled=device_data.mfa_enabled
    )
    db.add(new_device)
    db.commit()
    db.refresh(new_device)
    
    return ResponseModel(success=True, status_code=201, message="Device registered successfully", data=DeviceResponse.model_validate(new_device).model_dump())