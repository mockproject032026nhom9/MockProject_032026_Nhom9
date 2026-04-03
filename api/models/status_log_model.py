from sqlalchemy import Column, Integer, String, DateTime, ForeignKey, Text
from datetime import datetime
import uuid
from core.database import Base

class JobStatusLog(Base):
    __tablename__ = "job_status_logs" 

    job_status_id = Column(String(50), primary_key=True, autoincrement=True, default=lambda: f"jsl{uuid.uuid4().hex[:28]}")
    job_id = Column(Integer, ForeignKey("jobs.id"), nullable=False)
    status = Column(String(50), nullable=False)
    time_stamps = Column(DateTime, default=datetime.utcnow)
    delay = Column(String(50), nullable=True) # VD: "2h"
    exception_flags = Column(String(255), nullable=True)
    note = Column(Text, nullable=True)