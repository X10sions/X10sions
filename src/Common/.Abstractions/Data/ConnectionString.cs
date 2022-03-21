namespace Common.Data;

public class ConnectionString : IConnectionString {
  public ConnectionString(string value) {
    Value = value;
  }
  public ConnectionString(string key, string value, string? providerName = null) : this(value) {
    Key = key;
    ProviderName = providerName;
  }

  public string? Key { get; }
  public string? ProviderName { get; }
  public string Value { get; }

}