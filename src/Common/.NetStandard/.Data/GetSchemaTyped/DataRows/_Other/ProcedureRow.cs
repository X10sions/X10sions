using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;
public class ProcedureRow : BaseTypedDataRow {
  public ProcedureRow(DataRow row) : base(row) { }

  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    public string?[] RestrictionValues3 => new[] { Schema, Name, Type };
    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Name, Type };
  }

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    //dic[nameof()] = dataRow =>
    //    PROCEDURE_CAT = dataRow.Field<string>(nameof(PROCEDURE_CAT));
    //PROCEDURE_SCHEM = dataRow.Field<string>(nameof(PROCEDURE_SCHEM));
    //PROCEDURE_NAME = dataRow.Field<string>(nameof(PROCEDURE_NAME));
    //NUM_INPUT_PARAMS = dataRow.Field<short?>(nameof(NUM_INPUT_PARAMS));
    //NUM_OUTPUT_PARAMS = dataRow.Field<short?>(nameof(NUM_OUTPUT_PARAMS));
    //NUM_RESULT_SETS = dataRow.Field<short?>(nameof(NUM_RESULT_SETS));
    //REMARKS = dataRow.Field<string>(nameof(REMARKS));
    //PROCEDURE_TYPE = dataRow.Field<short?>(nameof(PROCEDURE_TYPE));
    return dic;
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
