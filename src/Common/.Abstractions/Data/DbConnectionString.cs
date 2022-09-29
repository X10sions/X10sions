namespace Common.Data;

public class DbConnectionString : IDbConnectionString {
  public DbConnectionString(string value) {
    Value = value;
  }
  public DbConnectionString(string key, string value, string? providerName = null) : this(value) {
    Key = key;
    ProviderName = providerName;
  }

  public string? Key { get; }
  public string? ProviderName { get; }
  public string Value { get; }
}