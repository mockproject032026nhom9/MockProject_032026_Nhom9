namespace QuanLyVanPhongCongChung.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IExportAuditPdfService, ExportAuditPdfService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
