using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System;
using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider {
  public abstract class _BaseDataProvider<TConnection, TDataReader> : _BaseDataProvider<TConnection>
    where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    public _BaseDataProvider(string name, MappingSchema mappingSchema, ISchemaProvider schemaProvider, TableOptions tableOptions) : base(name, mappingSchema, typeof(TDataReader), schemaProvider, tableOptions) { }
  }

  public abstract class _BaseDataProvider<TConnection> : DataProviderBase
    where TConnection : DbConnection, new() {

    public _BaseDataProvider(string name, MappingSchema mappingSchema, Type dataReaderType, ISchemaProvider schemaProvider, TableOptions tableOptions) : base(name, mappingSchema) {
      ConnectionNamespace = typeof(TConnection).Namespace;
      ConnectionType = typeof(TConnection);
      DataReaderType = dataReaderType;
      SchemaProvider = schemaProvider;
      //SqlOptimizer = sqlOptimizer;
      SupportedTableOptions = tableOptions;
    }

    abstract protected ISqlOptimizer SqlOptimizer { get; }
    protected ISchemaProvider SchemaProvider { get; }
    public Type ConnectionType { get; }

    public override TableOptions SupportedTableOptions { get; }
    public override string? ConnectionNamespace { get; }
    public override Type DataReaderType { get; }
    protected override IDbConnection CreateConnectionInternal(string connectionString) => new TConnection { ConnectionString = connectionString };
    public override ISchemaProvider GetSchemaProvider() => SchemaProvider;
    public override ISqlOptimizer GetSqlOptimizer() => SqlOptimizer;
  }

  public static class _BaseDataProviderExtensions {
  }

}