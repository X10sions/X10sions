using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data {
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

    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
    public static IDbCommand ReplaceCommndTextAndParameters(this IDbCommand command, string commandText, List<IDbDataParameter> parameters) {
      command.CommandText = commandText;
      command.Parameters.ReplaceAll(parameters);
      return command;
    }

    public static IDbCommand ConvertNamedParametersToPositionalParameters(this IDbCommand command, char parameterPrefix = '@') {
      var positional = command.GetPositionalCommandTextAndParameters(parameterPrefix);
      return command.ReplaceCommndTextAndParameters(positional.CommandText, positional.Parameters);
    }

    //[Obsolete("Does not work for Select @@Identity")]
    public static class NamedParameterPrefixPattern {
      public static readonly Regex AtSign = new Regex("(@\\w*)", RegexOptions.IgnoreCase);
      public static readonly Regex DollarSign = new Regex("($\\w*)", RegexOptions.IgnoreCase);
      public static readonly Regex Colon = new Regex("(:\\w*)", RegexOptions.IgnoreCase);
    }

    public static Regex GetNamedParameterPrefixPattern(char parameterPrefix) {
      switch (parameterPrefix) {
        case '@': return NamedParameterPrefixPattern.AtSign;
        case ':': return NamedParameterPrefixPattern.Colon;
        case '$': return NamedParameterPrefixPattern.DollarSign;
        default: return new Regex($@"({parameterPrefix}\w*)", RegexOptions.IgnoreCase);
      }
    }

    public static (string CommandText, List<IDbDataParameter> Parameters) GetPositionalCommandTextAndParameters(this IDbCommand command, char parameterPrefix = '@') {
      var namedParameterPrefixPattern = GetNamedParameterPrefixPattern(parameterPrefix);
      var newParameters = new List<IDbDataParameter>();
      var newCommandText = namedParameterPrefixPattern.Replace(command.CommandText, evaluator => {
        var match = evaluator.Groups[1].Value;
        var parameter = (from p in command.Parameters.OfType<IDbDataParameter>()
                         where p.ParameterName == match.TrimStart(parameterPrefix)
                         select command.CloneParameter(p)).FirstOrDefault();
        if (parameter != null) {
          parameter.ParameterName = $"{parameter.ParameterName}_{newParameters.Count}";
          newParameters.Add(parameter);
          return "?";
        }
        return match;
      });
      return (newCommandText, newParameters);
    }


    //public static (string CommandText, List<IDbDataParameter> Parameters) GetConvertNamedParametersToPositionalParameters2(this IDbCommand command) {
    //  //1. Find all occurrences parameters references in the SQL statement (such as @MyParameter).
    //  //2. Find the corresponding parameter in the command's parameters list.
    //  //3. Add the found parameter to the newParameters list and replace the parameter reference in the SQL with a question mark (?).
    //  //4. Replace the command's parameters list with the newParameters list.
    //  var oldParameters = command.Parameters;
    //  var oldCommandText = command.CommandText;
    //  var newParameters = new List<IDbDataParameter>();
    //  var newCommandText = oldCommandText;
    //  var paramNames = oldCommandText.Replace("@@", "??").Split('@').Select(x => x.Split(new[] { ' ', ')', ';', '\r', '\n' }).FirstOrDefault().Trim()).ToList().Skip(1);
    //  foreach (var p in paramNames) {
    //    newCommandText = newCommandText.Replace("@" + p, "?");
    //    var parameter = oldParameters.OfType<IDbDataParameter>().FirstOrDefault(a => a.ParameterName == p);
    //    if (parameter != null) {
    //      parameter.ParameterName = $"{parameter.ParameterName}_{newParameters.Count}";
    //      newParameters.Add(parameter);
    //    }
    //  }
    //  return (newCommandText, newParameters);
    //}

    //[Obsolete("Does not work properly, needs more testing")]
    //public static IDbCommand RewriteNamedParametersToPositionalParameters(this IDbCommand command) {
    //  var newCommand = command.GetRewriteNamedParametersToPositionalParameters();
    //  return command.ReplaceCommndTextAndParameters(newCommand.CommandText, newCommand.Parameters);
    //}

    //[Obsolete("Does not work properly, needs more testing")]
    //public static (string CommandText, List<IDbDataParameter> Parameters) GetRewriteNamedParametersToPositionalParameters(this IDbCommand command) {
    //  var newCommandText = command.CommandText;
    //  var newParameters = new List<IDbDataParameter>();

    //  var parameterMatches = command.Parameters.Cast<IDbDataParameter>().Select(x => Regex.Matches(newCommandText, "@" + x.ParameterName)).ToList();
    //  // Check to see if any of the parameters are listed multiple times in the command text.
    //  if (parameterMatches.Any(x => x.Count > 1)) {
    //    // order by descending to make the parameter name replacing easy
    //    var matches = parameterMatches.SelectMany(x => x.Cast<Match>()).OrderByDescending(x => x.Index);
    //    foreach (var match in matches) {
    //      // Substring removed the @ prefix.
    //      var parameterName = match.Value.Substring(1);
    //      // Add index to the name to make the parameter name unique.
    //      var newParameterName = parameterName + "_" + match.Index;
    //      var newParameter = (IDbDataParameter)((ICloneable)command.Parameters[parameterName]).Clone();
    //      newParameter.ParameterName = newParameterName;
    //      newParameters.Add(newParameter);
    //      // Replace the old parameter name with the new parameter name.
    //      newCommandText = newCommandText.Substring(0, match.Index) + "@" + newParameterName + newCommandText.Substring(match.Index + match.Length);
    //    }
    //    // The parameters were added to the list in the reverse order to make parameter name replacing easy.
    //    newParameters.Reverse();
    //    //ReplaceParameterNamesWithQuestionMark
    //    for (var index = command.Parameters.Count - 1; index >= 0; index--) {
    //      var p = (IDbDataParameter)command.Parameters[index];
    //      newCommandText = newCommandText.Replace("@" + p.ParameterName, "?");
    //    }
    //  }
    //  return (newCommandText, newParameters);
    //}

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
}