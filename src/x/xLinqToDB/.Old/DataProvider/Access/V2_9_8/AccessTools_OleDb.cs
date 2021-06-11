using LinqToDB.Data;
using System.Data;

namespace LinqToDB.DataProvider.Access.V_2_9_8 {
  public static class AccessTools_OleDb {
    static readonly AccessDataProvider_OleDb _accessDataProvider_OleDb = new AccessDataProvider_OleDb();

    static AccessTools_OleDb() {
      DataConnection.AddDataProvider(_accessDataProvider_OleDb);
    }

    public static IDataProvider GetDataProvider() => _accessDataProvider_OleDb;

    #region CreateDataConnection

    public static DataConnection CreateDataConnection(string connectionString) => new DataConnection(_accessDataProvider_OleDb, connectionString);
    public static DataConnection CreateDataConnection(IDbConnection connection) => new DataConnection(_accessDataProvider_OleDb, connection);
    public static DataConnection CreateDataConnection(IDbTransaction transaction) => new DataConnection(_accessDataProvider_OleDb, transaction);

    #endregion

    public static void CreateDatabase(string databaseName, bool deleteIfExists = false) => _accessDataProvider_OleDb.CreateDatabase(databaseName, deleteIfExists);
    public static void DropDatabase(string databaseName) => _accessDataProvider_OleDb.DropDatabase(databaseName);

  }
}