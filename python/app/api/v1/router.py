from fastapi import APIRouter

from app.modules.users.router import router as user_router

api_router = APIRouter()

# Combine all domain routers into a single versioned API router.
api_router.include_router(user_router)
