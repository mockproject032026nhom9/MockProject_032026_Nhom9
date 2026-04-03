import random
from datetime import datetime, timedelta
from sqlalchemy import create_engine, Column, Integer, String, DateTime
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

URL_DATABASE = "sqlite:///./notary.db"
engine = create_engine(URL_DATABASE, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

class NotaryAct(Base):
    __tablename__ = "notary_acts"
    id = Column(Integer, primary_key=True, index=True)
    act_type = Column(String)
    notary_name = Column(String) 
    client_name = Column(String)
    date_time = Column(DateTime)
    state = Column(String)
    status = Column(String)
    jurisdiction = Column(String)

Base.metadata.create_all(bind=engine)

def seed_data():
    db = SessionLocal()
    if db.query(NotaryAct).count() == 0:
        act_types = ["Real Estate Closing", "Wills & Trusts", "Power of Attorney", "Affidavits"]
        notaries = ["Sarah Jenkins", "David Chen", "Michael Brown", "Nick", "Emily White"]
        clients = ["Robert Miller", "Jotaro", "Alice Thompson", "Kevin Brown", "Lisa Ray"]
        
        now = datetime.now()
        random_dates = []
        for _ in range(100):
            random_dates.append((now - timedelta(days=random.randint(0, 100))).replace(microsecond=0))
        
        random_dates.sort()
        
        for i in range(1, 101):
            new_act = NotaryAct(
                id=i,
                act_type=random.choice(act_types),
                notary_name=random.choice(notaries),
                client_name=random.choice(clients),
                date_time=random_dates[i-1],
                state="California",
                status=random.choice(["Completed", "Inprocess", "Voided"]),
                jurisdiction="Los Angeles County, CA"
            )
            db.add(new_act)
        db.commit()
        print("--- Đã tạo thành công 100 dữ liệu mẫu (Sạch Job_ID & Notary ID) ---")
    db.close()