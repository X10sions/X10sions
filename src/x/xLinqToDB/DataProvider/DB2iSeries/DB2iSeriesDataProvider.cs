using Common.Data;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using System;
using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider.DB2iSeries;

public class DB2iSeriesDataProvider<TConnection, TDataReader> : DB2iSeriesDataProvider<TConnection> where TConnection : DbConnection, new() where TDataReader : IDataReader {
  public DB2iSeriesDataProvider(Version version) : base(version, typeof(TDataReader)) { }
}
public class DB2iSeriesDataProvider<TConnection> : DataProviderBase<TConnection> where TConnection : DbConnection, new() {

  public DB2iSeriesDataProvider(Version version, Type dataReaderType) : base(
    GetNameWithVersion_DB2iSeries(version),
    new DB2iSeriesMappingSchema(GetNameWithVersion_DB2iSeries(version)),
    dataReaderType,
    () => new DB2iSeriesSchemaProvider(),
    version.GetTableOptions_DB2iSeries()
    ) {
    //DataSourceInformationRow = dataSourceInformationRow;
    //DataSourceProductName = dataSourceProductName;
    //DataSourceVersion = dbVersion;

    //InitDataProvider();
  }

  static string GetNameWithVersion_DB2iSeries(Version version) => version.GetNameWithVersion(DbSystem.DB2iSeries.Name);
  //protected override ISqlOptimizer SqlOptimizer { get; }

  public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => throw new NotImplementedException();
  public override ISqlOptimizer GetSqlOptimizer() => new DB2iSeriesV5R4SqlOptimizer(SqlProviderFlags);
}