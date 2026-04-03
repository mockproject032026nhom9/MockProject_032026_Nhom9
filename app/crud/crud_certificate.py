from sqlalchemy.orm import Session, joinedload
from app.models.act_model import NotaryAct, Certificate
from app.schemas.certificate import CertificateUpdate

def get_certificate_detail(db: Session, act_id: str):
    act = (
        db.query(NotaryAct)
        .options(
            joinedload(NotaryAct.certificate),
            joinedload(NotaryAct.signatures)
        )
        .filter(NotaryAct.id == int(act_id))
        .first()
    )
    if not act:
        return None
    signer_names = ", ".join([f"User_{sig.user_id}" for sig in act.signatures]) if act.signatures else "No signers yet"
    return act, act.certificate, signer_names

def update_certificate(db: Session, act_id: str, payload: CertificateUpdate):
    act = db.query(NotaryAct).filter(NotaryAct.id == int(act_id)).first()
    if not act:
        return None
    cert = db.query(Certificate).filter(Certificate.act_id == int(act_id)).first()
    if not cert:
        cert = Certificate(act_id=int(act_id))
        db.add(cert)

    cert.venue = payload.venue
    cert.certificate_date = payload.certificate_date
    cert.seal_type = payload.seal_type

    # Nút "Finalize and Lock Record" Chốt sổ Act!
    act.status = "LOCKED" 
    db.commit()
    db.refresh(cert)
    return cert