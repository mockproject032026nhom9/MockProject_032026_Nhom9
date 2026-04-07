namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.PrintAuditReport;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record PrintAuditReportQuery(
    string Format = "CSV",
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    string? DocumentId = null) : IRequest<AuditReportDto>;
