from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.api.deps import get_db
from app.schemas.certificate import CertificateUpdate
from app.crud import crud_certificate

router = APIRouter()


@router.get("/{act_id}/certificate", summary="Get Certificate")
def get_certificate(act_id: str, db: Session = Depends(get_db)):
    result = crud_certificate.get_certificate_detail(db=db, act_id=act_id)
    if not result:
        raise HTTPException(status_code=404, detail="Record not found")
    act, cert, signer_names = result
    return {
        "status_code": 200,
        "success": True,
        "data": {
            "act_id": str(act.id),
            "status": act.status,
            "certificate": {
                "venue": cert.venue if cert else "",
                "certificate_date": cert.certificate_date.isoformat()
                if cert and cert.certificate_date
                else None,
                "seal_type": cert.seal_type if cert else "Physical Seal Reference",
                "names_of_signers": signer_names,
            },
        },
    }


@router.patch("/{act_id}/certificate", summary="Finalize and Lock Record")
def update_certificate(
    act_id: str, payload: CertificateUpdate, db: Session = Depends(get_db)
):
    cert = crud_certificate.update_certificate(db=db, act_id=act_id, payload=payload)
    if not cert:
        raise HTTPException(status_code=404, detail="Record not found")
    return {
        "status_code": 200,
        "success": True,
        "message": "Certificate generated and Act locked successfully!",
    }
