using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ColumnRow {
    public ColumnRow(DataRow row) {
      foreach (DataColumn column in row.Table.Columns) {
        switch (column.ColumnName) {
          case nameof(TABLE_CAT): TABLE_CAT = row.Field<string>(column); break;
          case nameof(TABLE_SCHEM): TABLE_SCHEM = row.Field<string>(column); break;
          case nameof(TABLE_NAME): TABLE_NAME = row.Field<string>(column); break;
          case nameof(COLUMN_NAME): COLUMN_NAME = row.Field<string>(column); break;
          case nameof(DATA_TYPE): DATA_TYPE = row.Field<short?>(column); break;
          case nameof(TYPE_NAME): TYPE_NAME = row.Field<string>(column); break;
          case nameof(COLUMN_SIZE): COLUMN_SIZE = row.Field<int?>(column); break;
          case nameof(BUFFER_LENGTH): BUFFER_LENGTH = row.Field<int?>(column); break;
          case nameof(DECIMAL_DIGITS): DECIMAL_DIGITS = row.Field<short?>(column); break;
          case nameof(NUM_PREC_RADIX): NUM_PREC_RADIX = row.Field<short?>(column); break;
          case nameof(NULLABLE): NULLABLE = row.Field<short?>(column); break;
          case nameof(REMARKS): REMARKS = row.Field<string>(column); break;
          case nameof(COLUMN_DEF): COLUMN_DEF = row.Field<string>(column); break;
          case nameof(SQL_DATA_TYPE): SQL_DATA_TYPE = row.Field<short?>(column); break;
          case nameof(SQL_DATETIME_SUB): SQL_DATETIME_SUB = row.Field<short?>(column); break;
          case nameof(CHAR_OCTET_LENGTH): CHAR_OCTET_LENGTH = row.Field<int?>(column); break;
          case nameof(ORDINAL_POSITION): ORDINAL_POSITION = row.Field<int?>(column); break;
          case nameof(IS_NULLABLE): IS_NULLABLE = row.Field<string>(column); break;
          default: throw new NotImplementedException(column.ColumnName);
        }
      }
    }

    public class Query {
      public string Catalog { get; set; }
      public string Schema { get; set; }
      public string Table { get; set; }
      public string Name { get; set; }

      //public string[] RestrictionValues(DbConnection conn) {
      //  var sb = new StringBuilder();
      //  foreach (var r in conn.RestrictionsTyped().OrderBy(x=> x.RestrictionNumber)) {
      //  }

      //} => new[] { Schema, Table, Name };
      //public string[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
    }


    public string TABLE_CAT { get; }
    public string TABLE_SCHEM { get; }
    public string TABLE_NAME { get; }
    public string COLUMN_NAME { get; }
    public short? DATA_TYPE { get; }
    public string TYPE_NAME { get; }
    public int? COLUMN_SIZE { get; }
    public int? BUFFER_LENGTH { get; }
    public short? DECIMAL_DIGITS { get; }
    public short? NUM_PREC_RADIX { get; }
    public short? NULLABLE { get; }
    public string REMARKS { get; }
    public string COLUMN_DEF { get; }
    public short? SQL_DATA_TYPE { get; }
    public short? SQL_DATETIME_SUB { get; }
    public int? CHAR_OCTET_LENGTH { get; }
    public int? ORDINAL_POSITION { get; }
    public string IS_NULLABLE { get; }
  }

}