from typing import Any, Optional
from pydantic import BaseModel

class ResponseModel(BaseModel):
    """
    Standard API Response Wrapper for the Notarization System.
    """
    success: bool
    status_code: int
    message: str
    data: Optional[Any] = None
    errors: Optional[Any] = None
    meta: Optional[Any] = None