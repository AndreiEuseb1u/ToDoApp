namespace ToDoApp.Api.Common
{
    public class ServiceResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public ServiceErrorType ErrorType { get; init; } = ServiceErrorType.None;

        public static ServiceResult Ok() =>
            new() { Success = true};
        public static ServiceResult Fail(string errorMessage, ServiceErrorType errorType) =>
            new() { Success = false, ErrorMessage = errorMessage, ErrorType =  errorType};
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; init; }

        public static ServiceResult<T> Ok(T data) => 
            new() { Success = true, Data = data };
        public new static ServiceResult<T> Fail(string errorMessage, ServiceErrorType errorType) => 
            new() { Success = false, ErrorMessage = errorMessage, ErrorType = errorType };
    }
}
