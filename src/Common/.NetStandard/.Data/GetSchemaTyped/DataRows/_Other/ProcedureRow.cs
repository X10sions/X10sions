using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ProcedureRow:BaseTypedDataRow {
    public ProcedureRow(DataRow row) :base(row){ }

    public class Query {
      public string? Catalog { get; set; }
      public string? Schema { get; set; }
      public string? Name { get; set; }
      public string? Type { get; set; }

      public string?[] RestrictionValues3 => new[] { Schema, Name, Type };
      public string?[] RestrictionValues4 => new[] { Catalog, Schema, Name, Type };
    }

    public string PROCEDURE_CAT { get => DataRow.Field<string>(nameof(PROCEDURE_CAT)); set => DataRow[nameof(PROCEDURE_CAT)] = value; }
    public string PROCEDURE_SCHEM { get => DataRow.Field<string>(nameof(PROCEDURE_SCHEM)); set => DataRow[nameof(PROCEDURE_SCHEM)] = value; }
    public string PROCEDURE_NAME { get => DataRow.Field<string>(nameof(PROCEDURE_NAME)); set => DataRow[nameof(PROCEDURE_NAME)] = value; }
    public short? NUM_INPUT_PARAMS { get => DataRow.Field<short?>(nameof(NUM_INPUT_PARAMS)); set => DataRow[nameof(NUM_INPUT_PARAMS)] = value; }
    public short? NUM_OUTPUT_PARAMS { get => DataRow.Field<short?>(nameof(NUM_OUTPUT_PARAMS)); set => DataRow[nameof(NUM_OUTPUT_PARAMS)] = value; }
    public short? NUM_RESULT_SETS { get => DataRow.Field<short?>(nameof(NUM_RESULT_SETS)); set => DataRow[nameof(NUM_RESULT_SETS)] = value; }
    public string REMARKS { get => DataRow.Field<string>(nameof(REMARKS)); set => DataRow[nameof(REMARKS)] = value; }
    public short? PROCEDURE_TYPE { get => DataRow.Field<short?>(nameof(PROCEDURE_TYPE)); set => DataRow[nameof(PROCEDURE_TYPE)] = value; }
  }

}