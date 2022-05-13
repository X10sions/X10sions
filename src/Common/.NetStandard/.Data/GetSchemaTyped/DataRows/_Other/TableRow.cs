using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows;

//public class TableDataTable : BaseTypedDataTable<TableDataRow> {
//  protected override TableDataRow NewRowFromBuilderTyped(DataRowBuilder builder) => new TableDataRow(builder);
//}

//public class TableDataRow : DataRow{
//  protected internal TableDataRow(DataRowBuilder builder) : base(builder) { }

//  public string TABLE_CAT { get => this.Field<string>(nameof(TABLE_CAT)); set => base[nameof(TABLE_CAT)] = value; }
//  public string TABLE_SCHEM { get => this.Field<string>(nameof(TABLE_SCHEM)); set => base[nameof(TABLE_SCHEM)] = value; }
//  public string TABLE_NAME { get => this.Field<string>(nameof(TABLE_NAME)); set => base[nameof(TABLE_NAME)] = value; }
//  public string TABLE_TYPE { get => this.Field<string>(nameof(TABLE_TYPE)); set => base[nameof(TABLE_TYPE)] = value; }
//  public string REMARKS { get => this.Field<string>(nameof(REMARKS)); set => base[nameof(REMARKS)] = value; }
//}

public class TableRow : BaseTypedDataRow {
  public TableRow() { }
  public TableRow(DataRow dataRow) : base(dataRow) { }

  public override void SetValues(DataRow dataRow) {
    TABLE_CAT = dataRow.Field<string>(nameof(TABLE_CAT));
    TABLE_SCHEM = dataRow.Field<string>(nameof(TABLE_SCHEM));
    TABLE_NAME = dataRow.Field<string>(nameof(TABLE_NAME));
    TABLE_TYPE = dataRow.Field<string>(nameof(TABLE_TYPE));
    REMARKS = dataRow.Field<string>(nameof(REMARKS));
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
