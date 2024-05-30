using System.Data.Common;

namespace System.Data.Odbc;
  public static class OdbcConnectionExtensions {

  public static DataTable Columns(this OdbcConnection connection, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Columns), new[] { catalog, schema, table, name });
  public static DataTable Indexes(this OdbcConnection connection, string catalog, string schema, string table, string name) => connection.GetSchemaDataTable(nameof(Indexes), new[] { catalog, schema, table, name });
  public static DataTable Procedures(this OdbcConnection connection, string catalog, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Procedures), new[] { catalog, schema, name, type });
  public static DataTable ProcedureColumns(this OdbcConnection connection, string catalog, string schema, string procedure, string name) => connection.GetSchemaDataTable(nameof(ProcedureColumns), new[] { catalog, schema, procedure, name });
  public static DataTable ProcedureParameters(this OdbcConnection connection, string catalog, string schema, string procedure, string column) => connection.GetSchemaDataTable(nameof(ProcedureParameters), new[] { catalog, schema, procedure, column });
  public static DataTable Tables(this OdbcConnection connection, string catalog, string schema, string name) => connection.GetSchemaDataTable(nameof(Tables), new[] { catalog, schema, name });
  public static DataTable Views(this OdbcConnection connection, string catalog, string schema, string table) => connection.GetSchemaDataTable(nameof(Views), new[] { catalog, schema, table });


}
