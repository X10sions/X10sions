namespace Common.Data.GetSchemaTyped.DataRows;

public class TableRow : ITypedDataRow {
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
