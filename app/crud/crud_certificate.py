from sqlalchemy.orm import Session
from app.models.act_model import NotaryAct
from app.schemas.certificate import CertificateUpdate


def get_certificate_detail(db: Session, act_id: str):
    # Lấy thông tin hiển thị lên màn hình Certificate
    return db.query(NotaryAct).filter(NotaryAct.act_id == act_id).first()


def update_certificate(db: Session, act_id: str, payload: CertificateUpdate):
    # Lưu lại các thay đổi khi người dùng chọn con dấu, sửa venue...
    act = db.query(NotaryAct).filter(NotaryAct.act_id == act_id).first()
    if not act:
        return None
    # Gán dữ liệu từ Pydantic schema vào SQLAlchemy Model
    act.venue = payload.venue
    act.seal_type = payload.seal_type
    db.commit()
    db.refresh(act)
    return act
