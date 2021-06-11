using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ViewRow {
    public ViewRow(DataRow row) {
      foreach (DataColumn column in row.Table.Columns) {
        switch (column.ColumnName) {
          case nameof(TABLE_CAT): TABLE_CAT = row.Field<string>(column); break;
          case nameof(TABLE_SCHEM): TABLE_SCHEM = row.Field<string>(column); break;
          case nameof(TABLE_NAME): TABLE_NAME = row.Field<string>(column); break;
          case nameof(TABLE_TYPE): TABLE_TYPE = row.Field<string>(column); break;
          case nameof(REMARKS): REMARKS = row.Field<string>(column); break;
          default: throw new NotImplementedException(column.ColumnName);
        }
      }
    }

    public class Query {
      public string Catalog { get; set; }
      public string Schema { get; set; }
      public string Name { get; set; }
      public string Table { get; set; }

      public string[] RestrictionValues_ODBC => new[] { Catalog, Schema, Table };
      public string[] RestrictionValues_IDB2 => new[] { Schema, Name };
    }

    ////IDB2
    //public static DataTable Databases(this DbConnection connection, string name) => connection.GetSchemaDataTable(nameof(Databases), new[] { name });
    //public static DataTable IndexColumns(this DbConnection connection, string schema, string constraint, string name) => connection.GetSchemaDataTable(nameof(IndexColumns), new[] { schema, constraint, name });
    //public static DataTable Schemas(this DbConnection connection, string name) => connection.GetSchemaDataTable(nameof(Schemas), new[] { name });
    //public static DataTable Tables(this DbConnection connection, string schema, string name, string type) => connection.GetSchemaDataTable(nameof(Tables), new[] { schema, name, type });
    //public static DataTable ViewColumns(this DbConnection connection, string schema, string view, string name) => connection.GetSchemaDataTable(nameof(ViewColumns), new[] { schema, view, name });

    public string TABLE_CAT { get; }
    public string TABLE_SCHEM { get; }
    public string TABLE_NAME { get; }
    public string TABLE_TYPE { get; }
    public string REMARKS { get; }
  }

}