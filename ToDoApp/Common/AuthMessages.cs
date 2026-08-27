using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApp.Common;

public static class AuthMessages
{
    public static class SignUp
    {
        public const string Failed = "Something went wrong during sign up. Please try again.";
    }

    public static class SignIn
    {
        public const string Failed = "Something went wrong during sign in. Please try again.";
        public const string InvalidCredentials = "Invalid email or password.";
    }

    public static class SignOut
    {
        public const string Failed = "Something went wrong while signing out. Please try again.";
    }

    public static class ResetPassword
    {
        public const string Failed = "Something went wrong while trying to process your request. Please try again.";
    }

    public static class ConfirmPasswordReset
    {
        public const string Failed = "Something went wrong while trying to process your request. Please try again.";
        public const string GenericError = "Something went wrong. Please try again or request a new reset link.";
    }
}
