namespace QuanLyVanPhongCongChung.Application.Common.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger,
    ICurrentUserService currentUserService
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.UserId ?? "Anonymous";

        logger.LogInformation("Handling {RequestName} by {UserId}", requestName, userId);

        var response = await next(cancellationToken);

        logger.LogInformation("Handled {RequestName} by {UserId}", requestName, userId);

        return response;
    }
}
