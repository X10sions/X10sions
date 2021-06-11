using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Common.Data {

  public class DbConnectionProvider : IEquatable<DbConnectionProvider> {
    DbConnectionProvider(string name, Func<string, DbConnection> dbConnectionGetter, Type dbConnectionType) {
      Name = name;
      //Namespace = dbConnectionType.Namespace;
      DbConnectionGetter = dbConnectionGetter;
      DbConnectionType = dbConnectionType;
      List.Add(this);
    }
    public string Name { get; }
    //public string Namespace { get; }
    public Func<string, DbConnection> DbConnectionGetter { get; }
    public Type DbConnectionType { get; }

    DbConnection GetDbConnection(string connectionString) => DbConnectionGetter(connectionString);

    public static DbConnectionProvider Add<TDbConnection>(string name, Func<string, TDbConnection> dbConnectionGetter) where TDbConnection : DbConnection
      => new DbConnectionProvider(name, dbConnectionGetter, typeof(TDbConnection));

    public bool Equals(DbConnectionProvider? other) => other != null && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is DbConnectionProvider && Equals(obj as DbConnectionProvider);
    public override int GetHashCode() => HashCode.Combine(Name);

    public static HashSet<DbConnectionProvider> List = new HashSet<DbConnectionProvider>();

    //public static DbConnectionProvider IBM_Data_Db2 { get; } = Add("IBM.Data.Db2", connectionString => new IBM.Data.DB2.DB2Connection(connectionString));
    //public static DbConnectionProvider IBM_Data_DB2_Core { get; } = Add("IBM.Data.DB2.Core", connectionString => new IBM.Data.DB2.Core.DB2Connection(connectionString));
    //public static DbConnectionProvider IBM_Data_DB2_iSeries { get; } = Add("IBM.Data.DB2.iSeries", connectionString => new IBM.Data.DB2.iSeries.iDB2Connection(connectionString));
    //public static DbConnectionProvider Microsoft_Data_SqlClient { get; } = Add("Microsoft.Data.SqlClient", connectionString => new Microsoft.Data.SqlClient.SqlConnection(connectionString));
    //public static DbConnectionProvider Microsoft_Data_Sqlite { get; } = Add("Microsoft.Data.Sqlite", connectionString => new Microsoft.Data.Sqlite.SqliteConnection(connectionString));
    //public static DbConnectionProvider MySql_Data_MySqlClient { get; } = Add("MySql.Data.MySqlClient", connectionString => new MySql.Data.MySqlClient.MySqlConnection(connectionString));
    //public static DbConnectionProvider MySqlConnector { get; } = Add("MySqlConnector", connectionString => new MySqlConnector.MySqlConnection(connectionString));
    //public static DbConnectionProvider Npgsql { get; } = Add("Npgsql", connectionString => new Npgsql.NpgsqlConnection(connectionString));
    //public static DbConnectionProvider Oracle_ManagedDataAccess_Client { get; } = Add("Oracle.ManagedDataAccess.Client", connectionString => new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString));
    //public static DbConnectionProvider System_Data_Odbc { get; } = Add("System.Data.Odbc", cs => new System.Data.Odbc.OdbcConnection(cs));
    //public static DbConnectionProvider System_Data_OleDb { get; } = Add("System.Data.OleDb", cs => new System.Data.OleDb.OleDbConnection(cs));
    //public static DbConnectionProvider System_Data_SqlClient { get; } = Add("System.Data.SqlClient", connectionString => new System.Data.SqlClient.SqlConnection(connectionString));
    //public static DbConnectionProvider System_Data_SQLite { get; } = Add("System.Data.SQLite", connectionString => new System.Data.SQLite.SQLiteConnection(connectionString));

    public static DbConnectionProvider? GetDbConnectionProvider(string providerName) => List.FirstOrDefault(x => x.Name.Equals(providerName, StringComparison.OrdinalIgnoreCase));
    public static DbConnection? GetDbConnection(string providerName, string connectionString) => GetDbConnectionProvider(providerName)?.GetDbConnection(connectionString) ?? null;

  }
}