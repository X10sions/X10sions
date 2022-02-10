using System;
using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {
  public class IndexRow: BaseTypedDataRow {
    public IndexRow(DataRow row) : base(row) {  }

    public class Query {
      public string? Catalog { get; set; }
      public string? Schema { get; set; }
      public string? Table { get; set; }
      public string? Name { get; set; }

      public string?[] RestrictionValues3 => new[] { Schema, Table, Name };
      public string?[] RestrictionValues4 => new[] { Catalog, Schema, Table, Name };
    }

    public string TABLE_CAT { get => DataRow.Field<string>(nameof(TABLE_CAT)); set => DataRow[nameof(TABLE_CAT)] = value; }
    public string TABLE_SCHEM { get => DataRow.Field<string>(nameof(TABLE_SCHEM)); set => DataRow[nameof(TABLE_SCHEM)] = value; }
    public string TABLE_NAME { get => DataRow.Field<string>(nameof(TABLE_NAME)); set => DataRow[nameof(TABLE_NAME)] = value; }
    public int? NON_UNIQUE { get => DataRow.Field<int?>(nameof(NON_UNIQUE)); set => DataRow[nameof(NON_UNIQUE)] = value; }
    public string INDEX_QUALIFIER { get => DataRow.Field<string>(nameof(INDEX_QUALIFIER)); set => DataRow[nameof(INDEX_QUALIFIER)] = value; }
    public string INDEX_NAME { get => DataRow.Field<string>(nameof(INDEX_NAME)); set => DataRow[nameof(INDEX_NAME)] = value; }
    public int? TYPE { get => DataRow.Field<int?>(nameof(TYPE)); set => DataRow[nameof(TYPE)] = value; }
    public int? ORDINAL_POSITION { get => DataRow.Field<int?>(nameof(ORDINAL_POSITION)); set => DataRow[nameof(ORDINAL_POSITION)] = value; }
    public string COLUMN_NAME { get => DataRow.Field<string>(nameof(COLUMN_NAME)); set => DataRow[nameof(COLUMN_NAME)] = value; }
    public string ASC_OR_DESC { get => DataRow.Field<string>(nameof(ASC_OR_DESC)); set => DataRow[nameof(ASC_OR_DESC)] = value; }
    public int? CARDINALITY { get => DataRow.Field<int?>(nameof(CARDINALITY)); set => DataRow[nameof(CARDINALITY)] = value; }
    public int? PAGES { get => DataRow.Field<int?>(nameof(PAGES)); set => DataRow[nameof(PAGES)] = value; }
    public string FILTER_CONDITION { get => DataRow.Field<string>(nameof(FILTER_CONDITION)); set => DataRow[nameof(FILTER_CONDITION)] = value; }
  }

}