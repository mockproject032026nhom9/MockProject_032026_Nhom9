namespace QuanLyVanPhongCongChung.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using System.Security.Claims;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
