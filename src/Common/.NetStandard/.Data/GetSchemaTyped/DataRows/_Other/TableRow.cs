using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class TableRow {
    public TableRow(DataRow row) {
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
      public string Type { get; set; }

      public string[] RestrictionValues_ODBC => new[] { Catalog, Schema, Name };
      public string[] RestrictionValues_IDB2 => new[] { Schema, Name, Type };
    }

    public string TABLE_CAT { get; }
    public string TABLE_SCHEM { get; }
    public string TABLE_NAME { get; }
    public string TABLE_TYPE { get; }
    public string REMARKS { get; }

  }

}