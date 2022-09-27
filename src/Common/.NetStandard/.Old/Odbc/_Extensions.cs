using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Data.Odbc {
  public static class _Extensions {

    public static IEnumerable<string> ConnectionStringSplit(string connectionString)
      => connectionString.Split(';').Where(x => x.Contains('='));

    public static Dictionary<string, string> ConnectionStringToDictionary(string connectionString)
      => (from x in ConnectionStringSplit(connectionString)
          select x.Split(new char[] { '=' }, 2)).ToDictionary(kvp => kvp[0].Trim(), kvp => kvp[1].Trim(), StringComparer.InvariantCultureIgnoreCase);

    public static IEnumerable<KeyValuePair<string, string>> ConnectionStringToKeyValuePairs(string connectionString)
      => from x in ConnectionStringSplit(connectionString)
         let kvp = x.Split(new char[] { '=' }, 2)
         select new KeyValuePair<string, string>(kvp[0].Trim(), kvp[1].Trim());


    public static void SetConnectionString<T>(this T connectionStringBuilder, string connectionString) {
      var csbType = typeof(T);
      //var type = connectionStringBuilder.GetType();
      var csbTypeName = csbType.Name;
      var csbProps = csbType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var kvp in ConnectionStringToKeyValuePairs(connectionString)) {
        var csProp = csbProps.FirstOrDefault(p => p.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));
        if (csProp == null) {
          throw new Exception($"{csbTypeName} is missing '{kvp.Key}' property, perhaps this property is not supported ");
        }
        csProp.SetValue(connectionStringBuilder, Convert.ChangeType(kvp.Value, csProp.PropertyType), null);
      }

    }

  }
}