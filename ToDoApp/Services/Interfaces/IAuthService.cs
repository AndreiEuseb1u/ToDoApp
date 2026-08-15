using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Models;

namespace ToDoApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> SignUp(string email, string password);
        Task<AuthResult> SignIn(string email, string password);
        Task<AuthResult> SignOut();
        Task<AuthResult> ResetPassword(string email);
        Task<AuthResult> ConfirmPasswordReset(string newPassword);
        Guid? GetCurrentUserId();
    }
}
