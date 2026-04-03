import os
import uuid
from typing import Literal
from fastapi import UploadFile, HTTPException, status
from pathlib import Path

class MediaService:
    """Handles professional file storage and unique naming for biometric assets."""
    
    ALLOWED_EXTENSIONS = {".png", ".jpg", ".jpeg"}
    UPLOAD_ROOT = "uploads"
    
    def save_file(self, file: UploadFile, media_type: Literal["signature", "thumbprint"]) -> str:
        """
        Validate, rename, and save an uploaded file to the designated directory.
        Returns the relative URL path for the database.
        """
        # 1. Validate Extension
        extension = Path(file.filename).suffix.lower()
        if extension not in self.ALLOWED_EXTENSIONS:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_MESSAGE,
                detail=f"Invalid file type. Allowed: {', '.join(self.ALLOWED_EXTENSIONS)}"
            )

        # 2. Resolve Directory
        # Logic: signatures ➔ uploads/signatures, thumbprints ➔ uploads/thumbprints
        sub_dir = "signatures" if media_type == "signature" else "thumbprints"
        target_dir = Path(self.UPLOAD_ROOT) / sub_dir
        
        if not target_dir.exists():
            target_dir.mkdir(parents=True)

        # 3. Generate Unique UUID Filename
        unique_filename = f"{uuid.uuid4()}{extension}"
        file_path = target_dir / unique_filename

        # 4. Write Binary Data
        # Note: Using standard block writing for efficiency
        try:
            with open(file_path, "wb") as buffer:
                buffer.write(file.file.read())
        except Exception as e:
            raise HTTPException(
                status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
                detail=f"Failed to save file: {str(e)}"
            )
        finally:
            file.file.close()

        # 5. Return Relative URL for static serving
        return f"/{self.UPLOAD_ROOT}/{sub_dir}/{unique_filename}"

media_service = MediaService()
