from typing import Any

from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.database import get_db
from app.core.security import decode_access_token
from app.modules.users.models import User
from app.modules.users.schemas import UserRole
from app.modules.users.services import user_service

# Standard OAuth2 scheme for Bearer token extraction
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/api/v1/users/login")

async def get_current_user(
    db: AsyncSession = Depends(get_db), 
    token: str = Depends(oauth2_scheme)
) -> User:
    """
    Standard authentication dependency.
    Validates the bearer token and returns the current user.
    """
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    
    payload = decode_access_token(token)
    if not payload:
        raise credentials_exception
    
    user_id = payload.get("sub")
    if not user_id:
        raise credentials_exception
        
    user = await user_service.get_by_id(db, int(user_id))
    if not user:
        raise credentials_exception
        
    return user

class RoleChecker:
    """
    Minimalist RBAC dependency. 
    Usage: Depends(RoleChecker([UserRole.ADMIN]))
    """
    def __init__(self, allowed_roles: list[UserRole]):
        self.allowed_roles = [r.value for r in allowed_roles]

    def __call__(self, user: User = Depends(get_current_user)) -> User:
        """Enforces role check before allowing access to a route."""
        role_name = user.role.name if user.role else None
        
        if role_name not in self.allowed_roles:
            raise HTTPException(
                status_code=status.HTTP_403_FORBIDDEN,
                detail="Not enough permissions",
            )
        return user
