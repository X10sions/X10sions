using System;

namespace LinqToDB.Tests.Base.Tools {
  public class TempTable<T> : IDisposable {
    public ITable<T> Table { get; }

    public TempTable(IDataContext db, string tableName) {
      try {
        Table = db.CreateTable<T>(tableName);
      } catch {
        db.DropTable<T>(tableName, throwExceptionIfNotExists: false);
        Table = db.CreateTable<T>(tableName);
      }
    }

    public void Dispose() => Table.DropTable();
  }
}