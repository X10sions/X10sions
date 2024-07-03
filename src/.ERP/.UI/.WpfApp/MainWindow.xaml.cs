using Common.Features.DummyFakeExamples.WeatherForecast;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using X10sions.ERP.Data.Models;

namespace X10sions.ERP.WpfApp {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddBlazorWebView();
      serviceCollection.AddSingleton(_appState);
      serviceCollection.AddSingleton<WeatherForecastService>();
      Resources.Add("services", serviceCollection.BuildServiceProvider());

      InitializeComponent();
    }
    private readonly AppState _appState = new();

    private void Button_Click(object sender, RoutedEventArgs e) {
      MessageBox.Show(this, $"Current counter value is: {_appState.Counter}", "Counter");
    }
    
  }

  public partial class Main {
    // Workaround for compiler error "error MC3050: Cannot find the type 'local:Main'"
    // It seems that, although WPF's design-time build can see Razor components, its runtime build cannot.
  }

}