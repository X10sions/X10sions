using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;

public class TableRow : ITypedDataRow {
  public TableRow() { }
  public TableRow(DataRow dataRow) {
    SetValues(dataRow);
  }

  public void SetValues(DataRow dataRow) {
    TABLE_CAT = dataRow.Field<string?>(nameof(TABLE_CAT));
    TABLE_SCHEM = dataRow.Field<string?>(nameof(TABLE_SCHEM));
    TABLE_NAME = dataRow.Field<string?>(nameof(TABLE_NAME));
    TABLE_TYPE = dataRow.Field<string?>(nameof(TABLE_TYPE));
    REMARKS = dataRow.Field<string?>(nameof(REMARKS));
  }

  public string? TABLE_CAT { get; set; }
  public string? TABLE_SCHEM { get; set; }
  public string? TABLE_NAME { get; set; }
  public string? TABLE_TYPE { get; set; }
  public string? REMARKS { get; set; }

  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    public string?[] RestrictionValues_ODBC => new[] { Catalog, Schema, Name };
    public string?[] RestrictionValues_IDB2 => new[] { Schema, Name, Type };
  }

}
