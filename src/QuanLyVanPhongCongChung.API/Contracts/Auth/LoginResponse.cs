namespace QuanLyVanPhongCongChung.API.Contracts.Auth;

public sealed record LoginResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    Guid UserId,
    string Email,
    string FullName,
    string Role);
