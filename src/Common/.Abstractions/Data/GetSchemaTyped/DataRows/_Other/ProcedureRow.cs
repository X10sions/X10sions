namespace Common.Data.GetSchemaTyped.DataRows;
public class ProcedureRow : ITypedDataRow {
  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    public string?[] RestrictionValues3 => new[] { Schema, Name, Type };
    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Name, Type };
  }

  public string PROCEDURE_CAT { get; set; } = string.Empty;
  public string PROCEDURE_SCHEM { get; set; } = string.Empty;
  public string PROCEDURE_NAME { get; set; } = string.Empty;
  public short? NUM_INPUT_PARAMS { get; set; }
  public short? NUM_OUTPUT_PARAMS { get; set; }
  public short? NUM_RESULT_SETS { get; set; }
  public string REMARKS { get; set; } = string.Empty;
  public short? PROCEDURE_TYPE { get; set; }
}
