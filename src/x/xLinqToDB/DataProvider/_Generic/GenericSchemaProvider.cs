using System;
using LinqToDB.Data;
using LinqToDB.SchemaProvider;
using System.Collections.Generic;
using Common.Data.GetSchemaTyped.DataRows;

namespace LinqToDB.DataProvider {
  public class GenericSchemaProvider : SchemaProviderBase {
    public GenericSchemaProvider(DataSourceInformationRow dataSourceInformationRow) : base() {
      this.dataSourceInformationRow = dataSourceInformationRow;
    }
    DataSourceInformationRow dataSourceInformationRow;

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();

    protected override DataType GetDataType(string? dataType, string? columnType, long? length, int? prec, int? scale) => throw new NotImplementedException();

    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();

    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();

    protected override string? GetProviderSpecificTypeNamespace() => throw new NotImplementedException();

    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();
  }
}

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesSchemaProvider : SchemaProviderBase {
    public DB2iSeriesSchemaProvider() : base() {
      //this.dataSourceInformationRow = dataSourceInformationRow;
    }
    //DataSourceInformationRow dataSourceInformationRow;

    protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();
    protected override DataType GetDataType(string? dataType, string? columnType, long? length, int? prec, int? scale) => throw new NotImplementedException();
    protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();
    protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) => throw new NotImplementedException();
    protected override string? GetProviderSpecificTypeNamespace() => throw new NotImplementedException();
    protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) => throw new NotImplementedException();
  }
}