from typing import Any, Dict, Optional
from pydantic import BaseModel


class ResponseModel(BaseModel):
    success: bool
    status_code: int
    message: Optional[str] = None
    data: Optional[Any] = None
    meta: Optional[Dict[str, Any]] = None