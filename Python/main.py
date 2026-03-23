from fastapi import FastAPI
from pydantic import BaseModel
from datetime import datetime
from typing import Optional

app = FastAPI()

class AuditLog(BaseModel):
    user_id: str
    action: str
    details: Optional[dict] = None

db = []

@app.get("/")
def home():
    return {"status": "Audit Module is running"}

@app.post("/logs")
def create_log(item: AuditLog):
    new_entry = {
        "id": len(db) + 1,
        "timestamp": datetime.now(),
        **item.dict()
    }
    db.append(new_entry)
    return new_entry

@app.get("/logs")
def get_logs():
    return db