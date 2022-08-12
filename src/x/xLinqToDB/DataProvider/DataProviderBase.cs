using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider {
  public abstract class DataProviderBase<TConnection, TDataReader> : DataProviderBase<TConnection>
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    public DataProviderBase(string name, MappingSchema mappingSchema) : base(name, mappingSchema, typeof(TDataReader)) {
      //, Func<ISchemaProvider> getSchemaProvider, TableOptions tableOptions
      //, getSchemaProvider, tableOptions
    }
  }

  public abstract class DataProviderBase<TConnection> : DataProviderBase
    where TConnection : DbConnection, new() {

    public DataProviderBase(string name, MappingSchema mappingSchema, Type dataReaderType) : base(name, mappingSchema) {
      //, Func<ISchemaProvider> getSchemaProvider, TableOptions tableOptions
      ConnectionNamespace = typeof(TConnection).Namespace;
      ConnectionType = typeof(TConnection);
      DataReaderType = dataReaderType;
      //this.getSchemaProvider = getSchemaProvider;
      //SchemaProvider = schemaProvider;
      //SqlOptimizer = sqlOptimizer;
      //SupportedTableOptions = tableOptions;
    }
    //abstract protected ISqlOptimizer SqlOptimizer { get; }
    //Func<ISchemaProvider> getSchemaProvider;
    //protected ISchemaProvider SchemaProvider { get; }
    public Type ConnectionType { get; }

    public override string? ConnectionNamespace { get; }
    public override Type DataReaderType { get; }
    //public override TableOptions SupportedTableOptions { get; }
    //public abstract override TableOptions SupportedTableOptions { get; }
    protected override DbConnection CreateConnectionInternal(string connectionString) => new TConnection { ConnectionString = connectionString };
    //public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => throw new NotImplementedException();
    //public override ISchemaProvider GetSchemaProvider() => getSchemaProvider();
    //public override ISqlOptimizer GetSqlOptimizer() => SqlOptimizer;
  }

}