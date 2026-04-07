namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record SecureExportAccessDto(
    Guid ExportHistoryId,
    Guid DocumentEntityId,
    string DocumentId,
    string Format,
    DateTimeOffset ExpiresAt,
    bool IsExpired,
    string Scope);
