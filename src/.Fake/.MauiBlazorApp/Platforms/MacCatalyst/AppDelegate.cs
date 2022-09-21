using Foundation;

namespace X10sions.Fake.MauiBlazorApp {
  [Register("AppDelegate")]
  public class AppDelegate : MauiUIApplicationDelegate {
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
  }
}