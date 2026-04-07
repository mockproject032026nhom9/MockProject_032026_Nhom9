namespace QuanLyVanPhongCongChung.Application.Common.Behaviors;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;

public sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    ICurrentUserService currentUserService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int WarningThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > WarningThresholdMs)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId ?? "Anonymous";

            logger.LogWarning(
                "Long running request: {RequestName} ({ElapsedMs}ms) by {UserId}",
                requestName, stopwatch.ElapsedMilliseconds, userId);
        }

        return response;
    }
}
