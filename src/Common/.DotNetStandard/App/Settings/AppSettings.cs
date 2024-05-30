namespace Common.App.Settings;

public class AppSettings : IAppSettings {
  public string AppTitle { get; set; } = "-Unknown-";
  public DateTime Updated { get; set; } = DateTime.Now;
}