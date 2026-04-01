from typing import Literal
from fastapi import APIRouter, UploadFile, File, Form, status

from app.core.schemas import ApiResponse
from app.modules.media.services import media_service

router = APIRouter(prefix="/media", tags=["Media"])

@router.post("/upload", response_model=ApiResponse[dict])
async def upload_media(
    file: UploadFile = File(...),
    type: Literal["signature", "thumbprint"] = Form(...)
):
    """
    Upload a media file (Signature or Thumbprint).
    Stores the file on disk and returns the relative URL. 
    Accepts: PNG, JPG, JPEG.
    """
    url = media_service.save_file(file, type)
    
    return ApiResponse(
        status_code=status.HTTP_200_OK,
        data={"url": url}
    )
    
# Logic note: No database record is created HERE. 
# The URL should be passed to other endpoints (like Journal Entry update) 
# to link it to specific entities.
