using System.Data.Common;

namespace Common.Data.GetSchemaTyped {

  public abstract class _GetSchemaTypedBase<TConnection> : _GetSchemaTypedBase where TConnection : DbConnection, new() {
    public _GetSchemaTypedBase(string connectionString) : this(new TConnection { ConnectionString = connectionString }) { }
    public _GetSchemaTypedBase(TConnection connection) : base(connection) { }
  }

  public abstract class _GetSchemaTypedBase {
    public _GetSchemaTypedBase(DbConnection connection) {
      Connection = connection;
    }
    public DbConnection Connection { get; }

  }
}