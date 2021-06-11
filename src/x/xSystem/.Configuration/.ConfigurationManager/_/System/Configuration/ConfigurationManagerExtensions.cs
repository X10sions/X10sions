namespace System.Configuration {
  public static class ConfigurationManagerExtensions {

    public static string GetAppSetting(string key, string defaultValue = "-unknown-") => ConfigurationManager.AppSettings[key] ?? defaultValue;
    public static readonly string EnvironmentName = ConfigurationManagerExtensions.GetAppSetting(nameof(Environment), "-unknown-");

  }
}