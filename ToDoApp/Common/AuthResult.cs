using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApp.Common
{
    public class AuthResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }

        public static AuthResult Ok() => new() { Success = true };
        public static AuthResult Fail(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
    }
}
