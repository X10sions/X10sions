namespace Common.App.Settings;

public interface IApplicationDetailAppSettings {
  string? ApplicationName { get; }
  string? Description { get; }
  string? ContactWebsite { get; }
  string? LicenseDetail { get; }
}
