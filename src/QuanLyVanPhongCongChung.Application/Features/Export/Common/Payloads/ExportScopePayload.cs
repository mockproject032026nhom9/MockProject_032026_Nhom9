namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record ExportScopePayload(
    string Token,
    Guid DocumentEntityId,
    string DocumentId,
    string Format,
    DateTimeOffset ExpiresAt,
    string Scope);
