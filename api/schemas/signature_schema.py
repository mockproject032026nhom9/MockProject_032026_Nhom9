from pydantic import BaseModel
from datetime import datetime
from typing import Optional

# 1. Hạ tầng CA & HSM
class CABase(BaseModel):
    name: str
    is_approved: int

class HSMBase(BaseModel):
    provider_name: str
    hsm_serial_number: str
    key_rotation_status: str

# 2. Chứng chỉ (Certificates)
class CertificateCreate(BaseModel):
    ca_id: int
    hsm_key_id: int
    device_id: int
    valid_from: datetime
    valid_to: datetime

class CertificateResponse(BaseModel):
    id: int
    serial_number: str
    algorithm: str
    status: str
    thumbprint: str
    valid_to: datetime
    class Config: from_attributes = True

# 3. Con dấu (Seals)
class SealResponse(BaseModel):
    id: int
    name: str
    type: str
    status: str
    image_url: str
    allowed_act_types: str
    class Config: from_attributes = True

# 4. Ký số (Digital Signature)
class SigningRequest(BaseModel):
    certificate_id: int
    device_id: int
    document_hash: str 

class DigitalSignatureResponse(BaseModel):
    id: int
    signed_at: datetime
    signature_value: str
    verification_status: str
    ip_address: str
    class Config: from_attributes = True

# 5. Chữ ký hình ảnh (Signature Record)
class ImageSignatureCreate(BaseModel):
    act_id: int
    order_index: int
    signature_data: Optional[str] = None 