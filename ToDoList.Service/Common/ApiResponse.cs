namespace ToDoList.Service.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }
        public string? TraceId { get; set; }

        public static ApiResponse<T> Ok(T? data, string? message = null, int statusCode = 200) =>
            new() { Success = true, Data = data, Message = message, StatusCode = statusCode };

        public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null, int statusCode = 400, string? traceId = null) =>
            new() { Success = false, Message = message, Errors = errors?.ToList(), StatusCode = statusCode, TraceId = traceId };
    }
}
