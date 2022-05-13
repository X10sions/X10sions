using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;
public class IndexRow : BaseTypedDataRow {
  public IndexRow() { }
  public IndexRow(DataRow row) : base(row) { }

  public class Query {
    public string? Catalog { get; set; }
    public string? Schema { get; set; }
    public string? Table { get; set; }
    public string? Name { get; set; }

    public string?[] RestrictionValues3 => new[] { Schema, Table, Name };
    public string?[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
  }

  public override void SetValues(DataRow dataRow) {
    TABLE_CAT = dataRow.Field<string>(nameof(TABLE_CAT));
    TABLE_SCHEM = dataRow.Field<string>(nameof(TABLE_SCHEM));
    TABLE_NAME = dataRow.Field<string>(nameof(TABLE_NAME));
    NON_UNIQUE = dataRow.Field<int?>(nameof(NON_UNIQUE));
    INDEX_QUALIFIER = dataRow.Field<string>(nameof(INDEX_QUALIFIER));
    INDEX_NAME = dataRow.Field<string>(nameof(INDEX_NAME));
    TYPE = dataRow.Field<int?>(nameof(TYPE));
    ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    ASC_OR_DESC = dataRow.Field<string>(nameof(ASC_OR_DESC));
    CARDINALITY = dataRow.Field<int?>(nameof(CARDINALITY));
    PAGES = dataRow.Field<int?>(nameof(PAGES));
    FILTER_CONDITION = dataRow.Field<string>(nameof(FILTER_CONDITION));
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
