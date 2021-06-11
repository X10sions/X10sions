using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace System.Data {
  public static class IDbConnectionExtensions {

    public static string GetProviderName(this IDbConnection connection) {
      var csb = new DbConnectionStringBuilder {
        ConnectionString = connection.ConnectionString
      };
      const string provider = nameof(provider);
      if (csb.ContainsKey(provider))
        return csb[provider].ToString();
      var css = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().FirstOrDefault(x => x.ConnectionString == connection.ConnectionString);
      return css != null ? css.ProviderName : connection.ToString();
    }

  }
}