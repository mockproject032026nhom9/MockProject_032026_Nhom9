from contextlib import asynccontextmanager

from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.exceptions import RequestValidationError
from fastapi.staticfiles import StaticFiles
from sqlalchemy import select
from starlette.exceptions import HTTPException as StarletteHTTPException

from app.api.v1.router import api_router
from app.core.config import settings
from app.core.database import Base, SessionLocal, engine
from app.core.exceptions import (
    AppException,
    app_exception_handler,
    http_exception_handler,
    validation_exception_handler,
)
from app.core.logger import logger
from app.modules.roles.services import role_service

@asynccontextmanager
async def lifespan(app: FastAPI):
    """Lifecycle event handler for application startup and shutdown."""
    logger.info("Starting up FastAPI application...")
    
    # Initialize Database Schema
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)
    
    # Seed mandatory roles from the dedicated roles service
    async with SessionLocal() as db:
        await role_service.seed_roles(db)
        
    yield
    
    logger.info("Shutting down FastAPI application...")
    await engine.dispose()

app = FastAPI(
    title=settings.PROJECT_NAME,
    version=settings.VERSION,
    lifespan=lifespan,
    docs_url="/docs",
    redoc_url="/redoc"
)

# Mount Static Files (Local Storage for Signatures, etc.)
# Logic: Ensure static directory exists before mounting
import os
if not os.path.exists("uploads"):
    os.makedirs("uploads")

app.mount("/uploads", StaticFiles(directory="uploads"), name="uploads")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Global Exception Handlers
app.add_exception_handler(AppException, app_exception_handler)
app.add_exception_handler(RequestValidationError, validation_exception_handler)
app.add_exception_handler(StarletteHTTPException, http_exception_handler)

# Health check endpoint
@app.get("/", tags=["Health"])
async def root():
    """Health check endpoint. Returns application status and version."""
    return {
        "statusCode": 200,
        "success": True,
        "message": f"Connected to {settings.PROJECT_NAME} API Service",
        "version": settings.VERSION
    }

# Versioned API Routes
app.include_router(api_router, prefix=settings.API_V1_STR)
