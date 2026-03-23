from sqlalchemy import Column, Integer, DateTime, ForeignKey
from datetime import datetime
from core.database import Base

class Assignment(Base):
    __tablename__ = "job_assignments"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    job_id = Column(Integer, ForeignKey("jobs.id"), nullable=False)
    notary_id = Column(Integer, nullable=False, index=True)
    assigned_at = Column(DateTime, default=datetime.utcnow)
    accepted_at = Column(DateTime, nullable=True) 