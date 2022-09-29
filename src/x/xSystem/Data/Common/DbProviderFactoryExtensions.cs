namespace System.Data.Common;
  public static class DbProviderFactoryExtensions {

  public static DbCommand? CreateCommand(this DbProviderFactory factory, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) {
    var command = factory.CreateCommand();
    command.CommandText = commandText;
    command.CommandType = commandType;
    if (parameters != null) {
      command.AddParameters(parameters);
    }
    return command;
  }

  public static DbConnection? CreateConnection(this DbProviderFactory factory, string connectionString) {
    var conn = factory.CreateConnection();
    if (conn != null) {
      conn.ConnectionString = connectionString;
    }
    return conn;
  }

  public static DbConnectionStringBuilder? CreateConnectionStringBuilder(this DbProviderFactory factory, string connectionString) {
    var csb = factory.CreateConnectionStringBuilder();
    if (csb != null) {
      csb.ConnectionString = connectionString;
    }
    return csb;
  }

}