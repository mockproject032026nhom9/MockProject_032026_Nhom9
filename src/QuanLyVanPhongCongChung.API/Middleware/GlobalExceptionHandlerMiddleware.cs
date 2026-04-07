namespace QuanLyVanPhongCongChung.API.Middleware;

using System.Text.Json;
using QuanLyVanPhongCongChung.Application.Common.Exceptions;
using QuanLyVanPhongCongChung.Application.Common.Models;
using QuanLyVanPhongCongChung.Domain.Exceptions;

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (StatusCodes.Status400BadRequest, validationEx.Message),
            NotFoundException notFoundEx => (StatusCodes.Status404NotFound, notFoundEx.Message),
            ForbiddenAccessException forbiddenEx => (StatusCodes.Status403Forbidden, forbiddenEx.Message),
            DomainException domainEx => (StatusCodes.Status400BadRequest, domainEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception occurred");
        }
        else
        {
            logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse.FailResponse(message, statusCode);

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsJsonAsync(response, jsonOptions);
    }
}
