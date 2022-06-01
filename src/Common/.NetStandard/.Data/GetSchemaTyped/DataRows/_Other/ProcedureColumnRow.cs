using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ProcedureColumnRow : BaseTypedDataRow {
  public ProcedureColumnRow(DataRow row) : base(row) { }

  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Procedure { get; set; }
    public string? Name { get; set; }

    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Procedure, Name };
  }

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    //dic[nameof(BUFFER_LENGTH)] = dataRow => BUFFER_LENGTH = dataRow.Field<int?>(nameof(BUFFER_LENGTH));
    //dic[nameof(CHAR_OCTET_LENGTH)] = dataRow => CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(CHAR_OCTET_LENGTH));
    //dic[nameof(COLUMN_SIZE)] = dataRow => COLUMN_SIZE = dataRow.Field<int?>(nameof(COLUMN_SIZE));
    //dic[nameof(ORDINAL_POSITION)] = dataRow => ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    //dic[nameof(COLUMN_TYPE)] = dataRow => COLUMN_TYPE = dataRow.Field<short?>(nameof(COLUMN_TYPE));
    //dic[nameof(DATA_TYPE)] = dataRow => DATA_TYPE = dataRow.Field<short?>(nameof(DATA_TYPE));
    //dic[nameof(DECIMAL_DIGITS)] = dataRow => DECIMAL_DIGITS = dataRow.Field<short?>(nameof(DECIMAL_DIGITS));
    //dic[nameof(NULLABLE)] = dataRow => NULLABLE = dataRow.Field<short?>(nameof(NULLABLE));
    //dic[nameof(NUM_PREC_RADIX)] = dataRow => NUM_PREC_RADIX = dataRow.Field<short?>(nameof(NUM_PREC_RADIX));
    //dic[nameof(SQL_DATA_TYPE)] = dataRow => SQL_DATA_TYPE = dataRow.Field<short?>(nameof(SQL_DATA_TYPE));
    //dic[nameof(SQL_DATETIME_SUB)] = dataRow => SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(SQL_DATETIME_SUB));
    //dic[nameof(COLUMN_DEF)] = dataRow => COLUMN_DEF = dataRow.Field<string>(nameof(COLUMN_DEF));
    //dic[nameof(COLUMN_NAME)] = dataRow => COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    //dic[nameof(IS_NULLABLE)] = dataRow => IS_NULLABLE = dataRow.Field<string>(nameof(IS_NULLABLE));
    //dic[nameof(PROCEDURE_CAT)] = dataRow => PROCEDURE_CAT = dataRow.Field<string>(nameof(PROCEDURE_CAT));
    //dic[nameof(PROCEDURE_NAME)] = dataRow => PROCEDURE_NAME = dataRow.Field<string>(nameof(PROCEDURE_NAME));
    //dic[nameof(PROCEDURE_SCHEM)] = dataRow => PROCEDURE_SCHEM = dataRow.Field<string>(nameof(PROCEDURE_SCHEM));
    //dic[nameof(REMARKS)] = dataRow => REMARKS = dataRow.Field<string>(nameof(REMARKS));
    //dic[nameof(TYPE_NAME)] = dataRow => TYPE_NAME = dataRow.Field<string>(nameof(TYPE_NAME));
    return dic;
  }

  public int? BUFFER_LENGTH { get; set; }
  public int? CHAR_OCTET_LENGTH { get; set; }
  public int? COLUMN_SIZE { get; set; }
  public int? ORDINAL_POSITION { get; set; }
  public short? COLUMN_TYPE { get; set; }
  public short? DATA_TYPE { get; set; }
  public short? DECIMAL_DIGITS { get; set; }
  public short? NULLABLE { get; set; }
  public short? NUM_PREC_RADIX { get; set; }
  public short? SQL_DATA_TYPE { get; set; }
  public short? SQL_DATETIME_SUB { get; set; }
  public string COLUMN_DEF { get; set; } = string.Empty;
  public string COLUMN_NAME { get; set; } = string.Empty;
  public string IS_NULLABLE { get; set; } = string.Empty;
  public string PROCEDURE_CAT { get; set; } = string.Empty;
  public string PROCEDURE_NAME { get; set; } = string.Empty;
  public string PROCEDURE_SCHEM { get; set; } = string.Empty;
  public string REMARKS { get; set; } = string.Empty;
  public string TYPE_NAME { get; set; } = string.Empty;
}