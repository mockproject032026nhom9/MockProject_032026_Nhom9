from fastapi import FastAPI
from core.database import Base, engine
from api.routers import notarial_act_router


Base.metadata.create_all(bind=engine)

app = FastAPI(
    title="Notarization Management API",
    description="API for the Notarization Management System - Team 9",
    version="1.0.0"
)

@app.get("/")
def root():
    return {"message": "Welcome to Notarization Management API - Team Python is ready!"}

app.include_router(notarial_act_router.router)