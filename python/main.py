import uvicorn
from app.main import app

if __name__ == "__main__":
    # Start the application using uvicorn
    uvicorn.run("app.main:app", host="0.0.0.0", port=8000, reload=True)
