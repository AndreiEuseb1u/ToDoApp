namespace ToDoApp.Common
{
    public class ApiResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }

        public ApiErrorType ErrorType { get; init; }

        public static ApiResult Ok() => new() { Success = true };
        public static ApiResult Fail(string errorMessage, ApiErrorType errorType) => new() 
        { 
            Success = false, 
            ErrorMessage = errorMessage, 
            ErrorType = errorType
        };
    }

    public class ApiResult<T> : ApiResult
    {
        public T? Data { get; init; }
        
        public static ApiResult<T> Ok(T data) => new() { Success = true, Data = data };
        public new static ApiResult<T> Fail(string errorMessage, ApiErrorType errorType) => new() 
        { 
            Success = false, 
            ErrorMessage = errorMessage,
            ErrorType = errorType
        };
    }
}
