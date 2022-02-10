using System.Data;

namespace Common.Data.GetSchemaTyped.DataRows {

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
    public TableRow(DataRow dataRow) : base(dataRow) { }

    public string? TABLE_CAT { get => DataRow.Field<string>(nameof(TABLE_CAT)); set => DataRow[nameof(TABLE_CAT)] = value; }
    public string? TABLE_SCHEM { get => DataRow.Field<string>(nameof(TABLE_SCHEM)); set => DataRow[nameof(TABLE_SCHEM)] = value; }
    public string? TABLE_NAME { get => DataRow.Field<string>(nameof(TABLE_NAME)); set => DataRow[nameof(TABLE_NAME)] = value; }
    public string? TABLE_TYPE { get => DataRow.Field<string>(nameof(TABLE_TYPE)); set => DataRow[nameof(TABLE_TYPE)] = value; }
    public string? REMARKS { get => DataRow.Field<string>(nameof(REMARKS)); set => DataRow[nameof(REMARKS)] = value; }

    public class Query {
      public string? Catalog { get; set; }
      public string? Schema { get; set; }
      public string? Name { get; set; }
      public string? Type { get; set; }

      public string?[] RestrictionValues_ODBC => new[] { Catalog, Schema, Name };
      public string?[] RestrictionValues_IDB2 => new[] { Schema, Name, Type };
    }

  }
}