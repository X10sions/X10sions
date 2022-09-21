using Android.App;
using Android.Runtime;

namespace X10sions.Fake.MauiApp {
  [Application]
  public class MainApplication : MauiApplication {
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
      : base(handle, ownership) {
    }

    protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
  }
}