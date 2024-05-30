namespace Common.App.Settings;

public class ApplicationDetailAppSettings : IApplicationDetailAppSettings {
  public string? ApplicationName { get; set; }
  public string? Description { get; set; }
  public string? ContactWebsite { get; set; }
  public string? LicenseDetail { get; set; }
}