using Microsoft.Extensions.DependencyInjection;
using Supabase.Interfaces;
using ToDoApp.Services.Implementations;

namespace ToDoApp
{
    public partial class App : Application
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly SessionPersistenceService _sessionPersistence;
        public App(Supabase.Client supabaseClient, SessionPersistenceService sessionPersistence)
        {
            _supabaseClient = supabaseClient;
            _sessionPersistence = sessionPersistence;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _supabaseClient.InitializeAsync();

            if (_supabaseClient.Auth.CurrentUser is null)
            {
                var savedSession = _sessionPersistence.LoadSession();

                if (savedSession?.AccessToken is not null && savedSession?.RefreshToken is not null)
                {
                    await _supabaseClient.Auth.SetSession(savedSession.AccessToken, savedSession.RefreshToken);
                }
            }

            if (_supabaseClient.Auth.CurrentUser is not null)
            {
                Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}