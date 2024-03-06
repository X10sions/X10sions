
using System.Data.Common;

namespace System.Data;
public static class IDbConnectionExtensions {
  public static string ConnectionStringWithoutPassword(this IDbConnection connection) => GetDbConnectionStringBuilder(connection).RemovePasswordKeywords().ConnectionString;
  public static string ConnectionStringWithoutPasswordOrUser(this IDbConnection connection) => GetDbConnectionStringBuilder(connection).RemovePasswordKeywords().RemoveUserKeywords().ConnectionString;
  public static string ConnectionStringWithoutUser(this IDbConnection connection) => GetDbConnectionStringBuilder(connection).RemoveUserKeywords().ConnectionString;

  public static IDbCommand CreateCommand(this IDbConnection cn, string commandText, params KeyValuePair<string, object>[] parameters) {
    using (var cmd = cn.CreateCommand(commandText)) {
      cmd.AddParameters(parameters);
      return cmd;
    }
  }
  public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text) {
    var command = connection.CreateCommand();
    command.CommandText = commandText;
    command.CommandType = commandType;
    if (parameters != null) {
      command.AddParameters(parameters);
    }
    return command;
  }
  //public static IDbDataAdapter? CreateDataAdapter(this IDbConnection connection) => (connection is DbConnection dbConn) ? dbConn.CreateDataAdapter() : null;

  //public static IDbDataAdapter? CreateDataAdapter(this IDbConnection connection, IDbCommand selectCommand) {
  //  var dataAdapter = connection.CreateDataAdapter();
  //  if (dataAdapter is not null) {
  //    dataAdapter.SelectCommand = selectCommand;
  //  }
  //  return dataAdapter;
  //}

  public static DbConnectionStringBuilder GetDbConnectionStringBuilder(this IDbConnection connection) => new DbConnectionStringBuilder(connection.ConnectionString.Contains("Driver=", StringComparison.OrdinalIgnoreCase)) { ConnectionString = connection.ConnectionString };

  public static object GetConnectionStringValue(this IDbConnection connection, string connectionStringKey) => connection.GetDbConnectionStringBuilder()[connectionStringKey];

  public static T? GetConnectionStringValue<T>(this IDbConnection connection, string connectionStringKey, T? defaultValue = default) => connection.GetConnectionStringValue(connectionStringKey, defaultValue);

  public static T EnsureNotNull<T>(this T connection, Func<T> connectionFactory
    , Action? beforeNullConnectionAction = null, Action? afterNullConnectionAction = null) where T : IDbConnection, IDisposable {
    if (connection == null) {
      beforeNullConnectionAction?.Invoke();
      connection = connectionFactory();
      afterNullConnectionAction?.Invoke();
    }
    return connection;
  }

  public static T EnsureNotNullAndOpen<T>(this T dbConnection, Func<T> connectionFactory
    , Action? beforeNullConnectionAction = null, Action? afterNullConnectionAction = null
    , Action? beforeOpenConnectionAction = null, Action? afterOpenConnectionAction = null
    ) where T : IDbConnection
    => dbConnection
    .EnsureNotNull(connectionFactory, beforeNullConnectionAction, afterNullConnectionAction)
    .EnsureOpen(beforeOpenConnectionAction, afterOpenConnectionAction);

  public static T EnsureOpen<T>(this T connection, Action? beforeOpenConnectionAction = null, Action? afterOpenConnectionAction = null) where T : IDbConnection, IDisposable {
    if (connection.State != ConnectionState.Open) {
      beforeOpenConnectionAction?.Invoke();
      connection.Open();
      afterOpenConnectionAction?.Invoke();
    }
    return connection;
  }

  public static T EnsureClosed<T>(this T connection) where T : IDbConnection {
    if (connection.State == ConnectionState.Open) {
      connection.Close();
    }
    return connection;
  }

  public static T EnsureOpenCall<T>(this IDbConnection connection, Func<T> action) {
    var isConnectionNotOpen = connection.State != ConnectionState.Open;
    if (isConnectionNotOpen) { connection.Open(); }
    T returValue;
    try {
      returValue = action();
    } catch (Exception ex) {
      //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
      throw;
    }
    if (isConnectionNotOpen) { connection.Close(); }
    return returValue;
  }

  public static async Task<T> EnsureOpenCallAsync<T>(this IDbConnection connection, Func<Task<T>> action) {
    var isConnectionNotOpen = connection.State != ConnectionState.Open;
    if (isConnectionNotOpen) { await connection.OpenAsync(); }
    T returValue;
    try {
      returValue = await action();
    } catch (Exception ex) {
      //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
      throw;
    }
    if (isConnectionNotOpen) { await connection.CloseAsync(); }
    return returValue;
  }

  public static int ExecuteNonQuery(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => conn.EnsureOpenCall(() => conn.CreateCommand(commandText, parameters, commandType).ExecuteNonQuery());

  public static async Task<int> ExecuteNonQueryAsync(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => await conn.EnsureOpenCallAsync(() => conn.CreateCommand(commandText, parameters, commandType).ExecuteNonQueryAsync());


  //public static int ExecuteNonQuery(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[] parameters = null) {
  //  var isConnectionNotOpen = connection.State != ConnectionState.Open;
  //  if (isConnectionNotOpen) { connection.Open(); }
  //  int returValue;
  //  using (var command = connection.CreateCommand(commandText, commandType, parameters)) {
  //    try {
  //      returValue = command.ExecuteNonQuery();
  //    } catch (Exception ex) {
  //      //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
  //      throw ex;
  //    }
  //  }
  //  if (isConnectionNotOpen) { connection.Close(); }
  //  return returValue;
  //}

  public static IDataReader ExecuteReader(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => conn.EnsureOpenCall(() => conn.CreateCommand(commandText, parameters, commandType)).ExecuteReader();

  public static string ExecuteToCsv(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => conn.ExecuteReader(commandText, commandType, parameters).ToCsv();

  public static string ExecuteToHtmlTable(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null, string tableCssClass = "table")
    => conn.ExecuteReader(commandText, commandType, parameters).ToHtmlTable(tableCssClass);

  public static async Task<IDataReader> ExecuteReaderAsync(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => await conn.EnsureOpenCallAsync(() => conn.CreateCommand(commandText, parameters, commandType).ExecuteReaderAsync());

  //  public static IDataReader ExecuteReader(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[] parameters = null) {
  //  var isConnectionNotOpen = connection.State != ConnectionState.Open;
  //  if (isConnectionNotOpen) { connection.Open(); }
  //  IDataReader returValue;
  //  using (var command = connection.CreateCommand(commandText, commandType, parameters)) {
  //    try {
  //      returValue = command.ExecuteReader();
  //    } catch (Exception ex) {
  //      //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
  //      throw ex;
  //    }
  //  }
  //  if (isConnectionNotOpen) { connection.Close(); }
  //  return returValue;
  //}

  public static T ExecuteScalar<T>(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => conn.EnsureOpenCall(() => (T)conn.CreateCommand(commandText, parameters, commandType).ExecuteScalar());

  public static async Task<object> ExecuteScalarAsync(this IDbConnection conn, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null)
    => await conn.EnsureOpenCallAsync(() => conn.CreateCommand(commandText, parameters, commandType).ExecuteScalarAsync());

  //public static object ExecuteScalar(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[] parameters = null) {
  //  var isConnectionNotOpen = connection.State != ConnectionState.Open;
  //  if (isConnectionNotOpen) { connection.Open(); }
  //  object returValue;
  //  using (var command = connection.CreateCommand(commandText, commandType, parameters)) {
  //    try {
  //      returValue = command.ExecuteScalar();
  //    } catch (Exception ex) {
  //      //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
  //      throw ex;
  //    }
  //  }
  //  if (isConnectionNotOpen) { connection.Close(); }
  //  return returValue;
  //}

  public static bool IsOdbc(this IDbConnection connection) => connection.ConnectionString.Contains("Driver=", StringComparison.OrdinalIgnoreCase);
  public static bool IsOleDb(this IDbConnection connection) => connection.ConnectionString.Contains("Provider=", StringComparison.OrdinalIgnoreCase);

  public static DataTable LoadDataTable(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null) {
    var isConnectionNotOpen = connection.State != ConnectionState.Open;
    if (isConnectionNotOpen) { connection.Open(); }
    var dt = new DataTable();
    using (var cmd = connection.CreateCommand(commandText, parameters, commandType)) {
      try {
        dt.Load(cmd.ExecuteReader());
      } catch (Exception ex) {
        throw new Exception($"{ex.Message}{Environment.NewLine}{commandText}", ex);
      }
    }
    if (isConnectionNotOpen) { connection.Close(); }
    return dt;
  }

  public static Task OpenAsync(this IDbConnection connection) => connection.OpenAsync(CancellationToken.None);

  public static Task OpenAsync(this IDbConnection connection, CancellationToken cancellationToken) {
    if (cancellationToken.IsCancellationRequested) {
      return Task.FromCanceled(cancellationToken);
    }
    try {
      connection.Open();
      return Task.CompletedTask;
    } catch (Exception e) {
      return Task.FromException(e);
    }
  }

  public static Task CloseAsync(this IDbConnection connection) => connection.CloseAsync(CancellationToken.None);

  public static Task CloseAsync(this IDbConnection connection, CancellationToken cancellationToken) {
    if (cancellationToken.IsCancellationRequested) {
      return Task.FromCanceled(cancellationToken);
    }
    try {
      connection.Close();
      return Task.CompletedTask;
    } catch (Exception e) {
      return Task.FromException(e);
    }
  }


  //static DbConnection GetConnection(string connStr) {
  //  string providerName = null;
  //  var csb = new DbConnectionStringBuilder { ConnectionString = connStr };
  //  if (csb.ContainsKey("provider")) {
  //    providerName = csb["provider"].ToString();
  //  } else {
  //    var css = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().FirstOrDefault(x => x.ConnectionString == connStr);
  //    if (css != null) providerName = css.ProviderName;
  //  }
  //  if (providerName != null) {
  //    var providerExists = DbProviderFactories
  //      .GetFactoryClasses()
  //      .Rows.Cast<DataRow>()
  //      .Any(r => r[2].Equals(providerName));
  //    if (providerExists) {
  //      var factory = DbProviderFactories.GetFactory(providerName);
  //      var dbConnection = factory.CreateConnection();
  //      dbConnection.ConnectionString = connStr;
  //      return dbConnection;
  //    }
  //  }
  //  return null;
  //}

  #region AS400

  public static int ExecuteClCommand(this IDbConnection dbConnection, string clCommand) {
    var commandText = ConvertClCommandToSql(clCommand);
    return dbConnection.ExecuteNonQuery(commandText);
  }

  public static string ConvertClCommandToSql(string clCommand) {
    //var cmdLength = clCommand.Length.ToString().PadLeft(10, '0') + ".00000";
    var cmdLength = clCommand.Trim().Length.ToString("0000000000.00000");
    var cmdEscaped = clCommand.Replace("'", "''").Trim();
    return $"CALL QCMDEXC('{cmdEscaped}',{cmdLength})";
  }

  #endregion

}