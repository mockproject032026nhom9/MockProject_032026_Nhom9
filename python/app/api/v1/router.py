from fastapi import APIRouter

from app.modules.users.router import router as user_router
from app.modules.journal_entries.router import router as journal_router
from app.modules.media.router import router as media_router
from app.modules.acts.router import router as acts_router
from app.modules.references.router import router as references_router

api_router = APIRouter()

# Combine all domain routers into a single versioned API router.
api_router.include_router(user_router)
api_router.include_router(journal_router)
api_router.include_router(media_router)
api_router.include_router(acts_router)
api_router.include_router(references_router)
