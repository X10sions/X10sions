﻿//using System.Collections.Generic;
//using System.Data.Common;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;

namespace System.Data.Odbc {
  public static class OdbcCommandExtensions {

    //[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
    //public static OdbcCommand ConvertNamedParametersToPositionalParameters(this OdbcCommand command) {
    //  //1. Find all occurrences parameters references in the SQL statement (such as @MyParameter).
    //  //2. Find the corresponding parameter in the command's parameters list.
    //  //3. Add the found parameter to the newParameters list and replace the parameter reference in the SQL with a question mark (?).
    //  //4. Replace the command's parameters list with the newParameters list.
    //  var newParameters = new List<OdbcParameter>();
    //  var commandText = command.CommandText;
    //  commandText = DbCommandExtensions.NamedParameterPattern.Replace(commandText, match => {
    //    var parameter = command.Parameters.OfType<OdbcParameter>().FirstOrDefault(a => a.ParameterName == match.Groups[1].Value);
    //    if (parameter != null) {
    //      var parameterIndex = newParameters.Count;
    //      var newParameter = command.CreateParameter();
    //      newParameter.OdbcType = parameter.OdbcType;
    //      newParameter.ParameterName = "@parameter" + parameterIndex.ToString();
    //      newParameter.Value = parameter.Value;
    //      newParameters.Add(newParameter);
    //    }
    //    return "?";
    //  });
    //  command.Parameters.Clear();
    //  command.Parameters.AddRange(newParameters.ToArray());
    //  command.CommandText = commandText;
    //  return command;
    //}

  }
}