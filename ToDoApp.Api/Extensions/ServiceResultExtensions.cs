using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Api.Common;

namespace ToDoApp.Api.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ActionResult ToErrorActionResult(this ServiceResult serviceResult, ControllerBase controller)
        {
            return serviceResult.ErrorType switch
            {
                ServiceErrorType.NotFound => controller.NotFound(serviceResult.ErrorMessage),

                ServiceErrorType.ValidationFailed => controller.BadRequest(serviceResult.ErrorMessage),

                _ => throw new InvalidOperationException($"Unhandled ServiceErrorType: {serviceResult.ErrorMessage}")
            };
        }
    }
}
