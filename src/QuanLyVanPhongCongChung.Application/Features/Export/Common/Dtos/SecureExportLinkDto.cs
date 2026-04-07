namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record SecureExportLinkDto(
    string Token,
    string SecureUrl,
    DateTimeOffset ExpiresAt,
    string DocumentId,
    string Format);
