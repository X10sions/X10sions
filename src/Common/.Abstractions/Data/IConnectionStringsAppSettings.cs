using System.Runtime.Serialization;

namespace Common.Data;

public interface IConnectionStringsAppSettings : IDeserializationCallback, IReadOnlyDictionary<string, string>, ISerializable { }

public class ConnectionStringsAppSettings : Dictionary<string, string>, IConnectionStringsAppSettings { }

public class ConnectionStringsByProviderAppSettings : Dictionary<string, ConnectionStringsAppSettings> {

  public IDbConnectionString? GetDbConnectionString(string providerName, string csName) => (from provider in this
                                                                                            where provider.Key.Equals(providerName, true)
                                                                                            from cs in provider.Value
                                                                                            where cs.Key.Equals(csName, true)
                                                                                            select new DbConnectionString(cs.Key, cs.Value, provider.Key)).FirstOrDefault();

  public IEnumerable<IDbConnectionString> GetDbConnectionStrings() =>
    from provider in this
    from cs in provider.Value
    select new DbConnectionString(cs.Key, cs.Value, provider.Key);

  public IEnumerable<IDbConnectionString> GetDbConnectionStringsByProvider(string providerName) =>
    from provider in this
    where provider.Key.Equals(providerName, true)
    from cs in provider.Value
    select new DbConnectionString(cs.Key, cs.Value, provider.Key);

}

public static class IHaveConnectionStringProvidersAppSettingsExtensions {

  //  public static ConnectionStringsAppSettings GetConnectionStrings(this IHaveConnectionStringProvidersAppSettings settings, string providerName)
  //    => settings.ConnectionStringProviders[providerName];

  //  public static string? GetConnectionString(this IHaveConnectionStringProvidersAppSettings settings, string providerName, string connectionStringName)
  //    => settings.GetConnectionStrings(providerName)[connectionStringName];

  //  public static DbConnectionStringBuilder GetDbConnectionStringBuilder(this IHaveConnectionStringProvidersAppSettings settings, string providerName, string connectionStringName, bool useOdbcRules)
  //    => new DbConnectionStringBuilder(useOdbcRules) { ConnectionString = settings.GetConnectionStrings(providerName)[connectionStringName] };

}