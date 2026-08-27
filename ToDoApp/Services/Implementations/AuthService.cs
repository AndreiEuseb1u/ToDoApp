using Supabase;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Common;
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
                    return AuthResult.Fail(AuthMessages.SignUp.Failed);
                }

                return AuthResult.Ok();
            }

            catch (Exception ex)
            {
                return AuthResult.Fail(AuthMessages.SignUp.Failed);
            }
        }

        public async Task<AuthResult> SignIn(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignIn(email, password);

                if (session is null || session.User is null)
                {
                    return AuthResult.Fail(AuthMessages.SignIn.Failed);
                }

                return AuthResult.Ok();
            }

            catch (Exception ex)
            {
                return AuthResult.Fail(AuthMessages.SignIn.InvalidCredentials);
            }
        }

        public async Task<AuthResult> SignOut()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();

                return AuthResult.Ok();
            }

            catch (Exception ex)
            {
                return AuthResult.Fail(AuthMessages.SignOut.Failed);
            }
        }

        public async Task<AuthResult> ResetPassword(string email)
        {
            try
            {
                await _supabaseClient.Auth.ResetPasswordForEmail(email);

                return AuthResult.Ok();
            }

            catch (Exception ex)
            {
                return AuthResult.Fail(AuthMessages.ResetPassword.Failed);
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
                    return AuthResult.Fail(AuthMessages.ConfirmPasswordReset.Failed);
                }

                return AuthResult.Ok();
            }
            catch (Exception ex)
            {
                return AuthResult.Fail(AuthMessages.ConfirmPasswordReset.GenericError);
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