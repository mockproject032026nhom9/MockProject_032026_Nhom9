# Database Schema Documentation: Notary Journal System

```mermaid
classDiagram
    direction LR

    class Role {
        +int id
        +string name
    }

    class User {
        +int id
        +string email
        +string password_hash
        +string full_name
        +int role_id
        +string status
    }

    class Notary {
        +int id
        +int user_id
        +string ssn
        +string full_name
        +string date_of_birth
        +string phone
        +string email
        +string residential_address
        +string employment_type
        +date start_date
        +string internal_notes
        +string status
    }

    class NotaryAct {
        +int id
        +int notary_id
        +string status
        +bool is_legal_hold
        +datetime created_at
        +datetime updated_at
    }

    class ActStatusHistory {
        +int id
        +int act_id
        +string status
        +datetime timestamp
        +bool is_active
    }

    class ActAuditLog {
        +int id
        +int act_id
        +int user_id
        +string user_name
        +string action
        +string details
        +datetime timestamp
    }

    class JournalEntry {
        +int id
        +int act_id
        +float notarial_fee
        +string act_type
        +string state
        +string document_title
        +int number_of_documents
        +string location
        +string notary_notes
        +datetime created_at
    }

    class JournalSigner {
        +int id
        +int journal_id
        +string full_name
        +string role
        +string id_type
        +string id_number
        +string id_authority
        +datetime id_expiry_date
        +string verification_method
        +string status
    }

    class JournalBiometric {
        +int id
        +int signer_id
        +string signature_image
        +string thumbprint_image
    }

    class JournalFee {
        +int id
        +int journal_id
        +float base_notarial_fee
        +float service_fee
        +float travel_fee
        +float convenience_fee
        +float rush_fee
        +float total_amount
        +float notary_share
        +float company_share
    }

    class JournalCompliance {
        +int id
        +int journal_id
        +bool identity_verification
        +bool mandatory_fields
        +bool final_notary_seal
    }

    class VoidReason {
        +string code
        +string name
    }

    Role "1" -- "*" User : includes
    User "1" -- "0..1" Notary : has_profile
    Notary "1" -- "*" NotaryAct : manages
    NotaryAct "1" -- "*" ActStatusHistory : timeline
    NotaryAct "1" -- "*" ActAuditLog : audit_trail
    NotaryAct "1" -- "*" JournalEntry : records
    User "1" -- "*" ActAuditLog : performs_action
    
    JournalEntry "1" -- "*" JournalSigner : signers
    JournalEntry "1" -- "1" JournalFee : fees
    JournalEntry "1" -- "1" JournalCompliance : compliance
    JournalSigner "1" -- "1" JournalBiometric : biometric
```

This document provides a comprehensive overview of the Microsoft SQL Server (MSSQL) database schema for the Modular Notary Journal API. The schema is designed with normalization and traceability as core principles.

## 0. Identity & Professional Profiles

| Table | Purpose | Key Relationships |
|-------|---------|-------------------|
| `users` | Identity management and RBAC. | N:1 with `roles`, 1:1 with `notaries`. |
| `notaries` | Professional capacity profile (SSN, Employment). | 1:1 with `users`, 1:N with `notary_acts`. |
| `act_status_history` | Detailed historical timeline of the session status changes. | N:1 with `NotaryAct`. |
| `act_audit_logs` | Permanent audit trail of *who* performed *what* on this session. | N:1 with `NotaryAct`, N:1 with `User`. |

---

## 2. Recording Layer (Journal Entries)

The "Journal" layer handles the specific documents and signers involved in an act.

| Table | Purpose | Key Relationships |
|-------|---------|-------------------|
| `journal_entries` | The actual journal record for a document within an act. | N:1 with `NotaryAct`, 1:N with `JournalSigner`. |
| `journal_signers` | Individuals (Grantors, Witnesses) signing the document. | N:1 with `JournalEntry`, 1:1 with `JournalBiometric`. |
| `journal_biometrics` | Signature and thumbprint image pointers for a signer. | 1:1 with `JournalSigner`. |

---

## 3. Financial & Compliance

| Table | Purpose | Key Relationships |
|-------|---------|-------------------|
| `journal_fees` | Breakdown of fees (service, notary share) for a specific record. | 1:1 with `JournalEntry`. |
| `journal_compliance` | Checklist confirming legal requirements were met. | 1:1 with `JournalEntry`. |

---

## Technical Specifications
- **Database Engine**: Microsoft SQL Server (MSSQL).
- **Naming Convention**: `snake_case` (Normalized for interoperability).
- **ORM**: SQLAlchemy 2.0 (Async).
- **Driver**: ODBC Driver 18 for SQL Server (`aioodbc`).
