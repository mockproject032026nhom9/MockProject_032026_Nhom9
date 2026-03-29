from fastapi import FastAPI
from app.db.database import engine
from app.models import act_model

# Lệnh này sẽ tự động chạy vào MS SQL và tạo bảng nếu chưa có
act_model.Base.metadata.create_all(bind=engine)

app = FastAPI(title="Execution & Certificate Service")

@app.get("/")
def root():
    return {"message": "Database Connected and Tables Created Successfully!"}