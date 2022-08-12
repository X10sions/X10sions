using Common.Data;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.SchemaProvider;

namespace LinqToDB.DataProvider.DB2iSeries;

public interface IDB2iSeriesDataProvider : IDataProvider {
  //DataSourceInformationRow DataSourceInformationRow { get; }
}

class DB2iSeriesDataProvider<TConnection, TDataReader> : DB2iSeriesDataProvider<TConnection> where TConnection : DbConnection, new() where TDataReader : IDataReader {
  public DB2iSeriesDataProvider(Version version) : base(version, typeof(TDataReader)) { }
}

class DB2iSeriesDataProvider<TConnection> : DataProviderBase<TConnection>, IDB2iSeriesDataProvider where TConnection : DbConnection, new() {

  #region Instances

  public static Dictionary<string, IDB2iSeriesDataProvider> Instances = new Dictionary<string, IDB2iSeriesDataProvider>();

  public static DB2iSeriesDataProvider<TConnection> GetInstance(TConnection connection, Type dataReaderType) {
    Instances.TryGetValue(connection.ConnectionString, out var dataProvider);
    if (dataProvider != null) {
      return (DB2iSeriesDataProvider<TConnection>)dataProvider;
    }
    try {
      var isOpen = connection.State == ConnectionState.Open;
      if (!isOpen) connection.Open();
      var dt = connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
      var dataSourceInformationRow = new DataSourceInformationRow(dt);
      var db2IseriesdataProvider = new DB2iSeriesDataProvider<TConnection>(dataSourceInformationRow.Version, dataReaderType);
      Instances[connection.ConnectionString] = db2IseriesdataProvider;
      Console.WriteLine($"{nameof(DB2iSeriesDataProvider<TConnection>)}: {dataSourceInformationRow.GetDataSourceProductNameWithVersion()}");
      if (!isOpen) connection.Close();
      return db2IseriesdataProvider;
    } catch (Exception ex) {
      Console.WriteLine($"{nameof(DB2iSeriesDataProvider<TConnection>)}: {ex.Message}");
      throw ex;
    }
  }

  public static DB2iSeriesDataProvider<TConnection> GetInstance(string connectionString, Type dataReaderType) => GetInstance(new TConnection { ConnectionString = connectionString }, dataReaderType);
  public static DB2iSeriesDataProvider<TConnection> GetInstance<TDataReader>(string connectionString) where TDataReader : IDataReader => GetInstance(connectionString, typeof(TDataReader));
  public static DB2iSeriesDataProvider<TConnection> GetInstance<TDataReader>(IConfiguration configuration, string connectionStringName) where TDataReader : IDataReader => GetInstance(configuration.GetConnectionString(connectionStringName), typeof(TDataReader));
  public static DB2iSeriesDataProvider<TConnection> GetInstance<TDataReader>(TConnection connection) where TDataReader : IDataReader => GetInstance(connection, typeof(TDataReader));

  #endregion

  public DB2iSeriesDataProvider(Version version, Type dataReaderType) : base(
    GetNameWithVersion_DB2iSeries(version),
    new DB2iSeriesMappingSchema(GetNameWithVersion_DB2iSeries(version)),
    dataReaderType
    ) {
    this.version = version;
    //DataSourceInformationRow = dataSourceInformationRow;
    //DataSourceProductName = dataSourceProductName;
    //DataSourceVersion = dbVersion;

    //InitDataProvider();
  }
  Version version;
  static string GetNameWithVersion_DB2iSeries(Version version) => version.GetNameWithVersion(DbSystem.DB2iSeries.Name);
  //protected override ISqlOptimizer SqlOptimizer { get; }

  public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesV5R4SqlBuilder(this, mappingSchema, GetSqlOptimizer(), SqlProviderFlags);
  public override ISqlOptimizer GetSqlOptimizer() => new DB2iSeriesV5R4SqlOptimizer(SqlProviderFlags);
  public override ISchemaProvider GetSchemaProvider() => new DB2iSeriesSchemaProvider();
  public override TableOptions SupportedTableOptions => version.GetTableOptions_DB2iSeries();
}