from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from datetime import datetime

from core.database import get_db
from core.dependencies import get_current_user
from api.models.user_model import User
from api.models.job_model import Job
from api.models.assignment_model import Assignment
from api.models.status_log_model import JobStatusLog
from api.schemas.job_schema import JobCreate, JobResponse, AssignmentCreate, JobStatusUpdate
from api.schemas.response_schema import ResponseModel

router = APIRouter(prefix="/api/jobs", tags=["3. Job Dispatching"], dependencies=[Depends(get_current_user)])

@router.post("", response_model=ResponseModel)
def create_job(job_data: JobCreate, db: Session = Depends(get_db), current_user: User = Depends(get_current_user)):
    new_job = Job(
        client_id=current_user.id,
        service_type=job_data.service_type,
        location_type=job_data.location_type,
        location_details=job_data.location_details,
        requested_start_time=job_data.requested_start_time,
        requested_end_time=job_data.requested_end_time,
        signer_count=job_data.signer_count,
        status="Pending"
    )
    db.add(new_job)
    db.commit()
    db.refresh(new_job)
    
    return ResponseModel(success=True, status_code=201, message="Notarization request created successfully", data=JobResponse.model_validate(new_job).model_dump())

@router.get("", response_model=ResponseModel)
def get_jobs(skip: int = 0, limit: int = 10, db: Session = Depends(get_db)):
    jobs = db.query(Job).order_by(Job.id.desc()).offset(skip).limit(limit).all()
    total = db.query(Job).count()
    data = [JobResponse.model_validate(j).model_dump() for j in jobs]
    meta = {"page": (skip // limit) + 1, "limit": limit, "total_records": total}
    return ResponseModel(success=True, status_code=200, data=data, meta=meta)

@router.post("/{job_id}/assignments", response_model=ResponseModel)
def assign_notary(job_id: int, assign_data: AssignmentCreate, db: Session = Depends(get_db)):
    job = db.query(Job).filter(Job.id == job_id).first()
    if not job:
        return ResponseModel(success=False, status_code=404, message="Job not found")
        
    new_assignment = Assignment(job_id=job.id, notary_id=assign_data.notary_id)
    db.add(new_assignment)
    
    job.status = "Assigned"
    
    log = JobStatusLog(
        job_id=job.id,
        status="Assigned",
        note="System auto-log: Assigned to Notary"
    )
    db.add(log)
    
    db.commit()
    return ResponseModel(success=True, status_code=200, message="Job assigned successfully")

@router.patch("/{job_id}/status", response_model=ResponseModel)
def update_job_status(job_id: int, status_data: JobStatusUpdate, db: Session = Depends(get_db)):
    job = db.query(Job).filter(Job.id == job_id).first()
    if not job:
        return ResponseModel(success=False, status_code=404, message="Job not found")
        
    job.status = status_data.status
    
    log = JobStatusLog(
        job_id=job.id,
        status=status_data.status,
        delay=status_data.delay,
        exception_flags=status_data.exception_flags,
        note=status_data.note
    )
    db.add(log)
    
    db.commit()
    return ResponseModel(success=True, status_code=200, message=f"Status updated to: {status_data.status}")