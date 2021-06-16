using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.Data.Odbc {
  public static class OdbcConnectionExtensions {

    //public static GetSchemaTyped_Odbc GetSchemaTyped_Odbc(this OdbcConnection connection) => new GetSchemaTyped_Odbc(connection);

    static readonly Regex namedParameterPattern = new Regex(@"\?(\w+)");

    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
    public static OdbcCommand CreateCommandWithNamedParameters(this OdbcConnection connection, string sql, IDictionary<string, object> parameters) {
      var cmd = connection.CreateCommand();
      var parameterIndex = 0;
      var commandText = namedParameterPattern.Replace(sql, (m) => {
        var key = m.Groups[1].Value;
        var value = parameters[key];
        var parameterName = string.Format("{0}_{1}", key, parameterIndex++);
        if (value is string || !(value is IEnumerable)) {
          cmd.Parameters.AddWithValue(parameterName, value ?? DBNull.Value);
          return "?";
        } else {
          var enumerable = ((IEnumerable)value).Cast<object>();
          var i = 0;
          foreach (var el in enumerable) {
            var elementName = string.Format("{0}_{1}", parameterName, i++);
            cmd.Parameters.AddWithValue(elementName, el ?? DBNull.Value);
          }
          return string.Join(",", enumerable.Select(_ => "?"));
        }
      });
      cmd.CommandText = commandText;
      return cmd;
    }

    public static string GetLibraryList(this OdbcConnection dbConnection) => new OdbcConnectionStringBuilder(dbConnection.ConnectionString)["LibraryList"].ToString();

    //ODBC
    public static DataTable Columns(this OdbcConnection connection, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Columns), new[] { catalog, schema, table, name });
    public static DataTable Indexes(this OdbcConnection connection, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Indexes), new[] { catalog, schema, table, name });
    public static DataTable Procedures(this OdbcConnection connection, string catalog, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Procedures), new[] { catalog, schema, name, type });
    public static DataTable ProcedureColumns(this OdbcConnection connection, string catalog, string schema, string procedure, string name) => connection.GetSchemaDataTable(nameof(ProcedureColumns), new[] { catalog, schema, procedure, name });
    public static DataTable ProcedureParameters(this OdbcConnection connection, string catalog, string schema, string procedure, string column) => connection.GetSchemaDataTable(nameof(ProcedureParameters), new[] { catalog, schema, procedure, column });
    public static DataTable Tables(this OdbcConnection connection, string catalog, string schema, string name) => connection.GetSchemaDataTable(nameof(Tables), new[] { catalog, schema, name });
    public static DataTable Views(this OdbcConnection connection, string catalog, string schema, string table) => connection.GetSchemaDataTable(nameof(Views), new[] { catalog, schema, table });


  }
}