using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ToDoApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Window is not null)
            {
                AndroidX.Core.View.WindowCompat.SetDecorFitsSystemWindows(Window, false);
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
                Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}
