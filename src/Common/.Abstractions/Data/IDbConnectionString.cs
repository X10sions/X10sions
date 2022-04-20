using System.Data.Common;

namespace Common.Data;

public interface IHaveDbConnectionStringList {
  List<IDbConnectionString> DbConnectionStrings { get; }
}
public interface IHaveDbConnectionStrings {
  IDictionary<string, IDbConnectionString> DbConnectionStrings { get; }
}

public interface IDbConnectionString {
  // https://github.com/serenity-is/Serenity/blob/master/src/Serenity.Net.Data/Connections/IConnectionString.cs

  //ISqlDialect Dialect { get; }
  string? Key { get; }
  string? ProviderName { get; }
  string Value { get; }
}

public static class IDbConnectionStringExtensions {

  public static DbSystemOption? GetDbSystemOption(this IDbConnectionString dbConnectionString) => dbConnectionString.ProviderName switch {
    "Devart.Data.MySql" => DbSystemOption.MySql,
    "Devart.Data.Oracle" => DbSystemOption.Oracle,
    "Devart.Data.PostgreSql" => DbSystemOption.PostgreSql,
    "Devart.Data.SQLite" => DbSystemOption.Sqlite,
    "FirebirdSql.Data.FirebirdClient" => DbSystemOption.Firebird,
    "IBM.Data.DB2" => DbSystemOption.DB2,
    "IBM.Data.DB2.Core" => DbSystemOption.DB2,
    "IBM.Data.DB2.iSeries" => DbSystemOption.DB2iSeries,
    "Microsoft.Data.Sqlite" => DbSystemOption.Sqlite,
    "Microsoft.Data.SqlClient" => DbSystemOption.SqlServer,
    "MongoDB.Driver" => DbSystemOption.MongoDb,
    "MySql.Data.MySqlClient" => DbSystemOption.DB2,
    "MySqlConnector" => DbSystemOption.MySql,
    "Oracle.ManagedDataAccess.Client" => DbSystemOption.Oracle,
    "Npgsql" => DbSystemOption.PostgreSql,
    "Redis" => DbSystemOption.Redis,
    "System.Data.Odbc" => dbConnectionString.GetDbSystemOptionOdbc(),
    "System.Data.OleDb" => dbConnectionString.GetDbSystemOptionOleDb(),
    "System.Data.OracleClient" => DbSystemOption.Oracle,
    "System.Data.SQLite" => DbSystemOption.SqlServer,
    "System.Data.SqlClient" => DbSystemOption.Sqlite,
    _ => null
  };

  public static DbConnectionStringBuilder GetDbConnectionStringBuilder(this string connectionString, bool useOdbcRules = false)
    => new DbConnectionStringBuilder(useOdbcRules) { ConnectionString = connectionString };

  public static object? DbConnectionStringBuilderProperty(this string connectionString, string key, bool useOdbcRules = false)
    => GetDbConnectionStringBuilder(connectionString, useOdbcRules)[key];

  public static string? GetOdbcDriver(this IDbConnectionString dbConnectionString)
    => DbConnectionStringBuilderProperty(dbConnectionString.Value, "Driver", true)?.ToString();

  public static string? GetOleDbProvider(this IDbConnectionString dbConnectionString)
    => DbConnectionStringBuilderProperty(dbConnectionString.Value, "Provider", false)?.ToString();

  public static DbSystemOption? GetDbSystemOptionOdbc(this IDbConnectionString dbConnectionString) => dbConnectionString.GetOdbcDriver() switch {
    "Firebird/InterBase(r) driver" => DbSystemOption.Firebird,
    "{IBM DB2 ODBC DRIVER}" => DbSystemOption.DB2,
    // https://www.ibm.com/docs/en/i/7.1?topic=keywords-connection-string-general-properties
    "{IBM i Access ODBC Driver};MGDSN=0" => DbSystemOption.DB2iSeries,
    "{Microsoft Access Driver (*.mdb, *.accdb)}" => DbSystemOption.MicrosoftAccess,
    "{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}" => DbSystemOption.MicrosoftExcel,
    "{MySQL ODBC 8.0 ANSI Driver}" => DbSystemOption.MySql,
    "{MySQL ODBC 8.0 Unicode Driver}" => DbSystemOption.MySql,
    "{Microsoft ODBC for Oracle}" => DbSystemOption.Oracle,
    "{Oracle in OraDB18Home1}" => DbSystemOption.Oracle,
    "{PostgreSQL}" => DbSystemOption.PostgreSql,
    "{ODBC Driver 17 for SQL Server}" => DbSystemOption.SqlServer,
    _ => null
  };

  public static DbSystemOption? GetDbSystemOptionOleDb(this IDbConnectionString dbConnectionString) => dbConnectionString.GetOleDbProvider() switch {
    "DB2OLEDB" => DbSystemOption.DB2,
    "IBMDADB2" => DbSystemOption.DB2,
    // https://www.ibm.com/support/pages/access-client-solutions-ole-db-custom-connection-properties
    "IBMDA400" => DbSystemOption.DB2iSeries,
    //"Microsoft.ACE.OLEDB.12.0" => DbSystemOption.Access,    Excel,
    "MySQLProv" => DbSystemOption.MySql,
    "MSDAORA" => DbSystemOption.Oracle,
    "OraOLEDB.Oracle" => DbSystemOption.Oracle,
    "PostgreSQL OLE DB Provider" => DbSystemOption.PostgreSql,
    "MSOLEDBSQL" => DbSystemOption.SqlServer,
    _ => null
  };
  public static DbProviderOption? GetDbProviderOption(this IDbConnectionString dbConnectionString) => dbConnectionString.ProviderName switch {
    "Devart.Data.MySql" => DbProviderOption.Devart_Data_MySql,
    "Devart.Data.Oracle" => DbProviderOption.Devart_Data_Oracle,
    "Devart.Data.PostgreSql" => DbProviderOption.Devart_Data_PostgreSql,
    "Devart.Data.SQLite" => DbProviderOption.Devart_Data_SQLite,
    "FirebirdSql.Data.FirebirdClient" => DbProviderOption.FirebirdSql_Data_FirebirdClient,
    "IBM.Data.DB2" => DbProviderOption.IBM_Data_DB2,
    "IBM.Data.DB2.Core" => DbProviderOption.IBM_Data_DB2_Core,
    "IBM.Data.DB2.iSeries" => DbProviderOption.IBM_Data_DB2_iSeries,
    "Microsoft.Data.Sqlite" => DbProviderOption.Microsoft_Data_Sqlite,
    "Microsoft.Data.SqlClient" => DbProviderOption.Microsoft_Data_SqlClient,
    "MongoDB.Driver" => DbProviderOption.MongoDB_Driver,
    "MySql.Data.MySqlClient" => DbProviderOption.MySql_Data_MySqlClient,
    "MySqlConnector" => DbProviderOption.MySqlConnector,
    "Oracle.ManagedDataAccess.Client" => DbProviderOption.Oracle_ManagedDataAccess_Client,
    "Npgsql" => DbProviderOption.Npgsql,
    "Redis" => DbProviderOption.Redis,
    "System.Data.Odbc" => DbProviderOption.System_Data_Odbc,
    "System.Data.OleDb" => DbProviderOption.System_Data_OleDb,
    "System.Data.OracleClient" => DbProviderOption.System_Data_OracleClient,
    "System.Data.SQLite" => DbProviderOption.System_Data_SQLite,
    "System.Data.SqlClient" => DbProviderOption.System_Data_SQLite,
    _ => null
  };

}