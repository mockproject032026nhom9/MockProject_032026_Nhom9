namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record AuditReportDto(
    DateTimeOffset GeneratedAt,
    string Format,
    AuditReportSummaryDto Summary,
    IReadOnlyCollection<ExportActivityLogItemDto> Items,
    string? FileName,
    string? FileContentType,
    string? FileContentBase64,
    string? Note);
