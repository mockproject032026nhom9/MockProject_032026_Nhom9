from sqlalchemy import Column, Integer, String, DateTime, ForeignKey, Text, Boolean
from core.database import Base

class CertificateAuthority(Base): # mapping: certificate_authorities.csv
    __tablename__ = "certificate_authorities"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    name = Column(String(255))
    is_approved = Column(Integer) # [cite: 4]

class HSMKeyStorage(Base): # mapping: hsm_key_storages.csv
    __tablename__ = "hsm_key_storages"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    provider_name = Column(String(100))
    hsm_serial_number = Column(String(100))
    key_rotation_status = Column(String(50))
    last_rotation_at = Column(DateTime)
    next_rotation_due = Column(DateTime) # [cite: 5]


class Certificate(Base): # mapping: certificates.csv
    __tablename__ = "certificates"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    owner_user_id = Column(Integer, index=True)
    ca_id = Column(Integer, ForeignKey("certificate_authorities.id"))
    hsm_key_id = Column(Integer, ForeignKey("hsm_key_storages.id"))
    serial_number = Column(String(100))
    public_key = Column(Text)
    thumbprint = Column(String(255))
    algorithm = Column(String(50))
    valid_from = Column(DateTime)
    valid_to = Column(DateTime)
    status = Column(String(50))
    replace_cert_id = Column(Integer, nullable=True)
    device_id = Column(Integer, ForeignKey("devices.id")) # [cite: 1]

class Seal(Base): # mapping: seals.csv
    __tablename__ = "seals"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    commission_id = Column(Integer)
    type = Column(String(50))
    name = Column(String(100))
    status = Column(String(50))
    image_url = Column(String(255))
    issued_at = Column(DateTime)
    allowed_act_types = Column(String(255))
    replace_seal_id = Column(Integer, nullable=True)
    notarial_act_id = Column(Integer) # [cite: 2]

class DigitalSignature(Base): # mapping: digital_signatures.csv
    __tablename__ = "digital_signatures"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    user_id = Column(Integer)
    certificate_id = Column(Integer, ForeignKey("certificates.id"))
    device_id = Column(Integer, ForeignKey("devices.id"))
    document_hash = Column(Text)
    signature_value = Column(Text)
    signed_at = Column(DateTime)
    ip_address = Column(String(50))
    verification_status = Column(String(50)) # [cite: 3]

class SignatureRecord(Base): # mapping: signature.csv (store handwritten/image signature)
    __tablename__ = "signatures"
    id = Column(Integer, primary_key=True, autoincrement=True, index=True)
    act_id = Column(Integer)
    user_id = Column(Integer)
    order_index = Column(Integer)
    signature_data = Column(Text, nullable=True) # Store base64 image
    status = Column(String(50)) # [cite: 6]