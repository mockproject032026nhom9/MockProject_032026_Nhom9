from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.api.deps import get_db, get_current_user
from app.schemas.certificate import CertificateUpdate
from app.crud import crud_certificate

router = APIRouter()


@router.get("/{act_id}/certificate", summary="Lấy chi tiết màn hình Certificate")
def get_certificate(
    act_id: str,
    db: Session = Depends(get_db),
    current_user: dict = Depends(get_current_user),
):
    act = crud_certificate.get_certificate_detail(db=db, act_id=act_id)
    if not act:
        raise HTTPException(status_code=404, detail="Không tìm thấy hồ sơ")

    return {
        "status_code": 200,
        "success": True,
        "data": {"act_id": act.act_id, "venue": act.venue, "seal_type": act.seal_type},
    }


@router.patch("/{act_id}/certificate", summary="Lưu cấu hình Certificate")
def update_certificate(
    act_id: str,
    payload: CertificateUpdate,
    db: Session = Depends(get_db),
    current_user: dict = Depends(get_current_user),
):
    act = crud_certificate.update_certificate(db=db, act_id=act_id, payload=payload)
    if not act:
        raise HTTPException(status_code=404, detail="Không tìm thấy hồ sơ")

    return {
        "status_code": 200,
        "success": True,
        "message": "Đã lưu Certificate thành công!",
    }
