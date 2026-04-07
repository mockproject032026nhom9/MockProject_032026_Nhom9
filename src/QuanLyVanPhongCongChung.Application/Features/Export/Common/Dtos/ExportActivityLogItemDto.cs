namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record ExportActivityLogItemDto(
    Guid AuditLogId,
    string UserName,
    string RoleName,
    string DocumentId,
    string Format,
    DateTimeOffset Timestamp,
    string Status,
    string Action);
