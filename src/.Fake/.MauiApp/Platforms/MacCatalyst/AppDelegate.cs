using Foundation;

namespace X10sions.Fake.MauiApp {
  [Register("AppDelegate")]
  public class AppDelegate : MauiUIApplicationDelegate {
    protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
  }
}