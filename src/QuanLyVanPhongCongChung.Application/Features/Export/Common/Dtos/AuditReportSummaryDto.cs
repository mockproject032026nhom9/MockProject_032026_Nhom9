namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record AuditReportSummaryDto(
    int TotalRecords,
    int VerifiedCount,
    int PendingAuthCount,
    int ArchivedCount);
