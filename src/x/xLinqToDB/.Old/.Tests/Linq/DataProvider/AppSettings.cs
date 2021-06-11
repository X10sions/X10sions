//using System.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace LinqToDB.Tests.Linq.DataProvider {
  public static class AppSettings {
    static AppSettings() {
      // https://stackoverflow.com/questions/43010660/xunit-test-project-connection-string

      LocalConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      AppSettingsPath = LocalConfiguration["AppSettingsPath"];
      RemoteConfiguration = new ConfigurationBuilder().AddJsonFile(AppSettingsPath).Build();

      //config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      //var connectionString = config["Data:DefaultConnection:ConnectionString"];
      //DbContextOptions = new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(connectionString).Options;
    }

    public static string? AppSettingsPath { get; }
    public static IConfiguration LocalConfiguration { get; }
    public static IConfiguration RemoteConfiguration { get; }
    //public static string GetConnectionString(string name) => Configuration.GetConnectionString(name);


  }


  public class AppSettingsTests {
    public AppSettingsTests(ITestOutputHelper output) {
      //this.output = output;
      output.WriteLine($"{nameof(AppSettings.AppSettingsPath)}: {AppSettings.AppSettingsPath}");
    }
    //private readonly ITestOutputHelper output;

    [Fact] public void TestAppSettingsPathExists() => Assert.True(System.IO.File.Exists(AppSettings.AppSettingsPath));
    [Fact] public void TestLocalConfiguration() => Assert.NotNull(AppSettings.LocalConfiguration);
    [Fact] public void TestRemoteConfiguration() => Assert.NotNull(AppSettings.RemoteConfiguration);
  }

}