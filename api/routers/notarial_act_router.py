from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from datetime import datetime
from core.database import get_db
from core.dependencies import get_current_user
from api.models.notarial_act_model import NotarialAct, ActSigner, JournalEntry
from api.schemas.notarial_act_schema import (
    SetupActRequest, AddSignerRequest, ExecuteActRequest,
    UpdateActRequest, UpdateSignerRequest,
    NotarialActResponse, ActSignerResponse, JournalEntryResponse
)
from api.schemas.response_schema import ResponseModel

# router = APIRouter(prefix="/api/notarial-acts", tags=["Notarial Acts Workflow"], dependencies=[Depends(get_current_user)])
router = APIRouter(prefix="/api/notarial-acts", tags=["Notarial Acts Workflow"])

# ==================== NOTARIAL ACTS CRUD ====================

@router.get("", response_model=ResponseModel)
def get_all_acts(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    """Get all notarial acts with pagination"""
    acts = db.query(NotarialAct).order_by(NotarialAct.id.desc()).offset(skip).limit(limit).all()
    total = db.query(NotarialAct).count()
    data = [NotarialActResponse.model_validate(act).model_dump() for act in acts]
    return ResponseModel(success=True, status_code=200, data=data, meta={"total": total})

@router.get("/{act_id}", response_model=ResponseModel)
def get_act(act_id: int, db: Session = Depends(get_db)):
    """Get a single notarial act by ID"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    return ResponseModel(success=True, status_code=200, data=NotarialActResponse.model_validate(act).model_dump())

@router.post("/setup", response_model=ResponseModel)
def setup_act(act_data: SetupActRequest, db: Session = Depends(get_db)):
    """Step 1: Setup Act (Select Jurisdiction and Act Type)"""
    new_act = NotarialAct(**act_data.model_dump())
    new_act.status = "IN_PROGRESS"
    db.add(new_act)
    db.commit()
    db.refresh(new_act)
    return ResponseModel(success=True, status_code=201, data=NotarialActResponse.model_validate(new_act).model_dump(), message="Act setup successfully")

@router.put("/{act_id}", response_model=ResponseModel)
def update_act(act_id: int, update_data: UpdateActRequest, db: Session = Depends(get_db)):
    """Update a notarial act (only if not LOCKED or VOIDED)"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    if act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Cannot modify a locked or voided act")
    
    update_fields = update_data.model_dump(exclude_unset=True)
    for key, value in update_fields.items():
        setattr(act, key, value)
    
    db.commit()
    db.refresh(act)
    return ResponseModel(success=True, status_code=200, data=NotarialActResponse.model_validate(act).model_dump(), message="Act updated successfully")

@router.delete("/{act_id}", response_model=ResponseModel)
def delete_act(act_id: int, db: Session = Depends(get_db)):
    """Delete a notarial act (only if not LOCKED or VOIDED)"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    if act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Cannot delete a locked or voided act")
    
    db.query(ActSigner).filter(ActSigner.act_id == act_id).delete()
    db.query(JournalEntry).filter(JournalEntry.act_id == act_id).delete()
    db.delete(act)
    db.commit()
    return ResponseModel(success=True, status_code=200, message="Act deleted successfully")

# ==================== SIGNERS CRUD ====================

@router.get("/{act_id}/signers", response_model=ResponseModel)
def get_signers(act_id: int, db: Session = Depends(get_db)):
    """Get all signers for a notarial act"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    
    signers = db.query(ActSigner).filter(ActSigner.act_id == act_id).all()
    data = [ActSignerResponse.model_validate(s).model_dump() for s in signers]
    return ResponseModel(success=True, status_code=200, data=data, meta={"total": len(signers)})

@router.get("/{act_id}/signers/{signer_id}", response_model=ResponseModel)
def get_signer(act_id: int, signer_id: int, db: Session = Depends(get_db)):
    """Get a single signer by ID"""
    signer = db.query(ActSigner).filter(ActSigner.id == signer_id, ActSigner.act_id == act_id).first()
    if not signer:
        return ResponseModel(success=False, status_code=404, message="Signer not found")
    return ResponseModel(success=True, status_code=200, data=ActSignerResponse.model_validate(signer).model_dump())

