using System.Collections.Generic;

namespace Common.AspNetCore.Configuration.Options {
  public class LogLevelOptions : ILogLevelOptions {
    public LogLevelOptions() : this(false) { }

    public LogLevelOptions(bool isProductionEnvironment) {
      Default = isProductionEnvironment ? "Warning" : "Debug";
      Microsoft = "Information";
      System = "Information";
    }
    public string Default { get => this.GetDefault(); set => this.SetDefault(value); }
    public string Microsoft { get => this.GetMicrosoft(); set => this.SetMicrosoft(value); }
    public string System { get => this.GetSystem(); set => this.SetSystem(value); }
    public IDictionary<string, string> Items { get; set; } = new Dictionary<string, string>();

  }
}