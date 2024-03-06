namespace Common.App.Settings;

public interface IAppSettings {
  string AppTitle { get; }
  DateTime Updated { get; }
}
