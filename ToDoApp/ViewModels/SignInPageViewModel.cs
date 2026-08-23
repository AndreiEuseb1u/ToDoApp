using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Models;
using ToDoApp.Services.Implementations;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.ViewModels
{
    public partial class SignInPageViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        private readonly SessionPersistenceService _sessionPersistence;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string _errorMessage = string.Empty;

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RememberMeIconSource))]
        private bool _isRememberMeChecked;

        public string RememberMeIconSource => IsRememberMeChecked
            ? "solar_check_square_bold_24px.png"
            : "solar_check_square_linear_24px.png";

        public SignInPageViewModel(IAuthService authService, SessionPersistenceService sessionPersistence)
        {
            _authService = authService;
            _sessionPersistence = sessionPersistence;
        }

        [RelayCommand]
        private void ToggleRememberMe()
        {
            IsRememberMeChecked = !IsRememberMeChecked;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";

                return;
            }

            _sessionPersistence.ShouldPersist = IsRememberMeChecked;

            var result = await _authService.SignIn(Email, Password);

            if (result.Success is false)
            {
                ErrorMessage = result.ErrorMessage!;

                return;
            }

            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        private async Task GoToSignUp()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
        }
    }
}
