from fastapi import APIRouter, Depends, HTTPException, UploadFile, File
from sqlalchemy.orm import Session
from app.api.deps import get_db, get_current_user
from app.schemas.execution import ExecutionUpdate, ElectronicSignatureCreate
from app.crud import crud_execution

router = APIRouter()


@router.get("/{act_id}/excution")
def get_execution(act_id: str, db: Session = Depends(get_db)):
    result = crud_execution.get_execution_detail(db=db, act_id=act_id)
    if not result:
        raise HTTPException(status_code=404, detail="Record not found")

    act, signatures, journal = result

    return {
        "status_code": 200,
        "success": True,
        "data": {
            "act_id": str(act.id),
            "status": act.status,
            "execution": {
                "personal_appearance_verified": journal.personal_appearance_verified
                if journal
                else False,
                "oath_administered": journal.oath_administered if journal else False,
                "notes": journal.notes if journal else "",
                "locked_at": journal.locked_at.isoformat()
                if journal and journal.locked_at
                else None,
            },
            "signers": [
                {"signer_id": sig.user_id, "status": sig.status} for sig in signatures
            ],
        },
    }


@router.post("/{act_id}/lock-time", summary="Execution time lock")
def lock_time(act_id: str, db: Session = Depends(get_db)):
    crud_execution.lock_execution_time(db, act_id)
    return {"status_code": 200, "success": True, "message": "Time locked successfully"}


@router.patch("/{act_id}/excution", summary="Update & Finalize Record")
def update_execution(
    act_id: str, payload: ExecutionUpdate, db: Session = Depends(get_db)
):
    act, error_msg = crud_execution.update_execution_status(
        db=db, act_id=act_id, payload=payload
    )
    if not act:
        raise HTTPException(status_code=400, detail=error_msg)
    return {
        "status_code": 200,
        "success": True,
        "message": "Act updated successfully",
        "new_status": act.status,
    }


@router.post("/{act_id}/signers/{signer_id}/electronic-signature")
def upload_electronic_signature(
    act_id: str,
    signer_id: str,
    payload: ElectronicSignatureCreate,
    db: Session = Depends(get_db),
    current_user: dict = Depends(get_current_user),
):
    sig_record = crud_execution.save_electronic_signature(
        db=db, act_id=act_id, signer_id=signer_id, payload=payload
    )

    return {
        "status_code": 200,
        "success": True,
        "message": "Electronic signature saved successfully",
        "data": {"signature_id": sig_record.id, "status": sig_record.status},
    }


@router.post(
    "/{act_id}/signers/{signer_id}/wet-signature", summary="Upload file signature"
)
def upload_wet_signature(
    act_id: str,
    signer_id: str,
    file: UploadFile = File(...),
    db: Session = Depends(get_db),
    current_user: dict = Depends(get_current_user),
):
    file_path = f"https://storage.yourdomain.com/signatures/{act_id}_{signer_id}_{file.filename}"
    return {"status_code": 200, "success": True, "data": {"file_url": file_path}}
