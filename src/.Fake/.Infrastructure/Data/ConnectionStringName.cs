using FirebirdSql.Data.FirebirdClient;
using IBM.Data.Db2;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SQLite;

namespace X10sions.Fake.Data;

public enum ConnectionStringName {
  Access_Odbc,
  Access_OleDb,
  DB2_IBM,
  DB2_Odbc,
  DB2_OleDb,
  DB2iSeries_IBM,
  DB2iSeries_Odbc,
  DB2iSeries_OleDb,
  Firebird,
  MariaDb,
  MySql_Client,
  MySql_Connector,
  PostgreSql,
  Oracle,
  Sqlite_Microsoft,
  Sqlite_System,
  SqlServer_Microsoft,
  SqlServer_System,
  SqlServer_Odbc,
  SqlServer_OleDb
}

public static class ConnectionStringNamesExtensions {
  public static DbProviderFactory GetDbProviderFactory(this ConnectionStringName name) => name switch {
    ConnectionStringName.Access_Odbc => OdbcFactory.Instance,
    ConnectionStringName.Access_OleDb => OleDbFactory.Instance,
    ConnectionStringName.DB2_IBM => DB2Factory.Instance,
    ConnectionStringName.DB2_Odbc => OdbcFactory.Instance,
    ConnectionStringName.DB2_OleDb => OleDbFactory.Instance,
    //ConnectionStringName.DB2iSeries_IBM=> IBM.Data.DB2.iSeries.iDB2Factory.Instance,
    ConnectionStringName.DB2iSeries_Odbc => OdbcFactory.Instance,
    ConnectionStringName.DB2iSeries_OleDb => OleDbFactory.Instance,
    ConnectionStringName.Firebird => FirebirdClientFactory.Instance,
    ConnectionStringName.MariaDb => MySqlConnectorFactory.Instance,
    ConnectionStringName.MySql_Client => MySqlClientFactory.Instance,
    ConnectionStringName.MySql_Connector => MySqlConnectorFactory.Instance,
    ConnectionStringName.PostgreSql => NpgsqlFactory.Instance,
    ConnectionStringName.Oracle => OracleClientFactory.Instance,
    ConnectionStringName.Sqlite_Microsoft => SqliteFactory.Instance,
    ConnectionStringName.Sqlite_System => SQLiteFactory.Instance,
    ConnectionStringName.SqlServer_Microsoft => Microsoft.Data.SqlClient.SqlClientFactory.Instance,
    ConnectionStringName.SqlServer_System => System.Data.SqlClient.SqlClientFactory.Instance,
    ConnectionStringName.SqlServer_Odbc => OdbcFactory.Instance,
    ConnectionStringName.SqlServer_OleDb => OleDbFactory.Instance,
    _ => throw new NotImplementedException(name.ToString())
  };

  public static string GetConnectionString(this IConfiguration configuration, ConnectionStringName name) => configuration.GetConnectionString(name.ToString());
  public static string GetConnectionString(this ConnectionStringName name, IConfiguration configuration) => configuration.GetConnectionString(name.ToString());

  public static DbConnection? GetDbConnection(this ConnectionStringName name, IConfiguration configuration) {
    var conn = name.GetDbProviderFactory().CreateConnection();
    if (conn != null) {
      conn.ConnectionString = name.GetConnectionString(configuration);
    }    
    return conn;
  }

}