using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApp.Helpers.Auth
{
    public class PasswordValidator
    {
        public static List<string> Validate(string password)
        {
            List<string> errors = new();

            if (password.Length < 8)
                errors.Add("• At least 8 characters");
            if (!password.Any(char.IsUpper))
                errors.Add("• One uppercase letter");
            if (!password.Any(char.IsLower))
                errors.Add("• One lowercase letter");
            if (!password.Any(char.IsDigit))
                errors.Add("• One number");
            if (!password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)))
                errors.Add("• One special character");

            return errors;
        }
    }
}
