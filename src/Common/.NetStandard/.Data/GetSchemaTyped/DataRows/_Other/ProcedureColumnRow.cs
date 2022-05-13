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

  public override void SetValues(DataRow dataRow) {
    BUFFER_LENGTH = dataRow.Field<int?>(nameof(BUFFER_LENGTH));
    CHAR_OCTET_LENGTH = dataRow.Field<int?>(nameof(CHAR_OCTET_LENGTH));
    COLUMN_SIZE = dataRow.Field<int?>(nameof(COLUMN_SIZE));
    ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    COLUMN_TYPE = dataRow.Field<short?>(nameof(COLUMN_TYPE));
    DATA_TYPE = dataRow.Field<short?>(nameof(DATA_TYPE));
    DECIMAL_DIGITS = dataRow.Field<short?>(nameof(DECIMAL_DIGITS));
    NULLABLE = dataRow.Field<short?>(nameof(NULLABLE));
    NUM_PREC_RADIX = dataRow.Field<short?>(nameof(NUM_PREC_RADIX));
    SQL_DATA_TYPE = dataRow.Field<short?>(nameof(SQL_DATA_TYPE));
    SQL_DATETIME_SUB = dataRow.Field<short?>(nameof(SQL_DATETIME_SUB));
    COLUMN_DEF = dataRow.Field<string>(nameof(COLUMN_DEF));
    COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    IS_NULLABLE = dataRow.Field<string>(nameof(IS_NULLABLE));
    PROCEDURE_CAT = dataRow.Field<string>(nameof(PROCEDURE_CAT));
    PROCEDURE_NAME = dataRow.Field<string>(nameof(PROCEDURE_NAME));
    PROCEDURE_SCHEM = dataRow.Field<string>(nameof(PROCEDURE_SCHEM));
    REMARKS = dataRow.Field<string>(nameof(REMARKS));
    TYPE_NAME = dataRow.Field<string>(nameof(TYPE_NAME));
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