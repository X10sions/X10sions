using System.Data.Common;

namespace Common.Data.GetSchemaTyped {

  public abstract class _BaseGetSchemaTyped<TConnection> : _BaseGetSchemaTyped where TConnection : DbConnection, new() {
    public _BaseGetSchemaTyped(string connectionString) : this(new TConnection { ConnectionString = connectionString }) { }
    public _BaseGetSchemaTyped(TConnection connection) : base(connection) { }
  }

  public abstract class _BaseGetSchemaTyped {
    public _BaseGetSchemaTyped(DbConnection connection) {
      Connection = connection;
    }
    public DbConnection Connection { get; }

  }
}