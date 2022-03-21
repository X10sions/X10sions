namespace Common.Data;
public interface IAppSettingsConnectionStrings<TAppSettings> where TAppSettings : Dictionary<string, string> {
  TAppSettings ConnectionStrings { get; set; }
}