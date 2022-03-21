using System.Data.Common;

namespace Common.Data;

public class DbProvider {
  public string? DefaultConnectionString { get; set; }
  public Dictionary<string, DbProviderKeyword> Keywords { get; set; } = new Dictionary<string, DbProviderKeyword>();

  public string ReplaceKeyWords(string connectionString) {
    try {
      var useOdbcRules = connectionString?.IndexOf("driver=", StringComparison.OrdinalIgnoreCase) >= 0;
      var csb = new DbConnectionStringBuilder(useOdbcRules) { ConnectionString = connectionString };
      foreach (var kw in Keywords) {
        if (csb.ContainsKey(kw.Key)) {
          var value = csb[kw.Key].ToString();
          if (kw.Value.Values.ContainsKey(value)) {
            value = kw.Value.Values[value];
          }
          csb.Remove(kw.Key);
          csb.Add(kw.Value.ReplaceWith, value);
        }
      }
      return csb.ConnectionString;//.Replace("\"", string.Empty);
    } catch {
      return connectionString;
    }
  }
}