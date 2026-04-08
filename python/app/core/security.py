import bcrypt
import jwt
from datetime import datetime, timedelta, timezone
from typing import Any

from app.core.config import settings

def get_password_hash(password: str) -> str:
    """Hash a raw password using bcrypt."""
    pw_bytes = password.encode('utf-8')
    salt = bcrypt.gensalt()
    return bcrypt.hashpw(pw_bytes, salt).decode('utf-8')

def verify_password(plain_password: str, hashed_password: str) -> bool:
    """Verify a raw password against its bcrypt hash."""
    pw_bytes = plain_password.encode('utf-8')
    hash_bytes = hashed_password.encode('utf-8')
    return bcrypt.checkpw(pw_bytes, hash_bytes)

def create_access_token(data: dict[str, Any], expires_delta: timedelta | None = None) -> str:
    """
    Generate a JWT access token with essential claims.
    Standard claims: sub, exp.
    Custom claims: role.
    """
    to_encode = data.copy()
    expire = datetime.now(timezone.utc) + (expires_delta or timedelta(minutes=settings.ACCESS_TOKEN_EXPIRE_MINUTES))
    
    to_encode.update({"exp": expire})
    return jwt.encode(to_encode, settings.SECRET_KEY, algorithm=settings.ALGORITHM)

def decode_access_token(token: str) -> dict[str, Any] | None:
    """
    Decode and validate a JWT access token.
    Returns the payload if valid, otherwise None.
    """
    try:
        return jwt.decode(token, settings.SECRET_KEY, algorithms=[settings.ALGORITHM])
    except jwt.PyJWTError:
        return None
