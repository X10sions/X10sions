using Ardalis.SmartEnum;

namespace Common.Data;
public sealed class DbProviderNamespace : SmartEnum<DbProviderNamespace> {

  public static readonly DbProviderNamespace Devart_Data_MySql = new DbProviderNamespace("Devart.Data.MySql", DbSystem.MySql);
  public static readonly DbProviderNamespace Devart_Data_Oracle = new DbProviderNamespace("Devart.Data.Oracle", DbSystem.Oracle);
  public static readonly DbProviderNamespace Devart_Data_PostgreSql = new DbProviderNamespace("Devart.Data.PostgreSql", DbSystem.PostgreSql);
  public static readonly DbProviderNamespace Devart_Data_SQLite = new DbProviderNamespace("Devart.Data.SQLite", DbSystem.Sqlite);
  public static readonly DbProviderNamespace FirebirdSql_Data_FirebirdClient = new DbProviderNamespace("FirebirdSql.Data.FirebirdClient", DbSystem.Firebird);
  public static readonly DbProviderNamespace IBM_Data_DB2 = new DbProviderNamespace("IBM.Data.DB2", DbSystem.DB2);
  public static readonly DbProviderNamespace IBM_Data_DB2_Core = new DbProviderNamespace("IBM.Data.DB2.Core", DbSystem.DB2);
  public static readonly DbProviderNamespace IBM_Data_DB2_iSeries = new DbProviderNamespace("IBM.Data.DB2.iSeries", DbSystem.DB2iSeries);
  public static readonly DbProviderNamespace Microsoft_Data_Sqlite = new DbProviderNamespace("Microsoft.Data.Sqlite", DbSystem.Sqlite);
  public static readonly DbProviderNamespace Microsoft_Data_SqlClient = new DbProviderNamespace("Microsoft.Data.SqlClient", DbSystem.SqlServer);
  public static readonly DbProviderNamespace MongoDB_Driver = new DbProviderNamespace("MongoDB.Driver", DbSystem.MongoDb);
  public static readonly DbProviderNamespace MySql_Data_MySqlClient = new DbProviderNamespace("MySql.Data.MySqlClient", DbSystem.MySql);
  public static readonly DbProviderNamespace MySqlConnector = new DbProviderNamespace("MySqlConnector", DbSystem.MySql);
  public static readonly DbProviderNamespace Oracle_ManagedDataAccess_Client = new DbProviderNamespace("Oracle.ManagedDataAccess.Client", DbSystem.Oracle);
  public static readonly DbProviderNamespace Npgsql = new DbProviderNamespace("Npgsql", DbSystem.PostgreSql);
  public static readonly DbProviderNamespace Redis = new DbProviderNamespace("Redis", DbSystem.Redis);
  public static readonly DbProviderNamespace System_Data_Odbc = new DbProviderNamespace("System.Data.Odbc", null);
  public static readonly DbProviderNamespace System_Data_OleDb = new DbProviderNamespace("System.Data.OleDb", null);
  public static readonly DbProviderNamespace System_Data_OracleClient = new DbProviderNamespace("System.Data.OracleClient", DbSystem.Oracle);
  public static readonly DbProviderNamespace System_Data_SQLite = new DbProviderNamespace("System.Data.SQLite", DbSystem.Sqlite);
  public static readonly DbProviderNamespace System_Data_SqlClien = new DbProviderNamespace("System.Data.SqlClient", DbSystem.SqlServer);

  private DbProviderNamespace(string name, DbSystem? dbSystem) : base(name, List.Count) {
    DbSystem = dbSystem;
  }

  public DbSystem? DbSystem { get; }

}