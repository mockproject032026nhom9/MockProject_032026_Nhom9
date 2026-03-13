from fastapi import APIRouter, Response, status
from schemas.user_schema import User
from schemas.response_schema import ResponseModel # Import class xài chung đã chốt lúc trước

router = APIRouter(prefix="/api/users", tags=["Users"])

# Giả lập DB
users_db = []

@router.post("", status_code=status.HTTP_201_CREATED, response_model=ResponseModel)
def create_user(new_user: User):
    """
    Creates a new user and saves to the database.
    """
    users_db.append(new_user.model_dump())
    return ResponseModel(
        success=True,
        status_code=201,
        message="User created successfully",
        data=new_user.model_dump()
    )

@router.get("", response_model=ResponseModel)
def get_users():
    """
    Retrieves the list of all users.
    """
    return ResponseModel(
        success=True,
        status_code=200,
        message="Get user list successfully",
        data=users_db
    )

@router.put("/{user_id}", response_model=ResponseModel)
def update_user(user_id: int, updated_user: User, response: Response):
    """
    Updates an existing user by ID.
    """
    for i, user in enumerate(users_db):
        if user["id"] == user_id:
            users_db[i] = updated_user.model_dump()
            return ResponseModel(
                success=True,
                status_code=200,
                message="User updated successfully",
                data=updated_user.model_dump()
            )
            
    response.status_code = status.HTTP_404_NOT_FOUND
    return ResponseModel(
        success=False,
        status_code=404,
        message="User not found"
    )

@router.delete("/{user_id}", response_model=ResponseModel)
def delete_user(user_id: int, response: Response):
    """
    Deletes a user by ID.
    """
    for i, user in enumerate(users_db):
        if user["id"] == user_id:
            deleted_user = users_db.pop(i) 
            return ResponseModel(
                success=True,
                status_code=200,
                message="User deleted successfully",
                data=deleted_user
            )

    response.status_code = status.HTTP_404_NOT_FOUND
    return ResponseModel(
        success=False,
        status_code=404,
        message="User not found"
    )   