using System.Data.Common;

namespace Common.Data;

public static partial class IDbConnectionStringExtensions {

  public static DbSystem? GetDbSystemOption(this IDbConnectionString dbConnectionString) => dbConnectionString.ProviderName switch {
    "System.Data.Odbc" => dbConnectionString.GetDbSystemOptionOdbc(),
    "System.Data.OleDb" => dbConnectionString.GetDbSystemOptionOleDb(),
    _ => DbProviderNamespace.FromName(dbConnectionString.ProviderName).DbSystem
  };

  public static DbConnectionStringBuilder GetDbConnectionStringBuilder(this string connectionString, bool useOdbcRules = false)
    => new DbConnectionStringBuilder(useOdbcRules) { ConnectionString = connectionString };

  public static object? DbConnectionStringBuilderProperty(this string connectionString, string key, bool useOdbcRules = false)
    => GetDbConnectionStringBuilder(connectionString, useOdbcRules)[key];

  public static string? GetOdbcDriver(this IDbConnectionString dbConnectionString)
    => DbConnectionStringBuilderProperty(dbConnectionString.Value, "Driver", true)?.ToString();

  public static string? GetOleDbProvider(this IDbConnectionString dbConnectionString)
    => DbConnectionStringBuilderProperty(dbConnectionString.Value, "Provider", false)?.ToString();

  public static DbSystem? GetDbSystemOptionOdbc(this IDbConnectionString dbConnectionString) => OdbcDriver.FromName(dbConnectionString.GetOdbcDriver()).DbSystem;

  public static DbSystem? GetDbSystemOptionOleDb(this IDbConnectionString dbConnectionString) => OleDbProvider.FromName(dbConnectionString.GetOleDbProvider()).DbSystem;

  public static DbProviderNamespace? GetDbProviderNamespace(this IDbConnectionString dbConnectionString) => DbProviderNamespace.FromName(dbConnectionString.ProviderName);

}