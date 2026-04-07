namespace QuanLyVanPhongCongChung.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Persistence.Context;
using QuanLyVanPhongCongChung.Persistence.Interceptors;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.");

        services.AddScoped<AuditableEntityInterceptor>();

        // Write context
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, sqlServer =>
            {
                sqlServer.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sqlServer.EnableRetryOnFailure(maxRetryCount: 3);
            });
            options.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
        });

        // Read context (same DB, no tracking)
        services.AddDbContext<ReadOnlyApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        // Interface registrations
        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IReadOnlyApplicationDbContext>(sp =>
            sp.GetRequiredService<ReadOnlyApplicationDbContext>());
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
