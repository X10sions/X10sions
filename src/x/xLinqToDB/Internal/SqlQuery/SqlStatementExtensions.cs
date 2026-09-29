using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqToDB.Internal.SqlQuery;
  public static class SqlStatementExtensions {

  public static string GetProcessQueryExceptionMessage(this SqlStatement statement, EvaluationContext context, Exception ex, string dataConnectionTypeName) {
      //var parameters = statement.CollectParameters();
      //var parameters = context.ParameterValues;
      string sqlText = statement.ToString() ?? "Unable to resolve query text.";
      var parametersList = new List<SqlParameter>();
      statement.Visit(element => {
        if (element is SqlParameter sqlParameter) {
          parametersList.Add(sqlParameter);
        }
      });
      var formattedLines = new List<string>();
      foreach (SqlParameter p in parametersList) {
        object? clientValue = "Unresolved";
        object? providerValue = "Unresolved";
        if (context?.ParameterValues != null && context.ParameterValues.TryGetValue(p, out var paramValue)) {
          clientValue = paramValue.ClientValue;
          providerValue = paramValue.ProviderValue;
        }
        formattedLines.Add($"param: {p.Name}, clientValue = '{clientValue}', providerValue = '{providerValue}'");
      }
      var errorString = dataConnectionTypeName
        + Environment.NewLine + "SqlText: " + statement.SqlText
        + Environment.NewLine + "SelectQuery: " + statement.SelectQuery?.SqlText
        + Environment.NewLine + "Parameters: " + parametersList.Count
        + Environment.NewLine + string.Join(Environment.NewLine, parametersList.Select(x => $"param: {x.Name}= '{x.Value}'"))
        + Environment.NewLine + string.Join(Environment.NewLine, formattedLines);
      //Log.Logger.Error(ex, errorString);
      return $"{errorString}{Environment.NewLine}{ex.Message}";    
  }

}
