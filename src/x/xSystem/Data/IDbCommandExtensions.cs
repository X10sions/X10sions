using System.Data.Common;

namespace System.Data;
public static class IDbCommandExtensions {

  public static IDbCommand AddParameters(this IDbCommand cmd, params IDbDataParameter[] parameters) {
    cmd.Parameters.AddRange(parameters);
    return cmd;
  }

  public static IDbCommand AddParameters(this IDbCommand cmd, params KeyValuePair<string, object>[] parameters) {
    foreach (var dacParam in parameters) {
      var parameter = cmd.CreateParameter();
      parameter.ParameterName = dacParam.Key;
      parameter.Value = dacParam.Value;
      cmd.Parameters.Add(parameter);
    }
    return cmd;
  }

  public static IDbDataParameter CloneParameter(this IDbCommand command, IDbDataParameter parameter) {
    var clone = command.CreateParameter();
    clone.DbType = parameter.DbType;
    clone.Direction = parameter.Direction;
    clone.ParameterName = parameter.ParameterName;
    clone.Precision = parameter.Precision;
    clone.Scale = parameter.Scale;
    clone.Size = parameter.Size;
    clone.SourceColumn = parameter.SourceColumn;
    clone.SourceVersion = parameter.SourceVersion;
    clone.Value = parameter.Value;
    return clone;
  }

  #region Convert / ReWrite NamedParameters

  public static IDbCommand ReplaceCommndTextAndParameters(this IDbCommand command, string commandText, List<IDbDataParameter> parameters) {
    command.CommandText = commandText;
    command.Parameters.ReplaceAll(parameters);
    return command;
  }

  #endregion

  public static string ToSqlString(this IDbCommand cmd, bool doIncludeParameters = false) {
    var sql = cmd.CommandText;
    var cmdParam = new Text.StringBuilder();
    foreach (IDbDataParameter p in cmd.Parameters) {
      var val = p.DbType.GetSqlQualifiedValue(p.Value, true);
      sql = sql.Replace(p.ParameterName, val);
      cmdParam.AppendLine($"-- {p.ParameterName}({p.DbType}): {val}");
    }
    return sql + ";" + (doIncludeParameters ? $"-- Parameters {Environment.NewLine}/*" + cmdParam + "*/" : string.Empty);
  }

  #region "async"
  public static async Task<int> ExecuteNonQueryAsync(this IDbCommand cmd) => await ((DbCommand)cmd).ExecuteNonQueryAsync();
  public static async Task<int> ExecuteNonQueryAsync(this IDbCommand cmd, CancellationToken cancellationToken) => await ((DbCommand)cmd).ExecuteNonQueryAsync(cancellationToken);
  public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand cmd) => await ((DbCommand)cmd).ExecuteReaderAsync();
  public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand cmd, CommandBehavior behavior) => await ((DbCommand)cmd).ExecuteReaderAsync(behavior);
  public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand cmd, CommandBehavior behavior, CancellationToken cancellationToken) => await ((DbCommand)cmd).ExecuteReaderAsync(behavior, cancellationToken);
  public static async Task<IDataReader> ExecuteReaderAsync(this IDbCommand cmd, CancellationToken cancellationToken) => await ((DbCommand)cmd).ExecuteReaderAsync(cancellationToken);
  public static async Task<object> ExecuteScalarAsync(this IDbCommand cmd) => await ((DbCommand)cmd).ExecuteScalarAsync();
  public static async Task<object> ExecuteScalarAsync(this IDbCommand cmd, CancellationToken cancellationToken) => await ((DbCommand)cmd).ExecuteScalarAsync(cancellationToken);
  #endregion

}
