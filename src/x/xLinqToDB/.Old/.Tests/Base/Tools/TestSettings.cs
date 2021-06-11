using System.Collections.Generic;

namespace LinqToDB.Tests.Base.Tools {
  public class TestSettings {
    public string? BasedOn;
    public string[]? Providers;
    public string[]? Skip;
    public string? TraceLevel;
    public string? DefaultConfiguration;
    public string? NoLinqService;
    public Dictionary<string, TestConnection> Connections = new Dictionary<string, TestConnection>();
  }
}