using System.Configuration;

namespace Common.Data;
public class DbConnectionSettings : Dictionary<string, DbSystem> {
  public DbConnectionSettings() : base(StringComparer.OrdinalIgnoreCase) { }

  public static string GetName(string dbName, string providerName) => $"{dbName}:{providerName}";

  public List<ConnectionStringSettings> GetConnectionStringSettings()
    => (from dbSys in this
        from dbProv in dbSys.Value.ProviderNames
        from db in dbSys.Value.Databases
        select new ConnectionStringSettings {
          ConnectionString = dbProv.Value.ReplaceKeyWords($"{db.Value}{dbProv.Value.DefaultConnectionString}{dbSys.Value.DefaultConnectionString}"),
            //Name = $"{db.Key}:{dbProd.Key}:{dbSys.Key}",
            Name = GetName(db.Key, dbProv.Key),
          ProviderName = dbProv.Key
        }).ToList();
}