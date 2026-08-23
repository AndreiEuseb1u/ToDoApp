using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Helpers.Auth;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.ViewModels
{
    public partial class SignUpPageViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string _errorMessage = string.Empty;

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        public SignUpPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            var errors = PasswordValidator.Validate(Password);

            if (errors.Count > 0)
            {
                ErrorMessage = "Password must contain: \n" + string.Join("\n", errors);
                return;
            }

            var result = await _authService.SignUp(Email, Password);

            if (result.Success is false)
            {
                ErrorMessage = result.ErrorMessage!;
                return;
            }

            await Shell.Current.GoToAsync("//SignInPage");
        }

        [RelayCommand]
        private async Task GoToSignIn()
        {
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}
