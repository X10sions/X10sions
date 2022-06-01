using System.Data;
using System.Data.Common;

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

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    //dic[nameof(TABLE_CAT)] = dataRow => TABLE_CAT = dataRow.Field<string>(nameof(TABLE_CAT));
    //dic[nameof(TABLE_SCHEM)] = dataRow => TABLE_SCHEM = dataRow.Field<string>(nameof(TABLE_SCHEM));
    //dic[nameof(TABLE_NAME)] = dataRow => TABLE_NAME = dataRow.Field<string>(nameof(TABLE_NAME));
    //dic[nameof(COLUMN_NAME)] = dataRow => COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    //dic[nameof(DATA_TYPE)] = dataRow => DATA_TYPE = dataRow.Field<short?>(nameof(DATA_TYPE));
    //dic[nameof(TYPE_NAME)] = dataRow => TYPE_NAME = dataRow.Field<string>(nameof(TYPE_NAME));
    //dic[nameof(COLUMN_SIZE)] = dataRow => COLUMN_SIZE = dataRow.Field<int?>(nameof(COLUMN_SIZE));
    //dic[nameof(BUFFER_LENGTH)] = dataRow => BUFFER_LENGTH = dataRow.Field<int?>(nameof(BUFFER_LENGTH));
    //dic[nameof(DECIMAL_DIGITS)] = dataRow => DECIMAL_DIGITS = dataRow.Field<short?>(nameof(DECIMAL_DIGITS));
    //dic[nameof(NUM_PREC_RADIX)] = dataRow => NUM_PREC_RADIX = dataRow.Field<short?>(nameof(NUM_PREC_RADIX));
    //dic[nameof(NULLABLE)] = dataRow => NULLABLE = dataRow.Field<short?>(nameof(NULLABLE));
    //dic[nameof(REMARKS)] = dataRow => REMARKS = dataRow.Field<string>(nameof(REMARKS));
    //dic[nameof(COLUMN_DEF)] = dataRow => COLUMN_DEF = dataRow.Field<string>(nameof(COLUMN_DEF));
    //dic[nameof(SQL_DATA_TYPE)] = dataRow => SQL_DATA_TYPE = dataRow.Field<short?>(nameof(SQL_DATA_TYPE));
    //dic[nameof(SQL_DATETIME_SUB)] = dataRow => SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(SQL_DATETIME_SUB));
    //dic[nameof(CHAR_OCTET_LENGTH)] = dataRow => CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(CHAR_OCTET_LENGTH));
    //dic[nameof(ORDINAL_POSITION)] = dataRow => ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    //dic[nameof(IS_NULLABLE)] = dataRow => IS_NULLABLE = dataRow.Field<string>(nameof(IS_NULLABLE));
    return dic;
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
