namespace System.Data.Common;
public static class DbCommandExtensions {

  //public static DbDataAdapter CreateDataAdapter(this DbCommand selectCommand, DbCommand selectCommand, DbCommand? deleteCommand = null, DbCommand? insertCommand = null, DbCommand? updateCommand = null)  =>  selectCommand.Connection.CreateDataAdapter(selectCommand, deleteCommand ,  insertCommand ,  updateCommand);

  public static DbParameter GetParameter(this DbCommand command, string name, object value) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = name;
    parameter.Value = value != null ? value : DBNull.Value;
    parameter.Direction = ParameterDirection.Input;
    return parameter;
  }

  public static DbParameter GetParameterOut(this DbCommand command, string name, DbType type, object? value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput) {
    var parameter = command.CreateParameter();
    parameter.ParameterName = name;
    parameter.DbType = type;
    if (type == DbType.AnsiString || type == DbType.String) {
      parameter.Size = -1;
    }
    parameter.Direction = parameterDirection;
    if (value != null) {
      parameter.Value = value;
    } else {
      parameter.Value = DBNull.Value;
    }
    return parameter;
  }
}