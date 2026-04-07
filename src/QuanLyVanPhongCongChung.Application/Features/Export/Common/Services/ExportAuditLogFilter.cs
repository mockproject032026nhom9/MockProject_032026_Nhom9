namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

public sealed record ExportAuditLogFilter(
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    string? DocumentId = null,
    string? Format = null,
    string? Status = null);
