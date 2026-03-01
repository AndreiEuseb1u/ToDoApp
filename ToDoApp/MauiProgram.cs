using Microsoft.Extensions.Logging;

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

            return builder.Build();
        }
    }
}
