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
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "CertificateAuthorities",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    is_approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateAuthorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "dbo",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "HsmKeyStorages",
                schema: "dbo",
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
                    table.PrimaryKey("PK_HsmKeyStorages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lang_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    lang_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Organizations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    state_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_id_role",
                        column: x => x.id_role,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_AuditLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_Devices_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExportHistories",
                schema: "dbo",
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
                    table.PrimaryKey("PK_ExportHistories", x => x.id);
                    table.ForeignKey(
                        name: "FK_ExportHistories_Users_requested_by",
                        column: x => x.requested_by,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Jobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_Jobs_Users_client_id",
                        column: x => x.client_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notaries",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Notaries", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notaries_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    organization_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_modified_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Organizations_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "dbo",
                        principalTable: "Organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Users_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Certificates", x => x.id);
                    table.ForeignKey(
                        name: "FK_Certificates_CertificateAuthorities_ca_id",
                        column: x => x.ca_id,
                        principalSchema: "dbo",
                        principalTable: "CertificateAuthorities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_Certificates_replace_cert_id",
                        column: x => x.replace_cert_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Certificates_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "dbo",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Certificates_HsmKeyStorages_hsm_key_id",
                        column: x => x.hsm_key_id,
                        principalSchema: "dbo",
                        principalTable: "HsmKeyStorages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Certificates_Users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobStatusLogs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_JobStatusLogs", x => x.job_status_id);
                    table.ForeignKey(
                        name: "FK_JobStatusLogs_Jobs_job_id",
                        column: x => x.job_id,
                        principalSchema: "dbo",
                        principalTable: "Jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "dbo",
                columns: table => new
                {
                    notification_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    event_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    job_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sms = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<bool>(type: "bit", nullable: false),
                    app = table.Column<bool>(type: "bit", nullable: false),
                    delay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    time_stamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_Notifications_Events_event_id",
                        column: x => x.event_id,
                        principalSchema: "dbo",
                        principalTable: "Events",
                        principalColumn: "event_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Jobs_job_id",
                        column: x => x.job_id,
                        principalSchema: "dbo",
                        principalTable: "Jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobAssignments",
                schema: "dbo",
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
                    table.PrimaryKey("PK_JobAssignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_JobAssignments_Jobs_job_id",
                        column: x => x.job_id,
                        principalSchema: "dbo",
                        principalTable: "Jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAssignments_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "dbo",
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
                    table.PrimaryKey("PK_JournalEntries", x => x.id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotaryAuditLogs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryAuditLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryAuditLogs_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryAvailabilities",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryAvailabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryAvailabilities_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryBonds",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryBonds", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryBonds_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryCapabilities",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryCapabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryCapabilities_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryCommissions",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_state = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    commission_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    issue_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_renewal_applied = table.Column<bool>(type: "bit", nullable: false),
                    expected_renewal_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaryCommissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryCommissions_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryDocuments",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryDocuments", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryDocuments_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryIncidents",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryIncidents", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryIncidents_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryInsurances",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    policy_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    provider_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    coverage_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    effective_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaryInsurances", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryInsurances_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotaryServiceAreas",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    county_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaryServiceAreas", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryServiceAreas_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotaryServiceAreas_States_state_id",
                        column: x => x.state_id,
                        principalSchema: "dbo",
                        principalTable: "States",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotaryStatusHistory",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotaryStatusHistory", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotaryStatusHistory_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.id);
                    table.ForeignKey(
                        name: "FK_Deliveries_ServiceRequests_request_id",
                        column: x => x.request_id,
                        principalSchema: "dbo",
                        principalTable: "ServiceRequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_Documents_ServiceRequests_request_id",
                        column: x => x.request_id,
                        principalSchema: "dbo",
                        principalTable: "ServiceRequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotarialActs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_NotarialActs", x => x.id);
                    table.ForeignKey(
                        name: "FK_NotarialActs_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotarialActs_ServiceRequests_request_id",
                        column: x => x.request_id,
                        principalSchema: "dbo",
                        principalTable: "ServiceRequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Payments_ServiceRequests_request_id",
                        column: x => x.request_id,
                        principalSchema: "dbo",
                        principalTable: "ServiceRequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    result = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    method = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Verifications_ServiceRequests_request_id",
                        column: x => x.request_id,
                        principalSchema: "dbo",
                        principalTable: "ServiceRequests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalSignatures",
                schema: "dbo",
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
                    table.PrimaryKey("PK_DigitalSignatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_DigitalSignatures_Certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DigitalSignatures_Devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "dbo",
                        principalTable: "Devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DigitalSignatures_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeBreakdowns",
                schema: "dbo",
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
                    table.PrimaryKey("PK_FeeBreakdowns", x => x.id);
                    table.ForeignKey(
                        name: "FK_FeeBreakdowns_JournalEntries_journal_entry_id",
                        column: x => x.journal_entry_id,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Signers",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    journal_entry_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Signers_JournalEntries_journal_entry_id",
                        column: x => x.journal_entry_id,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RonTechnologies",
                schema: "dbo",
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
                    table.PrimaryKey("PK_RonTechnologies", x => x.id);
                    table.ForeignKey(
                        name: "FK_RonTechnologies_NotaryCapabilities_capability_id",
                        column: x => x.capability_id,
                        principalSchema: "dbo",
                        principalTable: "NotaryCapabilities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorityScopes",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    commission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    authority_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorityScopes", x => x.id);
                    table.ForeignKey(
                        name: "FK_AuthorityScopes_NotaryCommissions_commission_id",
                        column: x => x.commission_id,
                        principalSchema: "dbo",
                        principalTable: "NotaryCommissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seals",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Seals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Seals_NotaryCommissions_commission_id",
                        column: x => x.commission_id,
                        principalSchema: "dbo",
                        principalTable: "NotaryCommissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seals_Seals_replace_seal_id",
                        column: x => x.replace_seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ActLogEntries",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notary_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActLogEntries", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActLogEntries_NotarialActs_act_id",
                        column: x => x.act_id,
                        principalSchema: "dbo",
                        principalTable: "NotarialActs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActLogEntries_Notaries_notary_id",
                        column: x => x.notary_id,
                        principalSchema: "dbo",
                        principalTable: "Notaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActSignatures",
                schema: "dbo",
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
                    table.PrimaryKey("PK_ActSignatures", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActSignatures_NotarialActs_act_id",
                        column: x => x.act_id,
                        principalSchema: "dbo",
                        principalTable: "NotarialActs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActSignatures_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceReviews",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    act_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    result = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceReviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_ComplianceReviews_NotarialActs_act_id",
                        column: x => x.act_id,
                        principalSchema: "dbo",
                        principalTable: "NotarialActs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiometricData",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    signer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    signature_image = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiometricData", x => x.id);
                    table.ForeignKey(
                        name: "FK_BiometricData_Signers_signer_id",
                        column: x => x.signer_id,
                        principalSchema: "dbo",
                        principalTable: "Signers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    reported_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reported_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    seal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    certificate_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.id);
                    table.ForeignKey(
                        name: "FK_Incidents_Certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Incidents_Seals_seal_id",
                        column: x => x.seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_reported_by",
                        column: x => x.reported_by,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SealUsageLogs",
                schema: "dbo",
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
                    table.PrimaryKey("PK_SealUsageLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_SealUsageLogs_Seals_seal_id",
                        column: x => x.seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SealUsageLogs_Users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Revocations",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Revocations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Revocations_Certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Revocations_Incidents_incident_id",
                        column: x => x.incident_id,
                        principalSchema: "dbo",
                        principalTable: "Incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Revocations_Seals_seal_id",
                        column: x => x.seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Revocations_Users_performed_by",
                        column: x => x.performed_by,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Replacements",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Replacements", x => x.id);
                    table.ForeignKey(
                        name: "FK_Replacements_Certificates_new_certificate_id",
                        column: x => x.new_certificate_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Replacements_Certificates_old_certificate_id",
                        column: x => x.old_certificate_id,
                        principalSchema: "dbo",
                        principalTable: "Certificates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Replacements_Revocations_revocation_id",
                        column: x => x.revocation_id,
                        principalSchema: "dbo",
                        principalTable: "Revocations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Replacements_Seals_new_seal_id",
                        column: x => x.new_seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Replacements_Seals_old_seal_id",
                        column: x => x.old_seal_id,
                        principalSchema: "dbo",
                        principalTable: "Seals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Replacements_Users_performed_by",
                        column: x => x.performed_by,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActLogEntries_act_id",
                schema: "dbo",
                table: "ActLogEntries",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_ActLogEntries_notary_id",
                schema: "dbo",
                table: "ActLogEntries",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_ActSignatures_act_id",
                schema: "dbo",
                table: "ActSignatures",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_ActSignatures_user_id",
                schema: "dbo",
                table: "ActSignatures",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_entity_type_entity_id",
                schema: "dbo",
                table: "AuditLogs",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_timestamp",
                schema: "dbo",
                table: "AuditLogs",
                column: "timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_user_id",
                schema: "dbo",
                table: "AuditLogs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityScopes_commission_id",
                schema: "dbo",
                table: "AuthorityScopes",
                column: "commission_id");

            migrationBuilder.CreateIndex(
                name: "IX_BiometricData_signer_id",
                schema: "dbo",
                table: "BiometricData",
                column: "signer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ca_id",
                schema: "dbo",
                table: "Certificates",
                column: "ca_id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_device_id",
                schema: "dbo",
                table: "Certificates",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_hsm_key_id",
                schema: "dbo",
                table: "Certificates",
                column: "hsm_key_id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_owner_user_id",
                schema: "dbo",
                table: "Certificates",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_replace_cert_id",
                schema: "dbo",
                table: "Certificates",
                column: "replace_cert_id");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_status",
                schema: "dbo",
                table: "Certificates",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceReviews_act_id",
                schema: "dbo",
                table: "ComplianceReviews",
                column: "act_id");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_request_id",
                schema: "dbo",
                table: "Deliveries",
                column: "request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_user_id",
                schema: "dbo",
                table: "Devices",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalSignatures_certificate_id",
                schema: "dbo",
                table: "DigitalSignatures",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalSignatures_device_id",
                schema: "dbo",
                table: "DigitalSignatures",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "IX_DigitalSignatures_user_id",
                schema: "dbo",
                table: "DigitalSignatures",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_request_id",
                schema: "dbo",
                table: "Documents",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExportHistories_requested_by",
                schema: "dbo",
                table: "ExportHistories",
                column: "requested_by");

            migrationBuilder.CreateIndex(
                name: "IX_FeeBreakdowns_journal_entry_id",
                schema: "dbo",
                table: "FeeBreakdowns",
                column: "journal_entry_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_certificate_id",
                schema: "dbo",
                table: "Incidents",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_reported_at",
                schema: "dbo",
                table: "Incidents",
                column: "reported_at");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_reported_by",
                schema: "dbo",
                table: "Incidents",
                column: "reported_by");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_seal_id",
                schema: "dbo",
                table: "Incidents",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_job_id",
                schema: "dbo",
                table: "JobAssignments",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_notary_id_status",
                schema: "dbo",
                table: "JobAssignments",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_client_id",
                schema: "dbo",
                table: "Jobs",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_status",
                schema: "dbo",
                table: "Jobs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_JobStatusLogs_job_id",
                schema: "dbo",
                table: "JobStatusLogs",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_notary_id",
                schema: "dbo",
                table: "JournalEntries",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_status",
                schema: "dbo",
                table: "JournalEntries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_lang_code",
                schema: "dbo",
                table: "Languages",
                column: "lang_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotarialActs_jurisdiction_id",
                schema: "dbo",
                table: "NotarialActs",
                column: "jurisdiction_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotarialActs_notary_id",
                schema: "dbo",
                table: "NotarialActs",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotarialActs_request_id",
                schema: "dbo",
                table: "NotarialActs",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotarialActs_status",
                schema: "dbo",
                table: "NotarialActs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Notaries_email",
                schema: "dbo",
                table: "Notaries",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Notaries_ssn",
                schema: "dbo",
                table: "Notaries",
                column: "ssn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notaries_status",
                schema: "dbo",
                table: "Notaries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Notaries_user_id",
                schema: "dbo",
                table: "Notaries",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotaryAuditLogs_notary_id_created_at",
                schema: "dbo",
                table: "NotaryAuditLogs",
                columns: new[] { "notary_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_NotaryAvailabilities_notary_id",
                schema: "dbo",
                table: "NotaryAvailabilities",
                column: "notary_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotaryBonds_notary_id",
                schema: "dbo",
                table: "NotaryBonds",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotaryCapabilities_notary_id",
                schema: "dbo",
                table: "NotaryCapabilities",
                column: "notary_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotaryCommissions_commission_state",
                schema: "dbo",
                table: "NotaryCommissions",
                column: "commission_state");

            migrationBuilder.CreateIndex(
                name: "IX_NotaryCommissions_expiration_date",
                schema: "dbo",
                table: "NotaryCommissions",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "IX_NotaryCommissions_notary_id_status",
                schema: "dbo",
                table: "NotaryCommissions",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_NotaryDocuments_notary_id_doc_category",
                schema: "dbo",
                table: "NotaryDocuments",
                columns: new[] { "notary_id", "doc_category" });

            migrationBuilder.CreateIndex(
                name: "IX_NotaryIncidents_notary_id_status",
                schema: "dbo",
                table: "NotaryIncidents",
                columns: new[] { "notary_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_NotaryInsurances_notary_id",
                schema: "dbo",
                table: "NotaryInsurances",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotaryServiceAreas_notary_id_state_id",
                schema: "dbo",
                table: "NotaryServiceAreas",
                columns: new[] { "notary_id", "state_id" });

            migrationBuilder.CreateIndex(
                name: "IX_NotaryServiceAreas_state_id",
                schema: "dbo",
                table: "NotaryServiceAreas",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_NotaryStatusHistory_notary_id",
                schema: "dbo",
                table: "NotaryStatusHistory",
                column: "notary_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_event_id",
                schema: "dbo",
                table: "Notifications",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_job_id",
                schema: "dbo",
                table: "Notifications",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_request_id",
                schema: "dbo",
                table: "Payments",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_new_certificate_id",
                schema: "dbo",
                table: "Replacements",
                column: "new_certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_new_seal_id",
                schema: "dbo",
                table: "Replacements",
                column: "new_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_old_certificate_id",
                schema: "dbo",
                table: "Replacements",
                column: "old_certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_old_seal_id",
                schema: "dbo",
                table: "Replacements",
                column: "old_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_performed_by",
                schema: "dbo",
                table: "Replacements",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "IX_Replacements_revocation_id",
                schema: "dbo",
                table: "Replacements",
                column: "revocation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_certificate_id",
                schema: "dbo",
                table: "Revocations",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_incident_id",
                schema: "dbo",
                table: "Revocations",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_performed_by",
                schema: "dbo",
                table: "Revocations",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "IX_Revocations_seal_id",
                schema: "dbo",
                table: "Revocations",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_role_name",
                schema: "dbo",
                table: "Roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RonTechnologies_capability_id",
                schema: "dbo",
                table: "RonTechnologies",
                column: "capability_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seals_commission_id",
                schema: "dbo",
                table: "Seals",
                column: "commission_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seals_notarial_act_id",
                schema: "dbo",
                table: "Seals",
                column: "notarial_act_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seals_replace_seal_id",
                schema: "dbo",
                table: "Seals",
                column: "replace_seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seals_status",
                schema: "dbo",
                table: "Seals",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_SealUsageLogs_seal_id",
                schema: "dbo",
                table: "SealUsageLogs",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_SealUsageLogs_user_id",
                schema: "dbo",
                table: "SealUsageLogs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_customer_id",
                schema: "dbo",
                table: "ServiceRequests",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_organization_id",
                schema: "dbo",
                table: "ServiceRequests",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_Signers_journal_entry_id",
                schema: "dbo",
                table: "Signers",
                column: "journal_entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_States_state_code",
                schema: "dbo",
                table: "States",
                column: "state_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                schema: "dbo",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_id_role",
                schema: "dbo",
                table: "Users",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "IX_Users_status",
                schema: "dbo",
                table: "Users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_request_id",
                schema: "dbo",
                table: "Verifications",
                column: "request_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActLogEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ActSignatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuthorityScopes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BiometricData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ComplianceReviews",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deliveries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DigitalSignatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExportHistories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FeeBreakdowns",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JobAssignments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JobStatusLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryAuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryAvailabilities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryBonds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryDocuments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryIncidents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryInsurances",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryServiceAreas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryStatusHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Replacements",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RonTechnologies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SealUsageLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Verifications",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Signers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotarialActs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "States",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Revocations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryCapabilities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ServiceRequests",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Incidents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Certificates",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Seals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CertificateAuthorities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HsmKeyStorages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotaryCommissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notaries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");
        }
    }
}
