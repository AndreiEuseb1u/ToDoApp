using Microsoft.Extensions.Logging;
using ToDoApp.Services.Implementations;
using ToDoApp.Services.Interfaces;
using ToDoApp.ViewModels;
using ToDoApp.Views;

namespace ToDoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomEntry", (handler, view) =>
                    {
#if ANDROID
                        handler.PlatformView.BackgroundTintList =
                            Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                        {
                            handler.PlatformView.TextCursorDrawable =
                                new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.ParseColor("#F2BB05"));
                        }
#endif
                    });
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<SessionPersistenceService>();

            builder.Services.AddSingleton(sp => new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5246/")
            });

            builder.Services.AddSingleton(sp =>
            {
                var sessionPersistence = sp.GetRequiredService<SessionPersistenceService>();

                var options = new Supabase.SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false,
                    SessionHandler = sessionPersistence

                };

                var client = new Supabase.Client(
                    "https://vrwhinokcfhaqipqrnqn.supabase.co", 
                    "sb_publishable_H7NuxyCFH7fTxtlLdP7N8A_OUhewrCR",
                    options);

                return client;
            }); 


            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddSingleton<ITaskApiService, TaskApiService>();

            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<MainPageView>();

            builder.Services.AddSingleton<SignUpPageViewModel>();

            builder.Services.AddSingleton<SignUpPageView>();

            builder.Services.AddSingleton<SignInPageViewModel>();

            builder.Services.AddSingleton<SignInPageView>();

            return builder.Build();
        }
    }
}
