from fastapi import APIRouter
from app.api.v1.endpoints import execution, certificate

api_router = APIRouter()

api_router.include_router(execution.router, prefix="/notarial-acts", tags=["Execution (SC005)"])
api_router.include_router(certificate.router, prefix="/notarial-acts", tags=["Certificate (SC006)"])