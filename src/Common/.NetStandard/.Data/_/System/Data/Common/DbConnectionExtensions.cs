using Common.Data.GetSchemaTyped;
using Common.Data.GetSchemaTyped.DataRows;

namespace System.Data.Common;
public static class DbConnectionExtensions {

  public static Version? GetVersion(this DbConnection conn) {
    Version? version = null;
    var isNotOpen = conn.State != ConnectionState.Open;
    try {
      if (isNotOpen) { conn.Open(); }
      Version.TryParse(conn.ServerVersion?.Split(' ')[0], out version);
      if (version == null) {
        version = new DataSourceInformationRow(conn.GetSchema(DbMetaDataCollectionNames.DataSourceInformation).Rows[0]).Version;
      }
      if (isNotOpen) { conn.Close(); }
    } catch {
    }
    return version;
  }

  public static Version? GetVersion<TConnection>(string connectionString) where TConnection : DbConnection, new() {
    Version? version = null;
    using (var conn = new TConnection { ConnectionString = connectionString }) {
      version = conn.GetVersion();
    }
    return version;
  }

  //public static Version? GetVersion(IDbConnection conn) => (conn is DbConnection) ? GetVersion(conn as DbConnection) : null;

  public static DataTable GetSchemaDataTable(this DbConnection dbConnection, GetSchemaCollectionNames collectionName, string[]? restrictionValues = null) => dbConnection.GetSchemaDataTable(collectionName.ToString(), restrictionValues);

  public static GetSchemaHelper GetSchemaHelper(this DbConnection dbConnection) => new GetSchemaHelper(dbConnection);
  public static GetSchemaTypedCollection GetSchemaTypedCollection(this DbConnection dbConnection, string collectionName, string[] restrictionValues) => new GetSchemaTypedCollection(dbConnection, collectionName, restrictionValues);

}
