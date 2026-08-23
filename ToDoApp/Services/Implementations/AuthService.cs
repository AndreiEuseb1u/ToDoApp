using Supabase;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Models;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly Supabase.Client _supabaseClient;

        public AuthService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<AuthResult> SignUp(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignUp(email, password);

                if (session?.User is null)
                {
                    return new AuthResult { Success = false, ErrorMessage = "Something went wrong during sign up. Please try again." };
                }

                return new AuthResult { Success = true };
            }

            catch (Exception ex)
            {
                return new AuthResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<AuthResult> SignIn(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignIn(email, password);

                if (session is null || session.User is null)
                {
                    return new AuthResult { Success = false, ErrorMessage = "Something went wrong during sign in. Please try again." };
                }

                return new AuthResult { Success = true };
            }
            
            catch (Exception ex)
            {
                return new AuthResult { Success = false, ErrorMessage = "Invalid email or password." };
            }
        }

        public async Task<AuthResult> SignOut()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();

                return new AuthResult { Success = true };
            }

            catch (Exception ex)
            {
                return new AuthResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<AuthResult> ResetPassword(string email)
        {
            try
            {
                await _supabaseClient.Auth.ResetPasswordForEmail(email);

                return new AuthResult { Success = true };
            }

            catch (Exception ex)
            {
                return new AuthResult { Success = false, ErrorMessage = "Something went wrong while trying to process your request. Please try again." };
            }
        }

        public async Task<AuthResult> ConfirmPasswordReset(string newPassword)
        {
            try
            {
                var userAttributes = new UserAttributes { Password = newPassword };

                var user = await _supabaseClient.Auth.Update(userAttributes);

                if (user is null)
                {
                    return new AuthResult { Success = false, ErrorMessage = "Something went wrong while trying to process your request. Please try again." };
                }

                return new AuthResult { Success = true };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = true, ErrorMessage = "Something went wrong. Please try again or request a new reset link." };
            }
        }

        public Guid? GetCurrentUserId()
        {
            var currentUserId = _supabaseClient.Auth.CurrentUser?.Id;

            if (Guid.TryParse(currentUserId, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
