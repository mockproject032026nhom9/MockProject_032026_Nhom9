namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

public sealed record AuditReportFileResult(
    string? FileName,
    string? FileContentType,
    string? FileContentBase64,
    string? Note);
