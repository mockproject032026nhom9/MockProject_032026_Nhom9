from fastapi import APIRouter, Depends, Request, HTTPException
from sqlalchemy.orm import Session
import uuid
from datetime import datetime
from core.database import get_db
from core.dependencies import get_current_user
from api.models.signature_model import Certificate, DigitalSignature, Seal
from api.models.notary_model import Notary
from api.models.device_model import Device
from api.schemas.signature_schema import SigningRequest, DigitalSignatureResponse
from api.schemas.response_schema import ResponseModel

router = APIRouter(prefix="/api/signatures", tags=["4. Digital Signatures"], dependencies=[Depends(get_current_user)])

@router.post("/sign", response_model=ResponseModel)
def sign_document(data: SigningRequest, request: Request, db: Session = Depends(get_db), current_user = Depends(get_current_user)):
    """
    Enterprise digital signing flow:
    1. Verify the device belongs to the current user.
    2. Verify the certificate is valid.
    3. Create a digital signature record.
    """
    # 1. Check Device
    device = db.query(Device).filter(Device.id == data.device_id, Device.user_id == current_user.id).first()
    if not device:
        return ResponseModel(success=False, status_code=403, message="Invalid signing device")

    # 2. Check Certificate
    cert = db.query(Certificate).filter(Certificate.id == data.certificate_id, Certificate.owner_user_id == current_user.id).first()
    if not cert or cert.status != "ACTIVE":
        return ResponseModel(success=False, status_code=400, message="Digital certificate is expired or revoked")

    # 3. Create a digital signature (simulated private-key signing)
    new_sig = DigitalSignature(
        user_id=current_user.id,
        certificate_id=cert.id,
        device_id=device.id,
        document_hash=data.document_hash,
        signature_value=f"SIG-PKI-{uuid.uuid4().hex.upper()}",
        signed_at=datetime.utcnow(),
        ip_address=request.client.host,
        verification_status="VERIFIED"
    )
    
    db.add(new_sig)
    db.commit()
    db.refresh(new_sig)

    return ResponseModel(
        success=True, 
        status_code=201, 
        message="Document signed successfully", 
        data=DigitalSignatureResponse.model_validate(new_sig).model_dump()
    )

@router.get("/my-seals", response_model=ResponseModel)
def get_my_seals(db: Session = Depends(get_db), current_user = Depends(get_current_user)):
    """Get seals that belong to the currently logged-in notary."""

    notary = db.query(Notary).filter(Notary.user_id == current_user.id).first()

    if not notary:
        return ResponseModel(success=False, status_code=403, message="This account does not have a notary profile")

    seals = db.query(Seal).filter(Seal.notary_id == notary.id).all() 

    data = [s.id for s in seals]
    
    return ResponseModel(success=True, status_code=200, data=data)