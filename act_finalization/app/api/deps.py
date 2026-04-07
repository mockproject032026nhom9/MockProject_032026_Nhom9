from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from app.core.session import SessionLocal

# Dòng này tạo ra cái nút "Authorize" trên Swagger UI
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/api/v1/dev-login")


def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


def get_current_user(token: str = Depends(oauth2_scheme)):
    if token != "token-dung-de-test":
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Token error",
            headers={"WWW-Authenticate": "Bearer"},
        )

    # Trả về thông tin Notary giả lập
    return {"user_id": 1, "role": "NOTARY", "email": "luan@gmail.com"}
