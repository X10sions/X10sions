using System.Linq;

namespace System.Data.Common {
  public static class DbConnectionStringBuilderExtensions {

    public static string DataSource(this DbConnectionStringBuilder csb) => (string)csb["Data Source"];

    public static string Provider(this DbConnectionStringBuilder csb) => (string)csb[nameof(Provider)];

    public static void WithDefaultValuesFrom(this DbConnectionStringBuilder csb, DbConnectionStringBuilder defaultCsb) {
      foreach (var key in from x in defaultCsb.ConnectionString.Split(';') select x.Split('=').First()) {
        if (csb.ConnectionString.IndexOf(key + "=", StringComparison.Ordinal) < 0) {
          csb[key] = defaultCsb[key];
        }
      }
    }

  }
}