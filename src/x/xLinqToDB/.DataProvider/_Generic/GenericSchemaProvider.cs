﻿using LinqToDB.Data;
using LinqToDB.SchemaProvider;

namespace LinqToDB.DataProvider {
  public class GenericSchemaProvider : SchemaProviderBase {
    public GenericSchemaProvider() : base() {
      //DataSourceInformationRow dataSourceInformationRow
      //this.dataSourceInformationRow = dataSourceInformationRow;
    }
    // DataSourceInformationRow dataSourceInformationRow;

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();

    protected override DataType GetDataType(string? dataType, string? columnType, int? length, int? prec, int? scale) => throw new NotImplementedException();

    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();

    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();

    protected override string? GetProviderSpecificTypeNamespace() => throw new NotImplementedException();

    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();

  }
}
