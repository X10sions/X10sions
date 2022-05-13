using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ColumnRow : BaseTypedDataRow {
  public ColumnRow() { }
  public ColumnRow(DataRow row) : base(row) { }

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

  public override void SetValues(DataRow dataRow) {
    TABLE_CAT = dataRow.Field<string>(nameof(TABLE_CAT));
    TABLE_SCHEM = dataRow.Field<string>(nameof(TABLE_SCHEM));
    TABLE_NAME = dataRow.Field<string>(nameof(TABLE_NAME));
    COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    DATA_TYPE = dataRow.Field<short?>(nameof(DATA_TYPE));
    TYPE_NAME = dataRow.Field<string>(nameof(TYPE_NAME));
    COLUMN_SIZE = dataRow.Field<int?>(nameof(COLUMN_SIZE));
    BUFFER_LENGTH = dataRow.Field<int?>(nameof(BUFFER_LENGTH));
    DECIMAL_DIGITS = dataRow.Field<short?>(nameof(DECIMAL_DIGITS));
    NUM_PREC_RADIX = dataRow.Field<short?>(nameof(NUM_PREC_RADIX));
    NULLABLE = dataRow.Field<short?>(nameof(NULLABLE));
    REMARKS = dataRow.Field<string>(nameof(REMARKS));
    COLUMN_DEF = dataRow.Field<string>(nameof(COLUMN_DEF));
    SQL_DATA_TYPE = dataRow.Field<short?>(nameof(SQL_DATA_TYPE));
    SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(SQL_DATETIME_SUB));
    CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(CHAR_OCTET_LENGTH));
    ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    IS_NULLABLE = dataRow.Field<string>(nameof(IS_NULLABLE));
  }

  public string? TABLE_CAT { get; set; }
  public string? TABLE_SCHEM { get; set; }
  public string? TABLE_NAME { get; set; }
  public string COLUMN_NAME { get; set; } = string.Empty;
  public short? DATA_TYPE { get; set; }
  public string TYPE_NAME { get; set; } = string.Empty;
  public int? COLUMN_SIZE { get; set; }
  public int? BUFFER_LENGTH { get; set; }
  public short? DECIMAL_DIGITS { get; set; }
  public short? NUM_PREC_RADIX { get; set; }
  public short? NULLABLE { get; set; }
  public string? REMARKS { get; set; }
  public string COLUMN_DEF { get; set; } = string.Empty;
  public short? SQL_DATA_TYPE { get; set; }
  public short? SQL_DATETIME_SUB { get; set; }
  public int? CHAR_OCTET_LENGTH { get; set; }
  public int? ORDINAL_POSITION { get; set; }
  public string IS_NULLABLE { get; set; } = string.Empty;
}
