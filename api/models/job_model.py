from sqlalchemy import Column, Integer, String, DateTime
from core.database import Base

class Job(Base):
    __tablename__ = "jobs"

    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    client_id = Column(Integer, nullable=False, index=True) 
    service_type = Column(String(100)) 
    location_type = Column(String(50)) 
    location_details = Column(String(255))
    requested_start_time = Column(DateTime)
    requested_end_time = Column(DateTime)
    signer_count = Column(Integer, default=1)
    status = Column(String(50), default="Pending")