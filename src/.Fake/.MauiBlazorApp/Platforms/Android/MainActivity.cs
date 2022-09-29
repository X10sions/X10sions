using Android.App;
using Android.Content.PM;

namespace X10sions.Fake.MauiBlazorApp {
  [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
  public class MainActivity : MauiAppCompatActivity {
  }
}