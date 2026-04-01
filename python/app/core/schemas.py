from pydantic import BaseModel
from typing import Generic, TypeVar

T = TypeVar("T")

class ApiResponse(BaseModel, Generic[T]):
    """Standard API response envelope."""
    status_code: int = 200
    success: bool = True
    message: str = "Success"
    data: T | None = None

class Token(BaseModel):
    """Schema for JWT access tokens."""
    access_token: str
    token_type: str = "bearer"

class TokenData(BaseModel):
    """Essential data extracted from the JWT payload."""
    sub: str | None = None
    role: str | None = None

class PaginationParams(BaseModel):
    """Parameters for list pagination."""
    page: int = 1
    size: int = 20
    
class PaginatedResponse(BaseModel, Generic[T]):
    """Standard paginated response structure."""
    items: list[T]
    total: int
    page: int
    size: int
    pages: int
