from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base

SERVER_NAME = "DESKTOP-16F0H1J"             
DATABASE_NAME = "MockProject_Nhom9"    
USERNAME = "sa"                        
PASSWORD = "123"          
DRIVER = "ODBC Driver 17 for SQL Server" 


SQLALCHEMY_DATABASE_URL = f"mssql+pyodbc://{USERNAME}:{PASSWORD}@{SERVER_NAME}/{DATABASE_NAME}?driver={DRIVER}"


engine = create_engine(SQLALCHEMY_DATABASE_URL)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base = declarative_base() 

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()