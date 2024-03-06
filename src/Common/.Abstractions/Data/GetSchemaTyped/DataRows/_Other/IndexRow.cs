namespace Common.Data.GetSchemaTyped.DataRows;
public class IndexRow : ITypedDataRow {
  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Table { get; set; }
    public string? Name { get; set; }

    public string?[] RestrictionValues3 => new[] { Schema, Table, Name };
    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
  }
  public string TABLE_CAT { get; set; } = string.Empty;
  public string TABLE_SCHEM { get; set; } = string.Empty;
  public string TABLE_NAME { get; set; } = string.Empty;
  public int? NON_UNIQUE { get; set; }
  public string INDEX_QUALIFIER { get; set; } = string.Empty;
  public string INDEX_NAME { get; set; } = string.Empty;
  public int? TYPE { get; set; }
  public int? ORDINAL_POSITION { get; set; }
  public string COLUMN_NAME { get; set; } = string.Empty;
  public string ASC_OR_DESC { get; set; } = string.Empty;
  public int? CARDINALITY { get; set; }
  public int? PAGES { get; set; }
  public string FILTER_CONDITION { get; set; } = string.Empty;
}
