from sqlalchemy.orm import sessionmaker
from app.core.database import engine

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)