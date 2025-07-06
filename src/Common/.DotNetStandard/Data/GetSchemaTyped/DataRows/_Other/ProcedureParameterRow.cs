namespace Common.Data.GetSchemaTyped.DataRows;
public class ProcedureParameterRow : ITypedDataRow {
  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Procedure { get; set; }
    public string? Name { get; set; }

    public string?[] RestrictionValues3 => new[] { Schema, Procedure, Name };
    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Procedure, Name };
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
