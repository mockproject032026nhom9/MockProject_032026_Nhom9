namespace QuanLyVanPhongCongChung.API.Contracts.Exports;

public sealed record GenerateSecureLinkRequest(
    Guid DocumentEntityId,
    string DocumentId,
    string Format,
    int ExpireInMinutes = 60,
    string Scope = "Client");
