using System.Collections.Generic;

namespace Common.AspNetCore.Configuration.Options {
  public interface ILogLevelOptions {
    IDictionary<string, string> Items { get; set; }
    string Default { get; set; }
    string System { get; set; }
    string Microsoft { get; set; }
  }

  public static class ILogLevelOptionsExtensions {

    public static string GetDefault(this ILogLevelOptions options) => options.Items[nameof(ILogLevelOptions.Default)];
    public static string GetMicrosoft(this ILogLevelOptions options) => options.Items[nameof(ILogLevelOptions.Microsoft)];
    public static string GetSystem(this ILogLevelOptions options) => options.Items[nameof(ILogLevelOptions.System)];

    public static void SetDefault(this ILogLevelOptions options, string value) => options.Items[nameof(ILogLevelOptions.Default)] = value;
    public static void SetMicrosoft(this ILogLevelOptions options, string value) => options.Items[nameof(ILogLevelOptions.Microsoft)] = value;
    public static void SetSystem(this ILogLevelOptions options, string value) => options.Items[nameof(ILogLevelOptions.System)] = value;
  }

}