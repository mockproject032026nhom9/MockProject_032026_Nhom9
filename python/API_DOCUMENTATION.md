# API Documentation: Modular Notary Journal

This document provides technical details for the REST API endpoints implemented in the Notary Journal system. 

**Base URL**: `/api/v1`  
**Authentication**: Most endpoints require a `Bearer` token in the `Authorization` header.

---

## 1. Media & Biometrics

### Upload Media
Processes and stores signature or thumbprint images.
- **Endpoint**: `POST /media/upload`
- **Content-Type**: `multipart/form-data`
- **Request Body**:
  - `file`: (Binary) The image file.
  - `type`: (String) "signature" or "thumbprint".
- **Response**:
  ```json
  {
    "status_code": 200,
    "success": true,
    "data": { "url": "http://.../uploads/signatures/uuid.png" }
  }
  ```

---

## 2. Journal Management

### Get Journal Entry
Retrieves the full aggregate data for a master journal entry.
- **Endpoint**: `GET /journal-entries/{id}`
- **Response**: Aggregated JSON containing `signers`, `fees`, `compliance`, and `biometrics`.

### Update Journal Entry
Updates manual fields and biometric links.
- **Endpoint**: `PUT /journal-entries/{id}`
- **Request Body**:
  ```json
  {
    "fee_charged": 15.00,
    "location": "Springfield",
    "notary_notes": "...",
    "signature_url": "http://...",
    "thumbprint_url": "http://..."
  }
  ```

---

## 3. Status & Lifecycle (Acts)

### Get Act Status Overview
Retrieves the lifecycle timeline and legal hold flag.
- **Endpoint**: `GET /acts/{act_id}/status`
- **Response Data**:
  ```json
  {
    "act_id": "ACT001",
    "timeline": [
      { "status": "Draft", "timestamp": "...", "is_active": true }
    ],
    "is_legal_hold": false
  }
  ```

### Void Act
Cancels an act and records the reason.
- **Endpoint**: `POST /acts/{act_id}/void`
- **Request Body**:
  ```json
  {
    "reason_code": "DATA_ERROR",
    "additional_notes": "...",
    "approval_required": false
  }
  ```

### Update Legal Hold
Toggles the administrative "Legal Hold" flag.
- **Endpoint**: `PUT /acts/{act_id}/legal-hold`
- **Request Body**: `{ "is_legal_hold": true }`

---

## 4. Audit & History

### Get Audit Logs
Retrieves the permanent history of actions performed on an act.
- **Endpoint**: `GET /acts/{act_id}/audit-logs`
- **Query Params**: `page` (default 1), `size` (default 10)
- **Response Data**:
  ```json
  {
    "items": [
      { "timestamp": "...", "user": "Admin", "action": "VOIDED", "details": "..." }
    ],
    "total": 5
  }
  ```

---

## 5. References (Dropdowns)

### Get Void Reasons
Retrieves the standardized list for the "Void" reason dropdown.
- **Endpoint**: `GET /references/void-reasons`
- **Response Data**:
  ```json
  [
    { "code": "DATA_ERROR", "name": "Data Entry Error" },
    { "code": "CLIENT_REQUEST", "name": "Client Requested Cancel" }
  ]
  ```

---

## Technical Notes
- **Standard Format**: All responses follow the `ApiResponse` schema (`status_code`, `success`, `message`, `data`).
- **Pagination**: Audit logs use 1-based indexing for pages.
- **Validation**: Strict Pydantic v2 validation is applied to all request bodies.
