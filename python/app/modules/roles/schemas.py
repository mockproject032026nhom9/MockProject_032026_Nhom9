from pydantic import BaseModel, ConfigDict

class RoleBase(BaseModel):
    name: str

class RoleCreate(RoleBase):
    id: int

class RoleOut(RoleBase):
    id: int
    
    model_config = ConfigDict(from_attributes=True)
