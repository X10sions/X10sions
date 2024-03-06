using System.Collections;
using System.Text;

namespace System.Data {
  public static class IDbCommandExtensions {
    public static string ToSqlString(this IDbCommand command, bool doIncludeParameters = false) {
      var text = command.CommandText;
      var stringBuilder = new StringBuilder();
      var enumerator = default(IEnumerator);
      try {
        enumerator = command.Parameters.GetEnumerator();
        while (enumerator.MoveNext()) {
          var dbDataParameter = (IDbDataParameter)enumerator.Current;
          var sqlValue = dbDataParameter.DbType.GetSqlValue(dbDataParameter.Value, doIncludeQualifier: true);
          text = text.Replace(dbDataParameter.ParameterName, sqlValue);
          stringBuilder.AppendLine($"-- {dbDataParameter.ParameterName}({dbDataParameter.DbType}): {sqlValue}");
        }
      } finally {
        if (enumerator is IDisposable) {
          ((IDisposable)enumerator).Dispose();
        }
      }
      return text + ";" + (doIncludeParameters ? ("-- Parameters \r\n/*" + stringBuilder.ToString() + "*/") : string.Empty);
    }
  }
}
