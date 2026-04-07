namespace QuanLyVanPhongCongChung.Application.Common.Models;

public class ApiResponse<T>
{
    public int StatusCode { get; init; }
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200)
        => new() { StatusCode = statusCode, Success = true, Message = message, Data = data };

    public static ApiResponse<T> FailResponse(string message, int statusCode = 400)
        => new() { StatusCode = statusCode, Success = false, Message = message, Data = default };
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse SuccessResponse(string message = "Success", int statusCode = 200)
        => new() { StatusCode = statusCode, Success = true, Message = message };

    public new static ApiResponse FailResponse(string message, int statusCode = 400)
        => new() { StatusCode = statusCode, Success = false, Message = message };
}
