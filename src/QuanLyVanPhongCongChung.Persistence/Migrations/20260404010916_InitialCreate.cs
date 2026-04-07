using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyVanPhongCongChung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "certificate_authorities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    is_approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_authorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "hsm_key_storages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    provider_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    hsm_serial_number = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    key_rotation_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_rotation_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    next_rotation_due = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hsm_key_storages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lang_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    lang_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    tax_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    state_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_requests_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    id_role = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_id_role",
                        column: x => x.id_role,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "deliveries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveries", x => x.id);
                    table.ForeignKey(
                        name: "FK_deliveries_service_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_documents_service_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gateway = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    transaction = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_service_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "verifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    result = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    method = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_verifications_service_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    action = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    entity_type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    metadata = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    device_identifier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mfa_enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_devices_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "export_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    requested_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    export_scope = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_export_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_export_histories_users_requested_by",
                        column: x => x.requested_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    client_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    service_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    location_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    location_details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    requested_start_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    requested_end_time = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    signer_count = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_jobs_users_client_id",
                        column: x => x.client_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notaries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ssn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    photo_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    employment_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    internal_notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    residential_address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notaries", x => x.id);
                    table.ForeignKey(
                        name: "FK_notaries_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    owner_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ca_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    hsm_key_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    serial_number = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    public_key = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    thumbprint = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    algorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    valid_from = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    valid_to = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    replace_cert_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificates", x => x.id);
                    table.ForeignKey(
                        name: "FK_certificates_certificate_authorities_ca_id",
                        column: x => x.ca_id,
                        principalTable: "certificate_authorities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_certificates_certificates_replace_cert_id",
                        column: x => x.replace_cert_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_certificates_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_certificates_hsm_key_storages_hsm_key_id",
                        column: x => x.hsm_key_id,
                        principalTable: "hsm_key_storages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_certificates_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "job_status_logs",
                columns: table => new
                {
                    job_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    job_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    time_stamps = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    delay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    exception_flags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_status_logs", x => x.job_status_id);
                    table.ForeignKey(
                        name: "FK_job_status_logs_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    job_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sms = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<bool>(type: "bit", nullable: false),
                    app = table.Column<bool>(type: "bit", nullable: false),
                    delay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notifications_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    job_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    assigned_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_assignments_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_assignments_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "journal_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notarial_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_journal_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_journal_entries_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notarial_acts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    jurisdiction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notarial_acts", x => x.id);
                    table.ForeignKey(
                        name: "FK_notarial_acts_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notarial_acts_service_requests_request_id",
                        column: x => x.request_id,
                        principalTable: "service_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notary_audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    table_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    record_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    old_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    new_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    changed_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_audit_logs_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_availabilities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    working_days_per_week = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    fixed_day_off = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_availabilities_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_bonds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    provider_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    bond_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_bonds", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_bonds_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_capabilities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    mobile = table.Column<bool>(type: "bit", nullable: false),
                    ron = table.Column<bool>(type: "bit", nullable: false),
                    loan_signing = table.Column<bool>(type: "bit", nullable: false),
                    apostille_related_support = table.Column<bool>(type: "bit", nullable: false),
                    max_distance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_capabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_capabilities_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_commissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_state_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_renewal_applied = table.Column<bool>(type: "bit", nullable: false),
                    expected_renewal_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_commissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_commissions_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notary_commissions_states_commission_state_id",
                        column: x => x.commission_state_id,
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notary_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    doc_category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    upload_date = table.Column<DateOnly>(type: "date", nullable: false),
                    verified_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    version = table.Column<int>(type: "int", nullable: false),
                    is_current_version = table.Column<bool>(type: "bit", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_documents_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_incidents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    incident_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_incidents", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_incidents_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_insurances",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    policy_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    provider_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    coverage_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_insurances", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_insurances_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notary_service_areas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    county_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_service_areas", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_service_areas_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notary_service_areas_states_state_id",
                        column: x => x.state_id,
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notary_status_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notary_status_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_notary_status_history_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "digital_signatures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    signature_value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    document_hash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    signed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    verification_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_digital_signatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_digital_signatures_certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_digital_signatures_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_digital_signatures_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fee_breakdowns",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    journal_entry_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    base_notarial_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    service_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    travel_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    convenience_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    rush_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    notary_share = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    company_share = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fee_breakdowns", x => x.id);
                    table.ForeignKey(
                        name: "FK_fee_breakdowns_journal_entries_journal_entry_id",
                        column: x => x.journal_entry_id,
                        principalTable: "journal_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "signers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    journal_entry_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_signers", x => x.id);
                    table.ForeignKey(
                        name: "FK_signers_journal_entries_journal_entry_id",
                        column: x => x.journal_entry_id,
                        principalTable: "journal_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "act_log_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_act_log_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_act_log_entries_notarial_acts_act_id",
                        column: x => x.act_id,
                        principalTable: "notarial_acts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_act_log_entries_notaries_notary_id",
                        column: x => x.notary_id,
                        principalTable: "notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "act_signatures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_index = table.Column<int>(type: "int", nullable: false),
                    signature_data = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_act_signatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_act_signatures_notarial_acts_act_id",
                        column: x => x.act_id,
                        principalTable: "notarial_acts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_act_signatures_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "compliance_reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    result = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compliance_reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_compliance_reviews_notarial_acts_act_id",
                        column: x => x.act_id,
                        principalTable: "notarial_acts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ron_technologies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    capability_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ron_camera_ready = table.Column<bool>(type: "bit", nullable: false),
                    ron_internet_ready = table.Column<bool>(type: "bit", nullable: false),
                    digital_status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ron_technologies", x => x.id);
                    table.ForeignKey(
                        name: "FK_ron_technologies_notary_capabilities_capability_id",
                        column: x => x.capability_id,
                        principalTable: "notary_capabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "authority_scopes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    authority_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authority_scopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_authority_scopes_notary_commissions_commission_id",
                        column: x => x.commission_id,
                        principalTable: "notary_commissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    issued_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    allowed_act_types = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    notarial_act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    replace_seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seals", x => x.id);
                    table.ForeignKey(
                        name: "FK_seals_notary_commissions_commission_id",
                        column: x => x.commission_id,
                        principalTable: "notary_commissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_seals_seals_replace_seal_id",
                        column: x => x.replace_seal_id,
                        principalTable: "seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "biometric_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    signer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    signature_image = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biometric_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_biometric_data_signers_signer_id",
                        column: x => x.signer_id,
                        principalTable: "signers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "revocations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    performed_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    incident_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revocations", x => x.id);
                    table.ForeignKey(
                        name: "FK_revocations_certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_revocations_notary_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "notary_incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_revocations_seals_seal_id",
                        column: x => x.seal_id,
                        principalTable: "seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_revocations_users_performed_by",
                        column: x => x.performed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "seal_usage_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    used_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    page_number = table.Column<int>(type: "int", nullable: true),
                    is_anomaly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seal_usage_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_seal_usage_logs_seals_seal_id",
                        column: x => x.seal_id,
                        principalTable: "seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seal_usage_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "replacements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    old_seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    old_certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    new_seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    new_certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    replaced_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    performed_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    revocation_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replacements", x => x.id);
                    table.ForeignKey(
                        name: "FK_replacements_certificates_new_certificate_id",
                        column: x => x.new_certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_replacements_certificates_old_certificate_id",
                        column: x => x.old_certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_replacements_revocations_revocation_id",
                        column: x => x.revocation_id,
                        principalTable: "revocations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_replacements_seals_new_seal_id",
                        column: x => x.new_seal_id,
                        principalTable: "seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_replacements_seals_old_seal_id",
                        column: x => x.old_seal_id,
                        principalTable: "seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_replacements_users_performed_by",
                        column: x => x.performed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_act_log_entries_act_id",
                table: "act_log_entries",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_act_log_entries_notary_id",
                table: "act_log_entries",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_act_signatures_act_id",
                table: "act_signatures",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_act_signatures_user_id",
                table: "act_signatures",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_entity_type_entity_id",
                table: "audit_logs",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_timestamp",
                table: "audit_logs",
                column: "timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_user_id",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_authority_scopes_commission_id",
                table: "authority_scopes",
                column: "commission_id");

            migrationBuilder.CreateIndex(
                name: "IX_biometric_data_signer_id",
                table: "biometric_data",
                column: "signer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_certificates_ca_id",
                table: "certificates",
                column: "ca_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificates_device_id",
                table: "certificates",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificates_hsm_key_id",
                table: "certificates",
                column: "hsm_key_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificates_owner_user_id",
                table: "certificates",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificates_replace_cert_id",
                table: "certificates",
                column: "replace_cert_id");

            migrationBuilder.CreateIndex(
                name: "IX_certificates_status",
                table: "certificates",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_compliance_reviews_act_id",
                table: "compliance_reviews",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_request_id",
                table: "deliveries",
                column: "request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_devices_user_id",
                table: "devices",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_digital_signatures_certificate_id",
                table: "digital_signatures",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_digital_signatures_device_id",
                table: "digital_signatures",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_digital_signatures_user_id",
                table: "digital_signatures",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_documents_request_id",
                table: "documents",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_export_histories_requested_by",
                table: "export_histories",
                column: "requested_by");

            migrationBuilder.CreateIndex(
                name: "IX_fee_breakdowns_journal_entry_id",
                table: "fee_breakdowns",
                column: "journal_entry_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_assignments_job_id",
                table: "job_assignments",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_assignments_notary_id_status",
                table: "job_assignments",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_job_status_logs_job_id",
                table: "job_status_logs",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_client_id",
                table: "jobs",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_status",
                table: "jobs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_notary_id",
                table: "journal_entries",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_journal_entries_status",
                table: "journal_entries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_languages_lang_code",
                table: "languages",
                column: "lang_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notarial_acts_jurisdiction_id",
                table: "notarial_acts",
                column: "jurisdiction_id");

            migrationBuilder.CreateIndex(
                name: "IX_notarial_acts_notary_id",
                table: "notarial_acts",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_notarial_acts_request_id",
                table: "notarial_acts",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_notarial_acts_status",
                table: "notarial_acts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_notaries_email",
                table: "notaries",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_notaries_ssn",
                table: "notaries",
                column: "ssn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notaries_status",
                table: "notaries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_notaries_user_id",
                table: "notaries",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notary_audit_logs_notary_id_created_at",
                table: "notary_audit_logs",
                columns: new[] { "notary_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notary_availabilities_notary_id",
                table: "notary_availabilities",
                column: "notary_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notary_bonds_notary_id",
                table: "notary_bonds",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_notary_capabilities_notary_id",
                table: "notary_capabilities",
                column: "notary_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notary_commissions_commission_state_id",
                table: "notary_commissions",
                column: "commission_state_id");

            migrationBuilder.CreateIndex(
                name: "IX_notary_commissions_expiration_date",
                table: "notary_commissions",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "IX_notary_commissions_notary_id_status",
                table: "notary_commissions",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_notary_documents_notary_id_doc_category",
                table: "notary_documents",
                columns: new[] { "notary_id", "doc_category" });

            migrationBuilder.CreateIndex(
                name: "IX_notary_incidents_notary_id_status",
                table: "notary_incidents",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_notary_insurances_notary_id",
                table: "notary_insurances",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_notary_service_areas_notary_id_state_id",
                table: "notary_service_areas",
                columns: new[] { "notary_id", "state_id" });

            migrationBuilder.CreateIndex(
                name: "IX_notary_service_areas_state_id",
                table: "notary_service_areas",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_notary_status_history_notary_id",
                table: "notary_status_history",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_event_id",
                table: "notifications",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_job_id",
                table: "notifications",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_request_id",
                table: "payments",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_new_certificate_id",
                table: "replacements",
                column: "new_certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_new_seal_id",
                table: "replacements",
                column: "new_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_old_certificate_id",
                table: "replacements",
                column: "old_certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_old_seal_id",
                table: "replacements",
                column: "old_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_performed_by",
                table: "replacements",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "IX_replacements_revocation_id",
                table: "replacements",
                column: "revocation_id");

            migrationBuilder.CreateIndex(
                name: "IX_revocations_certificate_id",
                table: "revocations",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_revocations_incident_id",
                table: "revocations",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_revocations_performed_by",
                table: "revocations",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "IX_revocations_seal_id",
                table: "revocations",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ron_technologies_capability_id",
                table: "ron_technologies",
                column: "capability_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seal_usage_logs_seal_id",
                table: "seal_usage_logs",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_seal_usage_logs_user_id",
                table: "seal_usage_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_seals_commission_id",
                table: "seals",
                column: "commission_id");

            migrationBuilder.CreateIndex(
                name: "IX_seals_notarial_act_id",
                table: "seals",
                column: "notarial_act_id");

            migrationBuilder.CreateIndex(
                name: "IX_seals_replace_seal_id",
                table: "seals",
                column: "replace_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_seals_status",
                table: "seals",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_service_requests_organization_id",
                table: "service_requests",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_signers_journal_entry_id",
                table: "signers",
                column: "journal_entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_states_state_code",
                table: "states",
                column: "state_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id_role",
                table: "users",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "IX_users_status",
                table: "users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_verifications_request_id",
                table: "verifications",
                column: "request_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "act_log_entries");

            migrationBuilder.DropTable(
                name: "act_signatures");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "authority_scopes");

            migrationBuilder.DropTable(
                name: "biometric_data");

            migrationBuilder.DropTable(
                name: "compliance_reviews");

            migrationBuilder.DropTable(
                name: "deliveries");

            migrationBuilder.DropTable(
                name: "digital_signatures");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "export_histories");

            migrationBuilder.DropTable(
                name: "fee_breakdowns");

            migrationBuilder.DropTable(
                name: "job_assignments");

            migrationBuilder.DropTable(
                name: "job_status_logs");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "notary_audit_logs");

            migrationBuilder.DropTable(
                name: "notary_availabilities");

            migrationBuilder.DropTable(
                name: "notary_bonds");

            migrationBuilder.DropTable(
                name: "notary_documents");

            migrationBuilder.DropTable(
                name: "notary_insurances");

            migrationBuilder.DropTable(
                name: "notary_service_areas");

            migrationBuilder.DropTable(
                name: "notary_status_history");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "replacements");

            migrationBuilder.DropTable(
                name: "ron_technologies");

            migrationBuilder.DropTable(
                name: "seal_usage_logs");

            migrationBuilder.DropTable(
                name: "verifications");

            migrationBuilder.DropTable(
                name: "signers");

            migrationBuilder.DropTable(
                name: "notarial_acts");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "revocations");

            migrationBuilder.DropTable(
                name: "notary_capabilities");

            migrationBuilder.DropTable(
                name: "journal_entries");

            migrationBuilder.DropTable(
                name: "service_requests");

            migrationBuilder.DropTable(
                name: "certificates");

            migrationBuilder.DropTable(
                name: "notary_incidents");

            migrationBuilder.DropTable(
                name: "seals");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "certificate_authorities");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "hsm_key_storages");

            migrationBuilder.DropTable(
                name: "notary_commissions");

            migrationBuilder.DropTable(
                name: "notaries");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
