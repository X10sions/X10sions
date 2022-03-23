using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using X10sions.ERP.Data.Models;
using X10sions.ERP.Data.Services;
using X10sions.ERP.Razor;

namespace X10sions.ERP.WinForms {
  public partial class Form1 : Form {
    public Form1() {
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddBlazorWebView();
      serviceCollection.AddSingleton(_appState);

      serviceCollection.AddSingleton<WeatherForecastService>();

      InitializeComponent();

      blazorWebView1.HostPage = @"wwwroot\index.html";
      blazorWebView1.Services = serviceCollection.BuildServiceProvider();
      blazorWebView1.RootComponents.Add<App>("#app");
    }

    private readonly AppState _appState = new();

    private void button1_Click(object sender, EventArgs e) {
      MessageBox.Show(
          owner: this,
          text: $"Current counter value is: {_appState.Counter}",
          caption: "Counter");
    }

  }
}