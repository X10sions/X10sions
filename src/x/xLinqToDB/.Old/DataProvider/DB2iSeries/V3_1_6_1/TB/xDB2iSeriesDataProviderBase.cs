using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.TB {
  public abstract class xDB2iSeriesDataProviderBase<TDbConnection> : DataProviderBase where TDbConnection : IDbConnection, new() {
    public xDB2iSeriesDataProviderBase(xDB2iSeriesDataProviderOptions dataProviderOptions)
      : base(xDB2iSeriesProviderName.ForConnection<TDbConnection>(), xDB2iSeriesMappingSchema.Instance) {
      Options = dataProviderOptions;

      _sqlOptimizer = new xDB2iSeriesSqlOptimizer(SqlProviderFlags);
    }

    private xDB2iSeriesBulkCopy? _bulkCopy;
    private readonly xDB2iSeriesSqlOptimizer _sqlOptimizer;

    public override string? ConnectionNamespace { get; } = typeof(TDbConnection).Namespace;
    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new xDB2iSeriesSqlBuilder<TDbConnection>(this, mappingSchema);
    public abstract Type BinaryType { get; }
    public abstract override Type DataReaderType { get; }
    public bool DoUseNamedParameters => ParameterType == ParameterType.Named;
    //public string DummyTableName => Options.NamingConvention.DummyTableWithSchema();
    public xDB2iSeriesDataProviderOptions Options { get; }
    public abstract ParameterType ParameterType { get; }
    public abstract string GetProviderTypeName(IDbDataParameter parameter);
    public override ISchemaProvider GetSchemaProvider() => new xDB2iSeriesSchemaProvider(Options, typeof(TDbConnection).Namespace);
    public override ISqlOptimizer GetSqlOptimizer() => _sqlOptimizer;

    protected override IDbConnection CreateConnectionInternal(string connectionString) => new TDbConnection {
      ConnectionString = connectionString
    };

 
    public override BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) {
      if (_bulkCopy == null) {
        _bulkCopy = new xDB2iSeriesBulkCopy(Options.NamingConvention);
      }
      return _bulkCopy!.BulkCopy((options.BulkCopyType == BulkCopyType.Default) ? xDB2iSeriesTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source);
    }

    public override Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source, CancellationToken cancellationToken) {
      if (_bulkCopy == null) {
        _bulkCopy = new xDB2iSeriesBulkCopy(Options.NamingConvention);
      }
      return _bulkCopy!.BulkCopyAsync((options.BulkCopyType == BulkCopyType.Default) ? xDB2iSeriesTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source, cancellationToken);
    }

    public override void InitCommand(DataConnection dataConnection, CommandType commandType, string commandText, DataParameter[]? parameters, bool withParameters) {
      dataConnection.DisposeCommand();
      base.InitCommand(dataConnection, commandType, commandText, parameters, withParameters);
    }


  }
}