namespace QuanLyVanPhongCongChung.API.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLyVanPhongCongChung.API.Contracts.Auth;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Common.Models;
using QuanLyVanPhongCongChung.Domain.Enums;

public class AuthController(
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext,
    IConfiguration configuration) : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var email = request.Email?.Trim();
        var password = request.Password ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return BadRequest(ApiResponse.FailResponse("Email và password là bắt buộc."));

        var user = await readOnlyApplicationDbContext.Users
            .Where(x => x.Email == email)
            .Select(x => new
            {
                x.Id,
                x.Email,
                x.PasswordHash,
                x.FullName,
                x.Status,
                x.RoleId
            })
            .FirstOrDefaultAsync();

        if (user is null || user.Status != UserStatus.Active)
            return Unauthorized(ApiResponse.FailResponse("Thông tin đăng nhập không hợp lệ.", StatusCodes.Status401Unauthorized));

        if (!PasswordMatches(password, user.PasswordHash))
            return Unauthorized(ApiResponse.FailResponse("Thông tin đăng nhập không hợp lệ.", StatusCodes.Status401Unauthorized));

        var roleName = await readOnlyApplicationDbContext.Roles
            .Where(x => x.Id == user.RoleId)
            .Select(x => x.RoleName)
            .FirstOrDefaultAsync() ?? "User";

        var expiresAt = DateTimeOffset.UtcNow.AddHours(8);
        var token = GenerateJwtToken(user.Id, user.Email, user.FullName, roleName, expiresAt);

        var response = new LoginResponse(
            token,
            expiresAt,
            user.Id,
            user.Email,
            user.FullName,
            roleName);

        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    private string GenerateJwtToken(Guid userId, string email, string fullName, string roleName, DateTimeOffset expiresAt)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? "QuanLyVanPhongCongChung";
        var audience = jwtSection["Audience"] ?? "QuanLyVanPhongCongChung.Client";
        var signingKey = jwtSection["SigningKey"]
            ?? throw new InvalidOperationException("JWT signing key is not configured.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Role, roleName),
            new("role", roleName)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    private static bool PasswordMatches(string inputPassword, string storedHash)
    {
        var inputBytes = Encoding.UTF8.GetBytes(inputPassword);
        var storedBytes = Encoding.UTF8.GetBytes(storedHash ?? string.Empty);

        return CryptographicOperations.FixedTimeEquals(inputBytes, storedBytes);
    }
}
