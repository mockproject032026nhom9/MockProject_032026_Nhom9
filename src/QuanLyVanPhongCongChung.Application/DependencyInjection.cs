namespace QuanLyVanPhongCongChung.Application;

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QuanLyVanPhongCongChung.Application.Common.Behaviors;
using QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

        services.AddScoped<IExportAuditLogReader, ExportAuditLogReader>();
        services.AddScoped<IAuditReportFileBuilder, AuditReportFileBuilder>();

        return services;
    }
}
