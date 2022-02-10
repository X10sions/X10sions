using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ColumnRow:BaseTypedDataRow {
    public ColumnRow(DataRow row):base(row) {    }

    public class Query {
      public string? Catalog { get; set; }
      public string? Schema { get; set; }
      public string? Table { get; set; }
      public string? Name { get; set; }

      //public string[] RestrictionValues(DbConnection conn) {
      //  var sb = new StringBuilder();
      //  foreach (var r in conn.RestrictionsTyped().OrderBy(x=> x.RestrictionNumber)) {
      //  }

      //} => new[] { Schema, Table, Name };
      //public string[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
    }
        
    public string? TABLE_CAT { get => DataRow.Field<string>(nameof(TABLE_CAT)); set => DataRow[nameof(TABLE_CAT)] = value; }
    public string? TABLE_SCHEM { get => DataRow.Field<string>(nameof(TABLE_SCHEM)); set => DataRow[nameof(TABLE_SCHEM)] = value; }
    public string? TABLE_NAME { get => DataRow.Field<string>(nameof(TABLE_NAME)); set => DataRow[nameof(TABLE_NAME)] = value; }
    public string COLUMN_NAME { get => DataRow.Field<string>(nameof(COLUMN_NAME)); set => DataRow[nameof(COLUMN_NAME)] = value; }
    public short? DATA_TYPE { get => DataRow.Field<short?>(nameof(DATA_TYPE)); set => DataRow[nameof(DATA_TYPE)] = value; }
    public string TYPE_NAME { get => DataRow.Field<string>(nameof(TYPE_NAME)); set => DataRow[nameof(TYPE_NAME)] = value; }
    public int? COLUMN_SIZE { get => DataRow.Field<int?>(nameof(COLUMN_SIZE)); set => DataRow[nameof(COLUMN_SIZE)] = value; }
    public int? BUFFER_LENGTH { get => DataRow.Field<int?>(nameof(BUFFER_LENGTH)); set => DataRow[nameof(BUFFER_LENGTH)] = value; }
    public short? DECIMAL_DIGITS { get => DataRow.Field<short?>(nameof(DECIMAL_DIGITS)); set => DataRow[nameof(DECIMAL_DIGITS)] = value; }
    public short? NUM_PREC_RADIX { get => DataRow.Field<short?>(nameof(NUM_PREC_RADIX)); set => DataRow[nameof(NUM_PREC_RADIX)] = value; }
    public short? NULLABLE { get => DataRow.Field<short?>(nameof(NULLABLE)); set => DataRow[nameof(NULLABLE)] = value; }
    public string? REMARKS { get => DataRow.Field<string>(nameof(REMARKS)); set => DataRow[nameof(REMARKS)] = value; }
    public string COLUMN_DEF { get => DataRow.Field<string>(nameof(COLUMN_DEF)); set => DataRow[nameof(COLUMN_DEF)] = value; }
    public short? SQL_DATA_TYPE { get => DataRow.Field<short?>(nameof(SQL_DATA_TYPE)); set => DataRow[nameof(SQL_DATA_TYPE)] = value; }
    public short? SQL_DATETIME_SUB { get => DataRow.Field<short?>(nameof(SQL_DATETIME_SUB)); set => DataRow[nameof(SQL_DATETIME_SUB)] = value; }
    public int? CHAR_OCTET_LENGTH { get => DataRow.Field<int?>(nameof(CHAR_OCTET_LENGTH)); set => DataRow[nameof(CHAR_OCTET_LENGTH)] = value; }
    public int? ORDINAL_POSITION { get => DataRow.Field<int?>(nameof(ORDINAL_POSITION)); set => DataRow[nameof(ORDINAL_POSITION)] = value; }
    public string IS_NULLABLE { get => DataRow.Field<string>(nameof(IS_NULLABLE)); set => DataRow[nameof(IS_NULLABLE)] = value; }
  }

}