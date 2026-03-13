from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

users_db = []

class User(BaseModel):
    id: int
    name: str
    email: str

@app.post("/api/users", status_code=201)
def create_user(new_user: User):
    users_db.append(new_user.model_dump())

    return {
        "statusCode": 201,
        "success": True,
        "message": "User created",
        "data": new_user.model_dump()
    }

@app.get("/api/users")
def get_users():
    return {
        "statusCode": 200,
        "success": True,
        "message": "Get user list successfully",
        "data": users_db 
    }

@app.put("/api/users/{user_id}")
def update_user(user_id: int, updated_user: User):
    for i, user in enumerate(users_db):
        if user["id"] == user_id:
            users_db[i] = updated_user.model_dump()
            return {
                "statusCode": 200,
                "success": True,
                "message": "User updated successfully",
                "data": updated_user.model_dump()
            }
            
    return {
        "statusCode": 404,
        "success": False,
        "message": "User not found",
        "data": {}
    }

@app.delete("/api/users/{user_id}")
def delete_user(user_id: int):
    for i, user in enumerate(users_db):
        if user["id"] == user_id:
            deleted_user = users_db.pop(i) 
            return {
                "statusCode": 200,
                "success": True,
                "message": "User deleted successfully",
                "data": deleted_user
            }

    return {
        "statusCode": 404,
        "success": False,
        "message": "User not found",
        "data": {}
    }