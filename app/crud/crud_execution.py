from sqlalchemy.orm import Session
from datetime import datetime
from app.models.act_model import NotaryAct, Signature, JournalEntry
from app.schemas.execution import ExecutionUpdate, ElectronicSignatureCreate


def get_execution_detail(db: Session, act_id: str):
    act = db.query(NotaryAct).filter(NotaryAct.id == int(act_id)).first()
    if not act:
        return None
    signatures = db.query(Signature).filter(Signature.act_id == int(act_id)).all()
    # Lấy thêm data từ bảng Journal
    journal = db.query(JournalEntry).filter(JournalEntry.act_id == int(act_id)).first()
    return act, signatures, journal


def lock_execution_time(db: Session, act_id: str):
    # Xử lý nút Lock Time
    journal = db.query(JournalEntry).filter(JournalEntry.act_id == int(act_id)).first()
    if not journal:
        journal = JournalEntry(act_id=int(act_id))
        db.add(journal)

    journal.locked_at = datetime.utcnow()
    db.commit()
    db.refresh(journal)
    return journal


def update_execution_status(db: Session, act_id: str, payload: ExecutionUpdate):
    act = db.query(NotaryAct).filter(NotaryAct.id == int(act_id)).first()
    if not act:
        return None, "Record not found"

    # Cập nhật Journal
    journal = db.query(JournalEntry).filter(JournalEntry.act_id == int(act_id)).first()
    if not journal:
        journal = JournalEntry(act_id=int(act_id))
        db.add(journal)

    journal.personal_appearance_verified = payload.personal_appearance_verified
    journal.oath_administered = payload.oath_administered
    journal.notes = payload.notes

    # Xử lý logic đổi trạng thái
    if payload.action == "COMPLETE":
        # Bấm Complete mà chưa Lock Time thì báo lỗi
        if not journal.locked_at:
            return None, "Act completion time must be locked"
        # Loại Jurat bắt buộc phải có (Oath)
        if act.type and act.type.upper() == "JURAT" and not journal.oath_administered:
            return None, "Oath/Affirmation is required for Jurat type"
        # Bắt buộc phải có ít nhất 1 chữ ký trước khi Complete
        signatures = db.query(Signature).filter(Signature.act_id == act.id).all()
        if not signatures:
            return None, "Please collect the signer's signature"
        # Đủ điều kiện -> Chuyển status
        act.status = "COMPLETED"
    elif payload.action == "SAVE_LATER":
        # Bấm Save for Later -> Đổi thành In Progress
        act.status = "IN_PROGRESS"
    db.commit()
    return act, "Success"


def save_electronic_signature(
    db: Session, act_id: str, signer_id: str, payload: ElectronicSignatureCreate
):
    existing_sig = (
        db.query(Signature)
        .filter(
            Signature.act_id == act_id,
            Signature.user_id == signer_id,
        )
        .first()
    )

    if existing_sig:
        existing_sig.signature_data = payload.signature_data
        existing_sig.status = "SIGNED"
        sig_record = existing_sig
    else:
        new_sig = Signature(
            act_id=act_id,
            user_id=signer_id,
            order_index=1,
            signature_data=payload.signature_data,
            status="SIGNED",
        )
        db.add(new_sig)
        sig_record = new_sig

    db.commit()
    db.refresh(sig_record)
    return sig_record