@router.post("/{act_id}/signers", response_model=ResponseModel)
def add_signer(act_id: int, signer_data: AddSignerRequest, db: Session = Depends(get_db)):
    """Step 2: Add a signer and verify ID"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act or act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Act not found or cannot be modified")
    
    signer = ActSigner(act_id=act_id, **signer_data.model_dump())
    signer.verification_status = "VERIFIED"  # Simulated ID logic validation
    db.add(signer)
    db.commit()
    db.refresh(signer)
    return ResponseModel(success=True, status_code=201, data=ActSignerResponse.model_validate(signer).model_dump(), message="Signer added securely")

@router.put("/{act_id}/signers/{signer_id}", response_model=ResponseModel)
def update_signer(act_id: int, signer_id: int, update_data: UpdateSignerRequest, db: Session = Depends(get_db)):
    """Update a signer's information"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    if act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Cannot modify signers of a locked or voided act")
    
    signer = db.query(ActSigner).filter(ActSigner.id == signer_id, ActSigner.act_id == act_id).first()
    if not signer:
        return ResponseModel(success=False, status_code=404, message="Signer not found")
    
    update_fields = update_data.model_dump(exclude_unset=True)
    for key, value in update_fields.items():
        setattr(signer, key, value)
    
    db.commit()
    db.refresh(signer)
    return ResponseModel(success=True, status_code=200, data=ActSignerResponse.model_validate(signer).model_dump(), message="Signer updated successfully")

@router.delete("/{act_id}/signers/{signer_id}", response_model=ResponseModel)
def delete_signer(act_id: int, signer_id: int, db: Session = Depends(get_db)):
    """Delete a signer from a notarial act"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
    if act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Cannot remove signers from a locked or voided act")
    
    signer = db.query(ActSigner).filter(ActSigner.id == signer_id, ActSigner.act_id == act_id).first()
    if not signer:
        return ResponseModel(success=False, status_code=404, message="Signer not found")
    
    db.delete(signer)
    db.commit()
    return ResponseModel(success=True, status_code=200, message="Signer removed successfully")

# ==================== WORKFLOW STEPS (Execute, Lock, Void) ====================

@router.post("/{act_id}/execute", response_model=ResponseModel)
def execute_act(act_id: int, execution_data: ExecuteActRequest, db: Session = Depends(get_db)):
    """Step 3: Execute Act (Administer Oath, Appearance, Journal Entry)"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act or act.status in ["LOCKED", "VOIDED"]:
        return ResponseModel(success=False, status_code=400, message="Act not found or cannot be modified")
    
    if act.type.upper() == "JURAT" and not execution_data.oath_administered:
        return ResponseModel(success=False, status_code=422, message="Compliance Failed: Oath administration is mandatory for JURAT acts")

    journal_entry = db.query(JournalEntry).filter(JournalEntry.act_id == act_id).first()
    if journal_entry:
        for key, value in execution_data.model_dump().items():
            setattr(journal_entry, key, value)
    else:
        journal_entry = JournalEntry(act_id=act_id, **execution_data.model_dump())
        db.add(journal_entry)
        
    db.commit()
    db.refresh(journal_entry)
    return ResponseModel(success=True, status_code=200, data=JournalEntryResponse.model_validate(journal_entry).model_dump(), message="Act execution details saved")

@router.post("/{act_id}/lock", response_model=ResponseModel)
def finalize_and_lock(act_id: int, db: Session = Depends(get_db)):
    """Step 4: Finalize and lock the document (Compliance Check)"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
        
    signers = db.query(ActSigner).filter(ActSigner.act_id == act_id).all()
    if not signers:
        return ResponseModel(success=False, status_code=422, message="Compliance Failed: No signers attached")
        
    journal = db.query(JournalEntry).filter(JournalEntry.act_id == act_id).first()
    if not journal or not journal.personal_appearance_verified:
        return ResponseModel(success=False, status_code=422, message="Compliance Failed: Personal appearance not verified")
        
    act.status = "LOCKED"
    act.locked_at = datetime.utcnow()
    db.commit()
    db.refresh(act)
    
    return ResponseModel(success=True, status_code=200, data=NotarialActResponse.model_validate(act).model_dump(), message="Act finalized and digitally locked")

@router.post("/{act_id}/void", response_model=ResponseModel)
def void_act(act_id: int, reason: str, db: Session = Depends(get_db)):
    """Step 5: Physical Void of the act"""
    act = db.query(NotarialAct).filter(NotarialAct.id == act_id).first()
    if not act:
        return ResponseModel(success=False, status_code=404, message="Act not found")
        
    act.status = "VOIDED"
    act.void_reason = reason
    db.commit()
    return ResponseModel(success=True, status_code=200, message=f"Act logically voided: {reason}")
