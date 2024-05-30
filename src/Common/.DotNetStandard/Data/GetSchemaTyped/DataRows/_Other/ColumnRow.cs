namespace Common.Data.GetSchemaTyped.DataRows;
public class ColumnRow : ITypedDataRow {
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
