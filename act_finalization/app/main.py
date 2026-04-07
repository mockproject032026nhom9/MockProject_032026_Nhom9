from fastapi.security import OAuth2PasswordRequestForm
from fastapi.middleware.cors import CORSMiddleware
from app.models import act_model
from fastapi import FastAPI, Depends
from app.core.database import engine
from app.api.v1.api import api_router


# Tự động tạo bảng
act_model.Base.metadata.create_all(bind=engine)

app = FastAPI(title="Execution & Certificate Service")

# List API
app.include_router(api_router, prefix="/api/v1")

origins = ["*"]
app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"], 
    allow_headers=["*"],
)


# API giả lập để Swagger UI lấy Token
@app.post("/api/v1/dev-login", tags=["Dev Tools"])
def fake_login(form_data: OAuth2PasswordRequestForm = Depends()):
    return {"access_token": "token-dung-de-test", "token_type": "bearer"}


@app.get("/")
def root():
    return {"message": "Connected and Ready!"}
