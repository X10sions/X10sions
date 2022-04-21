using System.Data.Common;

namespace Common.Data;
public interface IHaveConnectionStringsAppSettings {
  ConnectionStringsAppSettings ConnectionStrings { get; }
}

public class ConnectionStringsAppSettings : Dictionary<string, string> { }


public interface IHaveConnectionStringProvidersAppSettings {
  ConnectionStringProvidersAppSettings ConnectionStringProviders { get; }
}

public class ConnectionStringProvidersAppSettings : Dictionary<string, ConnectionStringsAppSettings> {

  public IEnumerable<IDbConnectionString> GetDbConnectionStrings() => 
    from provider in this 
    from cs in provider.Value 
    select new DbConnectionString(cs.Key, cs.Value, provider.Key);

  public IEnumerable<IDbConnectionString> GetProvidr(string providerName) =>
    from provider in this
    from cs in provider.Value
    select new DbConnectionString(cs.Key, cs.Value, provider.Key);

}

public static class IHaveConnectionStringProvidersAppSettingsExtensions {

  public static ConnectionStringsAppSettings GetConnectionStrings(this IHaveConnectionStringProvidersAppSettings settings, string providerName)
    => settings.ConnectionStringProviders[providerName];

  public static string? GetConnectionString(this IHaveConnectionStringProvidersAppSettings settings, string providerName, string connectionStringName)
    => settings.GetConnectionStrings(providerName)[connectionStringName];

  public static DbConnectionStringBuilder GetDbConnectionStringBuilder(this IHaveConnectionStringProvidersAppSettings settings, string providerName, string connectionStringName, bool useOdbcRules)
    => new DbConnectionStringBuilder(useOdbcRules) { ConnectionString = settings.GetConnectionStrings(providerName)[connectionStringName] };

}