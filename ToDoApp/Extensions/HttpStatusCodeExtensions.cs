using System.Net;
using ToDoApp.Common;

namespace ToDoApp.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static ApiErrorType ToApiErrorType(this HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => ApiErrorType.NotFound,
                HttpStatusCode.Conflict => ApiErrorType.Conflict,
                _ => ApiErrorType.Unknown
            };
        }
    }
}
