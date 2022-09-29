namespace Common.Data;

public interface IDatabase { }

public static class IDatabaseExtensions {
  public static IDatabaseTable<T, TDatabase> GetDatabaseTable<T, TDatabase>(this TDatabase db) where T : class where TDatabase : IDatabase => new DatabaseTable<T, TDatabase>(db);
}

public interface IDatabaseTable<T> where T : class {
  IDatabase Database { get; }
}

public interface IDatabaseTable<T, TDatabase> where T : class where TDatabase : IDatabase {
  TDatabase Database { get; }
}

//public interface IDatabaseTable<T, TDatabase, TTable> : IDatabaseTable<T> where T : class {
//  TTable GetTable(TDatabase db);
//}

public class DatabaseTable<T> : IDatabaseTable<T> where T : class {
  public DatabaseTable(IDatabase database) {
    Database = database;
  }
  public IDatabase Database { get; }
}

public class DatabaseTable<T, TDatabase> : IDatabaseTable<T, TDatabase> where T : class where TDatabase : IDatabase {
  public DatabaseTable(TDatabase database) {
    Database = database;
  }
  public TDatabase Database { get; }
}