namespace Common.Data;

public class DbConnectionString : IDbConnectionString {
  public DbConnectionString(string value) {
    Value = value;
  }
  public DbConnectionString(string key, string value, string? providerName = null) : this(value) {
    Key = key;
    Provider = providerName;
  }

  public string? Key { get; }
  public string? Provider { get; }
  public string Value { get; }
}