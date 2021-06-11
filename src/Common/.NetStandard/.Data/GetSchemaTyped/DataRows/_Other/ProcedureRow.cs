using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class ProcedureRow {
    public ProcedureRow(DataRow row) {
      foreach (DataColumn column in row.Table.Columns) {
        switch (column.ColumnName) {
          case nameof(PROCEDURE_CAT): PROCEDURE_CAT = row.Field<string>(column); break;
          case nameof(PROCEDURE_SCHEM): PROCEDURE_SCHEM = row.Field<string>(column); break;
          case nameof(PROCEDURE_NAME): PROCEDURE_NAME = row.Field<string>(column); break;
          case nameof(NUM_INPUT_PARAMS): NUM_INPUT_PARAMS = row.Field<short?>(column); break;
          case nameof(NUM_OUTPUT_PARAMS): NUM_OUTPUT_PARAMS = row.Field<short?>(column); break;
          case nameof(NUM_RESULT_SETS): NUM_RESULT_SETS = row.Field<short?>(column); break;
          case nameof(REMARKS): REMARKS = row.Field<string>(column); break;
          case nameof(PROCEDURE_TYPE): PROCEDURE_TYPE = row.Field<short?>(column); break;
          default: throw new NotImplementedException(column.ColumnName);
        }
      }
    }

    public class Query {
      public string Catalog { get; set; }
      public string Schema { get; set; }
      public string Name { get; set; }
      public string Type { get; set; }

      public string[] RestrictionValues3 => new[] { Schema, Name, Type };
      public string[] RestrictionValues4 => new[] { Catalog, Schema, Name, Type };
    }

    public string PROCEDURE_CAT { get; }
    public string PROCEDURE_SCHEM { get; }
    public string PROCEDURE_NAME { get; }
    public short? NUM_INPUT_PARAMS { get; }
    public short? NUM_OUTPUT_PARAMS { get; }
    public short? NUM_RESULT_SETS { get; }
    public string REMARKS { get; }
    public short? PROCEDURE_TYPE { get; }
  }

}