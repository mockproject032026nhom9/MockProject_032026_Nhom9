from typing import Sequence

from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.exceptions import AppException
from app.core.schemas import Token
from app.core.security import create_access_token, get_password_hash, verify_password
from app.modules.users.models import User
from app.modules.users.schemas import UserCreate, UserUpdate

class UserService:
    """Business logic for User operations."""
    
    async def get_all(self, db: AsyncSession) -> Sequence[User]:
        """Fetch all users from the database."""
        result = await db.execute(select(User))
        return result.scalars().all()

    async def get_by_id(self, db: AsyncSession, user_id: int) -> User | None:
        """Fetch a single user by ID."""
        result = await db.execute(select(User).filter(User.id == user_id))
        return result.scalars().first()

    async def get_by_email(self, db: AsyncSession, email: str) -> User | None:
        """Fetch a single user by email."""
        result = await db.execute(select(User).filter(User.email == email))
        return result.scalars().first()

    async def create(self, db: AsyncSession, user_in: UserCreate) -> User:
        """Create a new user with password hashing and role_id."""
        if await self.get_by_email(db, user_in.email):
            raise AppException("This email address is already registered.", status_code=400)
            
        user_data = user_in.model_dump()
        user_data["password_hash"] = get_password_hash(user_data.pop("password"))
        
        user = User(**user_data)
        db.add(user)
        await db.commit()
        await db.refresh(user)
        return user

    async def update(self, db: AsyncSession, user_id: int, user_in: UserUpdate) -> User | None:
        """Update existing user properties."""
        user = await self.get_by_id(db, user_id)
        if not user:
            return None
        
        update_data = user_in.model_dump(exclude_unset=True)
        if "password" in update_data:
            update_data["password_hash"] = get_password_hash(update_data.pop("password"))
            
        for key, value in update_data.items():
            setattr(user, key, value)
        
        await db.commit()
        await db.refresh(user)
        return user

    async def delete(self, db: AsyncSession, user_id: int) -> bool:
        """Permanently delete a user account."""
        user = await self.get_by_id(db, user_id)
        if not user:
            return False
        await db.delete(user)
        await db.commit()
        return True

    async def authenticate(self, db: AsyncSession, email: str, password: str) -> User | None:
        """Authenticate user credentials returns user object or None."""
        user = await self.get_by_email(db, email)
        if not user or not verify_password(password, user.password_hash):
            return None
        return user

    def create_token(self, user: User) -> Token:
        """Generate a minimalist JWT access token with sub and role name."""
        role_name = user.role.name if user.role else "Customer"
        access_token = create_access_token(data={
            "sub": str(user.id), 
            "role": role_name
        })
        return Token(access_token=access_token)

user_service = UserService()
