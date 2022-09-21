using Android.App;
using Android.Runtime;

namespace X10sions.Fake.MauiBlazorApp {
  [Application]
  public class MainApplication : MauiApplication {
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
      : base(handle, ownership) {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
  }
}