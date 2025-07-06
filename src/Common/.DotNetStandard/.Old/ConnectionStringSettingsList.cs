using System.Configuration;
using System.Data;

namespace Common.Data;

public class ConnectionStringSettingsList : List<ConnectionStringSettings> {
  public ConnectionStringSettingsList() { }
  public ConnectionStringSettingsList(IEnumerable<ConnectionStringSettings> collection) : base(collection) { }

  public ConnectionStringSettings Get(string dbName, string providerName) => this.FirstOrDefault(x => x.Name.Equals(DbConnectionSettings.GetName(dbName, providerName), StringComparison.OrdinalIgnoreCase));
  public ConnectionStringSettings Get<T>(string dbName) where T : IDbConnection => Get(dbName, typeof(T).Namespace);
  //public ConnectionStringSettings Get(DbNames dbName, ProviderNames providerName) => Get(dbName.ToString() ,providerName.ToString());
  public ConnectionStringSettings GetFirst(string dbName) => this.FirstOrDefault(x => x.Name.Equals(dbName, StringComparison.OrdinalIgnoreCase));
  void Set(string name, ConnectionStringSettings value) {
    //int index = FindIndex(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    ///if (index > -1)
    //RemoveAt(index);
    RemoveAll(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    Add(value);
  }

}
