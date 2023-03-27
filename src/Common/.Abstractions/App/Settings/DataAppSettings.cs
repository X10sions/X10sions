namespace Common.App.Settings;

public interface IDataAppSettings {
  ConnectionStringsAppSettings ConnectionStrings { get; set; }
}

public class DataAppSettings : AppSettings, IDataAppSettings {
  public ConnectionStringsAppSettings ConnectionStrings { get; set; }
  public DateTime Updated { get; set; } = DateTime.Now;
}
