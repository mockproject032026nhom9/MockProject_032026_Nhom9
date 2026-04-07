namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record ExportAuditMetadataPayload(
    string DocumentId,
    string Format,
    string Status,
    string Source);
