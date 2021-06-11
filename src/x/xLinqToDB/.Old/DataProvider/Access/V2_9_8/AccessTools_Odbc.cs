using LinqToDB.Data;
using System.Data;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public static class AccessTools_Odbc {
    static readonly AccessDataProvider_Odbc _accessDataProvider_Odbc = new AccessDataProvider_Odbc();

    static AccessTools_Odbc() {
      DataConnection.AddDataProvider(_accessDataProvider_Odbc);
    }

    public static IDataProvider GetDataProvider() => _accessDataProvider_Odbc;

    #region CreateDataConnection

    public static DataConnection CreateDataConnection(string connectionString) => new DataConnection(_accessDataProvider_Odbc, connectionString);
    public static DataConnection CreateDataConnection(IDbConnection connection) => new DataConnection(_accessDataProvider_Odbc, connection);
    public static DataConnection CreateDataConnection(IDbTransaction transaction) => new DataConnection(_accessDataProvider_Odbc, transaction);

    #endregion

    public static void CreateDatabase(string databaseName, bool deleteIfExists = false) => _accessDataProvider_Odbc.CreateDatabase(databaseName, deleteIfExists);
    public static void DropDatabase(string databaseName) => _accessDataProvider_Odbc.DropDatabase(databaseName);

  }
}