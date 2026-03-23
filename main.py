from fastapi import FastAPI
from core.database import Base, engine
from api.routers import auth_router, user_router, job_router, notary_router, signature_router


Base.metadata.create_all(bind=engine)

app = FastAPI(
    title="Notarization Management API",
    description="API for the Notarization Management System - Team 9",
    version="1.0.0"
)

@app.get("/")
def root():
    return {"message": "Welcome to Notarization Management API - Team Python is ready!"}

app.include_router(auth_router.router, prefix="/api/auth", tags=["0. Authentication"])
app.include_router(user_router.router)
app.include_router(job_router.router)
app.include_router(notary_router.router)
app.include_router(signature_router.router)