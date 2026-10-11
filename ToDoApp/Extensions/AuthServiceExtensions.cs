using ToDoApp.Services.Interfaces;

namespace ToDoApp.Extensions
{
    public static class AuthServiceExtensions
    {
        public static async Task<Guid?> GetUserIdOrRedirectAsync(this IAuthService authService)
        {
            var userId = authService.GetCurrentUserId();

            if (userId is null)
            {
                await Shell.Current.GoToAsync("//SignInPage");
                return null;
            }

            return userId;
        }
    }
}
