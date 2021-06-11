using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class IndexRow {
    public IndexRow(DataRow row) {
      foreach (DataColumn column in row.Table.Columns) {
        switch (column.ColumnName) {
          case nameof(TABLE_CAT): TABLE_CAT = row.Field<string>(column); break;
          case nameof(TABLE_SCHEM): TABLE_SCHEM = row.Field<string>(column); break;
          case nameof(TABLE_NAME): TABLE_NAME = row.Field<string>(column); break;
          case nameof(NON_UNIQUE): NON_UNIQUE = row.Field<int?>(column); break;
          case nameof(INDEX_QUALIFIER): INDEX_QUALIFIER = row.Field<string>(column); break;
          case nameof(INDEX_NAME): INDEX_NAME = row.Field<string>(column); break;
          case nameof(TYPE): TYPE = row.Field<int?>(column); break;
          case nameof(ORDINAL_POSITION): ORDINAL_POSITION = row.Field<int?>(column); break;
          case nameof(COLUMN_NAME): COLUMN_NAME = row.Field<string>(column); break;
          case nameof(ASC_OR_DESC): ASC_OR_DESC = row.Field<string>(column); break;
          case nameof(CARDINALITY): CARDINALITY = row.Field<int?>(column); break;
          case nameof(PAGES): PAGES = row.Field<int?>(column); break;
          case nameof(FILTER_CONDITION): FILTER_CONDITION = row.Field<string>(column); break;
          default: throw new NotImplementedException(column.ColumnName);
        }
      }
    }

    public class Query {
      public string Catalog { get; set; }
      public string Schema { get; set; }
      public string Table { get; set; }
      public string Name { get; set; }

      public string[] RestrictionValues3 => new[] { Schema, Table, Name };
      public string[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
    }

    public string TABLE_CAT { get; }
    public string TABLE_SCHEM { get; }
    public string TABLE_NAME { get; }
    public int? NON_UNIQUE { get; }
    public string INDEX_QUALIFIER { get; }
    public string INDEX_NAME { get; }
    public int? TYPE { get; }
    public int? ORDINAL_POSITION { get; }
    public string COLUMN_NAME { get; }
    public string ASC_OR_DESC { get; }
    public int? CARDINALITY { get; }
    public int? PAGES { get; }
    public string FILTER_CONDITION { get; }
  }

}