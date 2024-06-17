using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using X10sions.Playground.Razor;

namespace X10sions.Playground.Apps.Wpf {
  /// <summary>
  /// https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/wpf?view=aspnetcore-6.0
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      var services = new ServiceCollection();
      services.AddBlazorWebView();
      services.AddWpfBlazorWebView();
      services.AddScoped<HttpClient>();

      services.AddAppSettings();

      Resources.Add("services", services.BuildServiceProvider());
      InitializeComponent();
    }
  }
}
