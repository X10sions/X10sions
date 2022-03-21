namespace Common.Data;

public class DbSystem {
  public string? DefaultConnectionString { get; set; }
  public Dictionary<string, DbProvider> ProviderNames { get; set; } = new Dictionary<string, DbProvider>();
  public Dictionary<string, string> Databases { get; set; } = new Dictionary<string, string>();
}