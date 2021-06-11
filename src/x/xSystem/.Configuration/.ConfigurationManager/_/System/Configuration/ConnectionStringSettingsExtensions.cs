using System.Collections.Generic;
using System.Data.Common;

namespace System.Configuration {
  public static class ConnectionStringSettingsExtensions {
    public static DbConnectionStringBuilder DbConnectionStringBuilder(this ConnectionStringSettings connectionStringSettings) => new DbConnectionStringBuilder { ConnectionString = connectionStringSettings.ConnectionString };
    public static Dictionary<string, string> ConnectionStringDictionary(this ConnectionStringSettings connectionStringSettings) => connectionStringSettings.ConnectionString.ToKeyValueDictionary(';', '=');
    public static string ConnectionStringPropertyValue(this ConnectionStringSettings connectionStringSettings, string propertyName) => connectionStringSettings.ConnectionStringDictionary()[propertyName];
  }
}