using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ToDoApp.Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static bool TryGetUserId(this ControllerBase controller, out Guid userId)
        {
            var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdClaim, out userId);
        }
    }
}
