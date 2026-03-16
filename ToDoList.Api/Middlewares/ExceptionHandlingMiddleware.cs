using System.Diagnostics;
using System.Text.Json;
using ToDoList.Service.Common;
using ToDoList.Service.Common.Exception;

namespace ToDoList.Api.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            var statusCode = exception switch
            {
                DomainValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var errors = exception switch
            {
                DomainValidationException vex => vex.Errors?.ToList() ?? [vex.Message],
                _ => null
            };

            var apiResponse = ApiResponse<object>.Fail(
                message: exception.Message,
                errors: errors,
                statusCode: statusCode,
                traceId: traceId
            );

            var payload = JsonSerializer.Serialize(apiResponse, _jsonOptions);
            return context.Response.WriteAsync(payload);
        }
    }
}
