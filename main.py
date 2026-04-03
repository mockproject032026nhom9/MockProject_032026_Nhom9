from fastapi import FastAPI, Depends, Query, Response, HTTPException
from fastapi.encoders import jsonable_encoder
from sqlalchemy.orm import Session
from sqlalchemy import or_, cast, String
from typing import Optional
from pydantic import BaseModel
from datetime import datetime, timedelta
from enum import Enum
import database
import json

app = FastAPI()

class ActTypeEnum(str, Enum):
    REAL_ESTATE = "Real Estate Closing"
    WILLS_TRUSTS = "Wills & Trusts"
    POWER_OF_ATTORNEY = "Power of Attorney"
    AFFIDAVITS = "Affidavits"

class StatusEnum(str, Enum):
    COMPLETED = "Completed"
    INPROCESS = "Inprocess"
    VOIDED = "Voided"

class DateRangeEnum(str, Enum):
    TODAY = "Today"
    LAST_7_DAYS = "Last 7 Days"
    LAST_30_DAYS = "Last 30 Days"
    LAST_90_DAYS = "Last 90 Days"

class ActSchema(BaseModel):
    id: int
    act_type: str
    notary_name: str
    client_name: str
    date_time: datetime
    state: str
    status: str

    class Config:
        from_attributes = True

@app.on_event("startup")
def startup_event():
    database.seed_data()

def get_db():
    db = database.SessionLocal()
    try: yield db
    finally: db.close()

# --- SCREEN 1: ACT LIST ---
@app.get("/acts")
def get_acts(
    keyword: Optional[str] = Query(None),
    act_type: Optional[ActTypeEnum] = Query(None),
    status: Optional[StatusEnum] = Query(None),
    date_range: Optional[DateRangeEnum] = Query(None),
    page: int = Query(1, ge=1),
    db: Session = Depends(get_db)
):
    LIMIT = 4
    query = db.query(database.NotaryAct)

    if keyword:
        query = query.filter(or_(cast(database.NotaryAct.id, String).contains(keyword),
                                 database.NotaryAct.notary_name.contains(keyword),
                                 database.NotaryAct.client_name.contains(keyword)))
    
    if act_type: query = query.filter(database.NotaryAct.act_type == act_type.value)
    if status: query = query.filter(database.NotaryAct.status == status.value)

    if date_range:
        now = datetime.now()
        pass

    total_count = query.count()
    offset = (page - 1) * LIMIT
    results = query.order_by(database.NotaryAct.id.desc()).offset(offset).limit(LIMIT).all()
    
    final_response = {
        "status_code": 200,
        "success": True,
        "pagination": {"total_entries": total_count, "current_page": page},
        "data": jsonable_encoder([ActSchema.model_validate(i) for i in results])
    }
    return Response(content=json.dumps(final_response, indent=4, sort_keys=False, separators=(',', ': ')), media_type="application/json")

# --- SCREEN 2: OVERVIEW ---
@app.get("/acts/{act_id}")
def get_act_detail(act_id: int, db: Session = Depends(get_db)):
    act = db.query(database.NotaryAct).filter(database.NotaryAct.id == act_id).first()
    if not act:
        raise HTTPException(status_code=404, detail="Hồ sơ không tồn tại")

    detail_response = {
        "status_code": 200,
        "success": True,
        "data": {
            "id": act.id, # Đã bỏ Job_ID
            "status": act.status.upper(),
            "act_summary": {
                "act_type": act.act_type,
                "filing_date": act.date_time.strftime("%B %d, %Y"),
                "jurisdiction": act.jurisdiction,
                "assigned_notary": act.notary_name # Đã bỏ ID Notary
            },
            "location_snapshot": {
                "verified_ip_geolocation": "Los Angeles, CA"
            },
            "compliance_snapshot": [
                {"step": "Identity verified via KBA", "status": "PASSED", "time": "10:14 AM"},
                {"step": "Oath or Affirmation required", "status": "PENDING", "action": "COMPLETE NOW"},
                {"step": "Journal entry completed", "status": "PENDING"}
            ],
            "document_preview": {
                "content_summary": f"DRAFT CERTIFICATE for {act.act_type} - Client: {act.client_name}"
            }
        }
    }
    return Response(content=json.dumps(detail_response, indent=4, sort_keys=False, separators=(',', ': ')), media_type="application/json")

@app.get("/acts/{act_id}/view_draft")
def preview_draft_certificate(act_id: int, db: Session = Depends(get_db)):
    act = db.query(database.NotaryAct).filter(database.NotaryAct.id == act_id).first()
    if not act:
        raise HTTPException(status_code=404, detail="Notary Act not found")

    response_data = {
        "status_code": 200,
        "success": True,
        "document_info": {
            "title": "DRAFT CERTIFICATE",
            "client_name": act.client_name,
            "act_type": act.act_type,
            "date": act.date_time.strftime("%B %d, %Y"),
            "jurisdiction": act.state,
            "notary": act.notary_name
        }
    }
    
    json_str = json.dumps(response_data, indent=4, sort_keys=False, separators=(',', ': '))
    return Response(content=json_str, media_type="application/json")