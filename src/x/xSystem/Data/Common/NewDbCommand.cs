namespace System.Data.Common {
  public class NewDbCommand : DbCommand {

    public NewDbCommand() {
      instance = DbConnection.CreateCommand();
    }

    public NewDbCommand(DbConnection connection) {
      Connection = connection;
      instance = connection.CreateCommand();
    }

    private DbCommand instance;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
    public override string CommandText { get => instance.CommandText; set => instance.CommandText = value; }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
    public override int CommandTimeout { get => instance.CommandTimeout; set => instance.CommandTimeout = value; }
    public override CommandType CommandType { get => instance.CommandType; set => instance.CommandType = value; }
    public override bool DesignTimeVisible { get => true; set => throw new NotImplementedException(); }
    protected override DbConnection DbConnection { get => instance.Connection; set => instance.Connection = value; }
    protected override DbParameterCollection DbParameterCollection => throw new NotImplementedException();
    protected override DbTransaction DbTransaction { get => instance.Transaction; set => instance.Transaction = value; }
    public override UpdateRowSource UpdatedRowSource { get => instance.UpdatedRowSource; set => instance.UpdatedRowSource = value; }

    public override void Cancel() => instance.Cancel();
    public override int ExecuteNonQuery() => instance.ExecuteNonQuery();
    public override object ExecuteScalar() => instance.ExecuteScalar();
    public override void Prepare() => instance.Prepare();
    protected override DbParameter CreateDbParameter() => instance.CreateParameter();
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => instance.ExecuteReader();

  }
}

