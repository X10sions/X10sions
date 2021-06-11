using LinqToDB.DataProvider.Firebird;

namespace LinqToDB.Tests.Base {
  public static partial class TestUtils {
    public class FirebirdTempTable<T> : TempTable<T> {
      public FirebirdTempTable(IDataContext db, string? tableName = null, string? databaseName = null, string? schemaName = null)
        : base(db, tableName, databaseName, schemaName) {
      }

      public override void Dispose() {
        DataContext.Close();
        FirebirdTools.ClearAllPools();
        base.Dispose();
      }
    }




  }
}