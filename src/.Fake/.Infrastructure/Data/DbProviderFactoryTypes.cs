using FirebirdSql.Data.FirebirdClient;
using IBM.Data.Db2;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SQLite;
//using SqlClientMs = Microsoft.Data.SqlClient;
//using SqlClientSys = System.Data.SqlClient;
//using SqlClientMs = Microsoft.Data.SqlClient;
//using SqlClientSys = System.Data.SqlClient;

namespace X10sions.Fake.Data;

public class DbProviderFactoryTypes : Dictionary<Type, DbProviderFactory> {

  public static Dictionary<Type, DbProviderFactory> Instance = new Dictionary<Type, DbProviderFactory> {
    { typeof(DB2Factory), DB2Factory.Instance},
    { typeof(FirebirdClientFactory), FirebirdClientFactory.Instance},
    { typeof(MySqlClientFactory), MySqlClientFactory.Instance},
    { typeof(MySqlConnectorFactory), MySqlConnectorFactory.Instance},
    { typeof(NpgsqlFactory ), NpgsqlFactory.Instance},
    { typeof(OracleClientFactory), OracleClientFactory.Instance},
    { typeof(OdbcFactory), OdbcFactory.Instance},
    { typeof(OleDbFactory), OleDbFactory.Instance},
    { typeof(Microsoft.Data.SqlClient. SqlClientFactory), Microsoft.Data.SqlClient.SqlClientFactory.Instance},
    { typeof(System.Data.SqlClient.SqlClientFactory), System.Data.SqlClient.SqlClientFactory.Instance},
    { typeof(SqliteFactory), SqliteFactory.Instance},
    { typeof(SQLiteFactory), SQLiteFactory.Instance},
  };
}


//public class ConnectionStrings {



//  public ConnectionStrings(IConfiguration configuration) => this.configuration = configuration;
//  IConfiguration configuration;

//  public string Access_Odbc => configuration.GetConnectionString(nameof(Access_Odbc));
//  public string Access_Oledb => configuration.GetConnectionString(nameof(Access_Oledb));
//  public string DB2_IBM => configuration.GetConnectionString(nameof(DB2_IBM));
//  public string DB2_Odbc => configuration.GetConnectionString(nameof(DB2_Odbc));
//  public string DB2iSeries_IBM => configuration.GetConnectionString(nameof(DB2iSeries_IBM));
//  public string DB2iSeries_Odbc => configuration.GetConnectionString(nameof(DB2iSeries_Odbc));
//  public string DB2iSeries_OleDb => configuration.GetConnectionString(nameof(DB2iSeries_OleDb));
//  public string MariaDb => configuration.GetConnectionString(nameof(MariaDb));
//  public string MySql => configuration.GetConnectionString(nameof(MySql));
//  public string PostgreSql => configuration.GetConnectionString(nameof(PostgreSql));
//  public string Oracle => configuration.GetConnectionString(nameof(Oracle));
//  public string SqlServer => configuration.GetConnectionString(nameof(SqlServer));
//  public string Sqlite => configuration.GetConnectionString(nameof(Sqlite));
//}


