from fastapi import Request, status
from fastapi.responses import JSONResponse
from fastapi.exceptions import RequestValidationError
from starlette.exceptions import HTTPException as StarletteHTTPException
from typing import Any

class AppException(Exception):
    """
    Standard Base Exception for business logic errors. 
    Returns internal status codes in a consistent API format.
    """
    def __init__(self, message: str, status_code: int = 400, data: Any = None):
        self.message = message
        self.status_code = status_code
        self.data = data

async def app_exception_handler(request: Request, exc: AppException):
    """Handler for business-related exceptions (AppException)."""
    return JSONResponse(
        status_code=exc.status_code,
        content={
            "statusCode": exc.status_code,
            "success": False,
            "message": exc.message,
            "data": exc.data
        }
    )

async def validation_exception_handler(request: Request, exc: RequestValidationError):
    """Handler for Pydantic validation errors (422 Unprocessable Entity)."""
    errors = exc.errors()
    msg = "Validation Error"
    if errors and len(errors) > 0:
        # Extract the specific custom error message if available (e.g., from raise ValueError)
        msg = errors[0].get("msg", msg)
        # Pydantic prepends 'Value error, ' for custom validators, strip it out cleanly
        if msg.startswith("Value error, "):
            msg = msg.replace("Value error, ", "", 1)
            
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content={
            "statusCode": 422,
            "success": False,
            "message": msg,
            "data": None
        }
    )

async def http_exception_handler(request: Request, exc: StarletteHTTPException):
    """Handler for standard FastAPI HTTP exceptions."""
    return JSONResponse(
        status_code=exc.status_code,
        content={
            "statusCode": exc.status_code,
            "success": False,
            "message": str(exc.detail),
            "data": None
        }
    )
