from fastapi import FastAPI
from routers import user_router

app = FastAPI(
    title="Notarization API",
    description="Sample CRUD to verify Day 2 conventions"
)

app.include_router(user_router.router)