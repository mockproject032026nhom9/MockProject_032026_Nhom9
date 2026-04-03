from typing import Sequence

from fastapi import APIRouter, Depends, status
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.database import get_db
from app.core.dependencies import RoleChecker, get_current_user
from app.core.schemas import ApiResponse, Token
from app.modules.users.models import User
from app.modules.users.schemas import (
    LoginSchema,
    UserCreate,
    UserOut,
    UserRole,
    UserUpdate,
)
from app.modules.users.services import user_service

router = APIRouter(prefix="/users", tags=["Users"])

@router.post("/register", response_model=ApiResponse[UserOut], status_code=status.HTTP_201_CREATED)
async def register(user_in: UserCreate, db: AsyncSession = Depends(get_db)):
    """User self-registration."""
    user = await user_service.create(db, user_in)
    return ApiResponse(
        statusCode=201,
        message="User registered successfully",
        data=user
    )

from fastapi.security import OAuth2PasswordRequestForm

@router.post("/login", response_model=Token)
async def login(form_data: OAuth2PasswordRequestForm = Depends(), db: AsyncSession = Depends(get_db)):
    """Authenticate and return a JWT access token."""
    user = await user_service.authenticate(db, form_data.username, form_data.password)
    if not user:
        raise HTTPException(
            status_code=401,
            detail="Invalid email or password",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    return user_service.create_token(user)

@router.get("/me", response_model=ApiResponse[UserOut])
async def get_me(current_user: User = Depends(get_current_user)):
    """Retrieve the current authenticated user's profile."""
    return ApiResponse(data=current_user)

@router.get("/", response_model=ApiResponse[Sequence[UserOut]], dependencies=[Depends(RoleChecker([UserRole.ADMIN]))])
async def get_all_users(db: AsyncSession = Depends(get_db)):
    """ADMIN ONLY: Retrieve all users in the system."""
    users = await user_service.get_all(db)
    return ApiResponse(data=users)

@router.get("/{user_id}", response_model=ApiResponse[UserOut], dependencies=[Depends(RoleChecker([UserRole.ADMIN]))])
async def get_user_by_id(user_id: int, db: AsyncSession = Depends(get_db)):
    """ADMIN ONLY: Retrieve a user by ID."""
    user = await user_service.get_by_id(db, user_id)
    if not user:
        return ApiResponse(statusCode=404, success=False, message="User not found")
    return ApiResponse(data=user)

@router.patch("/{user_id}", response_model=ApiResponse[UserOut], dependencies=[Depends(RoleChecker([UserRole.ADMIN]))])
async def update_user(user_id: int, user_in: UserUpdate, db: AsyncSession = Depends(get_db)):
    """ADMIN ONLY: Update a user's details or role."""
    user = await user_service.update(db, user_id, user_in)
    if not user:
        return ApiResponse(statusCode=404, success=False, message="User not found")
    return ApiResponse(
        message="User updated successfully",
        data=user
    )

@router.delete("/{user_id}", response_model=ApiResponse[bool], dependencies=[Depends(RoleChecker([UserRole.ADMIN]))])
async def delete_user(user_id: int, db: AsyncSession = Depends(get_db)):
    """ADMIN ONLY: Permanently remove a user account."""
    success = await user_service.delete(db, user_id)
    if not success:
        return ApiResponse(statusCode=404, success=False, message="User not found")
    return ApiResponse(message="User deleted", data=True)
