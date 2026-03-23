from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from core.database import get_db
from core.dependencies import get_current_user
from api.models.user_model import User
from api.models.notary_model import Notary, NotaryCapability, NotaryAvailability
from api.schemas.notary_schema import (
    NotaryResponse,
    NotaryCapabilityResponse,
    NotaryAvailabilityResponse,
)
from api.schemas.response_schema import ResponseModel

router = APIRouter(prefix="/api/notaries", tags=["2. Notary Profiles"], dependencies=[Depends(get_current_user)])

@router.get("", response_model=ResponseModel)
def get_all_notaries(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    """Get a list of all notaries."""
    notaries = db.query(Notary).order_by(Notary.id.desc()).offset(skip).limit(limit).all()
    total = db.query(Notary).count()
    
    data = [NotaryResponse.model_validate(n).model_dump() for n in notaries]
    meta = {"page": (skip // limit) + 1, "limit": limit, "total_records": total}
    
    return ResponseModel(success=True, status_code=200, data=data, meta=meta)

@router.get("/{notary_id}/capabilities", response_model=ResponseModel)
def get_notary_capabilities(notary_id: int, db: Session = Depends(get_db)):
    """Get capability details for a specific notary."""
    caps = db.query(NotaryCapability).filter(NotaryCapability.notary_id == notary_id).first()
    if not caps:
        return ResponseModel(success=False, status_code=404, message="Capabilities have not been configured for this notary")
        
    return ResponseModel(
        success=True, 
        status_code=200, 
        data=NotaryCapabilityResponse.model_validate(caps).model_dump()
    )

@router.get("/{notary_id}/availabilities", response_model=ResponseModel)
def get_notary_availabilities(notary_id: int, db: Session = Depends(get_db)):
    """Get working availability for a specific notary."""
    avail = db.query(NotaryAvailability).filter(NotaryAvailability.notary_id == notary_id).first()
    if not avail:
        return ResponseModel(success=False, status_code=404, message="Availability has not been configured for this notary")
        
    return ResponseModel(
        success=True, 
        status_code=200, 
        data=NotaryAvailabilityResponse.model_validate(avail).model_dump()
    )