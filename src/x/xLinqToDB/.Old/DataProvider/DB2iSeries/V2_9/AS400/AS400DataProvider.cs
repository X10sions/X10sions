using Common;
using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.DataProvider;
using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB.Linq;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.AS400 {
  [IsCustom(IsCustomReason.ThirdPartyExtension)]
  public class AS400DataProvider : DataProviderBase {
    DB2iSeriesConfiguration dB2ISeriesConfiguration;

    public AS400DataProvider(DB2iSeriesVersionRelease version = DB2iSeriesVersionRelease.V5R4) : this(new DB2iSeriesConfiguration {
      Version = version
    }) { }

    public AS400DataProvider(DB2iSeriesConfiguration dB2ISeriesConfiguration) : base(dB2ISeriesConfiguration.Provider.Name(), dB2ISeriesConfiguration.MappingSchema) {
      this.dB2ISeriesConfiguration = dB2ISeriesConfiguration;
      dB2ISeriesConfiguration.InitDataProvider(this, SetCharField);
      SqlProviderFlags.IsDistinctOrderBySupported = true;
      SetCharFieldToType<char>("CHAR", (r, i) => DataTools.GetChar(r, i));
    }

    public override string ConnectionNamespace => typeof(iDB2Connection).Namespace;
    public override Type DataReaderType => typeof(iDB2DataReader);
    public override BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) => table.BulkCopy_DB2iSeries(options, source, dB2ISeriesConfiguration);
    protected override IDbConnection CreateConnectionInternal(string connectionString) => new iDB2Connection(connectionString);
    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new AS400SqlBuilder(GetSqlOptimizer(), SqlProviderFlags, MappingSchema.ValueToSqlConverter);
    public override ISchemaProvider GetSchemaProvider() => new AS400SchemaProvider();
    public override ISqlOptimizer GetSqlOptimizer() => new AS400SqlOptimizer(SqlProviderFlags);
    public override bool IsCompatibleConnection(IDbConnection connection) => ReflectionExtensions.IsSameOrParentOf(typeof(iDB2Connection), Proxy.GetUnderlyingObject((DbConnection)connection).GetType());
    public override void SetParameter(DataConnection dataConnection, IDbDataParameter parameter, string name, DbDataType dataType, object value) => DB2iSeries.DB2iSeriesExtensions_IBM.SetParameter_DB2iSeries(parameter, dB2ISeriesConfiguration.MapGuidAsString, name, dataType, value, base.SetParameter);
    public override int Merge<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName) => DB2iSeries.DB2iSeriesExtensions.Merge_DB2iSeries(dataConnection, deletePredicate, delete, source, tableName, databaseName, schemaName);
    public override Task<int> MergeAsync<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName, CancellationToken token) => DB2iSeries.DB2iSeriesExtensions.MergeAsync_DB2iSeries(dataConnection, deletePredicate, delete, source, tableName, databaseName, schemaName, token);
    protected override BasicMergeBuilder<TTarget, TSource> GetMergeBuilder<TTarget, TSource>(DataConnection connection, IMergeable<TTarget, TSource> merge) => new DB2iSeries.DB2iSeriesMergeBuilder<TTarget, TSource>(connection, merge);

  }
}