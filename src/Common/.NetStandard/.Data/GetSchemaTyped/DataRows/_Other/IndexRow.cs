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

  public override Dictionary<string, Action<DataRow>> GetColumnSetValueDictionary() {
    var dic = new Dictionary<string, Action<DataRow>>();
    //dic[nameof(TABLE_CAT)] = dataRow => TABLE_CAT = dataRow.Field<string>(nameof(TABLE_CAT));
    //dic[nameof(TABLE_SCHEM)] = dataRow => TABLE_SCHEM = dataRow.Field<string>(nameof(TABLE_SCHEM));
    //dic[nameof(TABLE_NAME)] = dataRow => TABLE_NAME = dataRow.Field<string>(nameof(TABLE_NAME));
    //dic[nameof(NON_UNIQUE)] = dataRow => NON_UNIQUE = dataRow.Field<int?>(nameof(NON_UNIQUE));
    //dic[nameof(INDEX_QUALIFIER)] = dataRow => INDEX_QUALIFIER = dataRow.Field<string>(nameof(INDEX_QUALIFIER));
    //dic[nameof(INDEX_NAME)] = dataRow => INDEX_NAME = dataRow.Field<string>(nameof(INDEX_NAME));
    //dic[nameof(TYPE)] = dataRow => TYPE = dataRow.Field<int?>(nameof(TYPE));
    //dic[nameof(ORDINAL_POSITION)] = dataRow => ORDINAL_POSITION = dataRow.Field<int?>(nameof(ORDINAL_POSITION));
    //dic[nameof(COLUMN_NAME)] = dataRow => COLUMN_NAME = dataRow.Field<string>(nameof(COLUMN_NAME));
    //dic[nameof(ASC_OR_DESC)] = dataRow => ASC_OR_DESC = dataRow.Field<string>(nameof(ASC_OR_DESC));
    //dic[nameof(CARDINALITY)] = dataRow => CARDINALITY = dataRow.Field<int?>(nameof(CARDINALITY));
    //dic[nameof(PAGES)] = dataRow => PAGES = dataRow.Field<int?>(nameof(PAGES));
    //dic[nameof(FILTER_CONDITION)] = dataRow => FILTER_CONDITION = dataRow.Field<string>(nameof(FILTER_CONDITION));
    return dic;
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
