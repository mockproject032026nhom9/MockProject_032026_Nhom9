from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from core.database import get_db
from core.security import get_password_hash
from core.dependencies import get_current_user
from api.models.user_model import User
from api.schemas.user_schema import UserCreate, UserResponse
from api.schemas.response_schema import ResponseModel

router = APIRouter(prefix="/api/v1/users", tags=["1. Users Management"], dependencies=[Depends(get_current_user)])

@router.get("", response_model=ResponseModel)
def get_all_users(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    users = db.query(User).order_by(User.id.desc()).offset(skip).limit(limit).all()
    
    total = db.query(User).count()
    
    data = [UserResponse.model_validate(u).model_dump() for u in users]
    meta = {"page": (skip // limit) + 1, "limit": limit, "total_records": total}
    
    return ResponseModel(success=True, status_code=200, data=data, meta=meta)

@router.post("", response_model=ResponseModel)
def create_user(user_data: UserCreate, db: Session = Depends(get_db)):
    # Check duplicate email
    if db.query(User).filter(User.email == user_data.email).first():
        return ResponseModel(success=False, status_code=409, message="Email already exists")
        
    new_user = User(
        email=user_data.email,
        password_hash=get_password_hash(user_data.password), # Hash password
        full_name=user_data.full_name,
        phone_number=user_data.phone_number,
        dob=user_data.dob,
        address=user_data.address,
        id_role=user_data.id_role
    )
    db.add(new_user)
    db.commit()
    db.refresh(new_user)
    
    return ResponseModel(success=True, status_code=201, message="User created successfully", data=UserResponse.model_validate(new_user).model_dump())