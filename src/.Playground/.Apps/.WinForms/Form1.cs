using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using X10sions.Playground ;
using X10sions.Playground.Razor ;

namespace SharedRazor.BlazorWinForms;
public partial class Form1 : Form {
  public Form1() {
    InitializeComponent();
    var services = new ServiceCollection();
    services.AddBlazorWebView();
    services.AddWindowsFormsBlazorWebView();

    services.AddAppSettings();

    blazorWebView1.HostPage = "wwwroot\\index.html";
    blazorWebView1.Services = services.BuildServiceProvider();
    blazorWebView1.RootComponents.Add<App>("#app");
  }
}
