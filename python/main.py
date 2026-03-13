from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

# Simple User model
class User(BaseModel):
    id: int
    name: str
    email: str

# Mock database
users = [
    {"id": 1, "name": "Admin", "email": "admin@gmail.com"}
]

@app.get("/users")
def get_users():
    return {
        "statusCode": 200,
        "success": True,
        "message": "Users retrieved successfully",
        "data": users
    }

@app.get("/users/{id}")
def get_user(id: int):
    for u in users:
        if u["id"] == id:
            return {
                "statusCode": 200,
                "success": True, 
                "message": "User found", 
                "data": u
            }
    return {
        "statusCode": 404,
        "success": False, 
        "message": "User not found", 
        "data": None
    }

@app.post("/users")
def create_user(user: User):
    users.append(user.model_dump())
    return {
        "statusCode": 201,
        "success": True, 
        "message": "User created successfully", 
        "data": user
    }

@app.put("/users/{id}")
def update_user(id: int, user: User):
    for i, u in enumerate(users):
        if u["id"] == id:
            users[i] = user.model_dump()
            return {
                "statusCode": 200,
                "success": True, 
                "message": "User updated successfully", 
                "data": user
            }
    return {
        "statusCode": 404,
        "success": False, 
        "message": "User not found for update", 
        "data": None
    }

@app.delete("/users/{id}")
def delete_user(id: int):
    for i, u in enumerate(users):
        if u["id"] == id:
            users.pop(i)
            return {
                "statusCode": 200,
                "success": True, 
                "message": "User deleted successfully", 
                "data": None
            }
    return {
        "statusCode": 404,
        "success": False, 
        "message": "User not found for deletion", 
        "data": None
    }
