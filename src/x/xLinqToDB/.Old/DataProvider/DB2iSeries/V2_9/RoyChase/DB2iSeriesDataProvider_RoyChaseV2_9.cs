using IBM.Data.DB2.iSeries;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Linq;
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

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9.RoyChase{
  public class DB2iSeriesDataProvider_RoyChaseV2_9 : DynamicDataProviderBase {
    DB2iSeriesConfiguration db2iSeriesConfiguration;
    public DB2iSeriesDataProvider_RoyChaseV2_9() : this(new DB2iSeriesConfiguration()) { }
    public DB2iSeriesDataProvider_RoyChaseV2_9(DB2iSeriesConfiguration db2iSeriesConfiguration) : base(db2iSeriesConfiguration.Provider.Name(), db2iSeriesConfiguration.MappingSchema) {
      this.db2iSeriesConfiguration = db2iSeriesConfiguration;
      db2iSeriesConfiguration.InitDataProvider(this, SetCharField);
    }
    public override string ConnectionNamespace { get; } = string.Empty;
    protected override string ConnectionTypeName { get; } = typeof(iDB2Connection).AssemblyQualifiedName;//DB2iSeriesTools.ConnectionTypeName;
    protected override string DataReaderTypeName { get; } = typeof(iDB2DataReader).AssemblyQualifiedName;//DB2iSeriesTools.DataReaderTypeName;
    public override BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) => table.BulkCopy_DB2iSeries(options, source, db2iSeriesConfiguration);
    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesSqlBuilder_IBM(GetSqlOptimizer(), SqlProviderFlags, mappingSchema.ValueToSqlConverter, db2iSeriesConfiguration);
    public override ISchemaProvider GetSchemaProvider() => new DB2iSeriesSchemaProvider_IBM();
    public override ISqlOptimizer GetSqlOptimizer() => new DB2iSeriesSqlOptimizer(SqlProviderFlags);
    public override void InitCommand(DataConnection dataConnection, CommandType commandType, string commandText, DataParameter[] parameters, bool withParameters) => this.InitCommand_DB2iSeries(dataConnection, commandType, commandText, parameters, withParameters);
    public override bool IsCompatibleConnection(IDbConnection connection) => typeof(iDB2Connection).IsSameOrParentOf(Proxy.GetUnderlyingObject((DbConnection)connection).GetType());
    protected override void OnConnectionTypeCreated(Type connectionType) => this.OnConnectionTypeCreated_DB2iSeries(connectionType, baseSetProviderField, baseGetSetParameter);
    public override void SetParameter(IDbDataParameter parameter, string name, DbDataType dataType, object value) => parameter.SetParameter_DB2iSeries(db2iSeriesConfiguration.MapGuidAsString, name, dataType, value, base.SetParameter);
    void baseSetProviderField(Type toType, Type fieldType, string methodName) => SetProviderField(toType, fieldType, methodName, true);
    Action<IDbDataParameter> baseGetSetParameter(Type connectionType) => GetSetParameter(connectionType, nameof(iDB2Parameter), nameof(iDB2DbType), nameof(iDB2DbType), nameof(iDB2Blob));

    #region Merge
    public override int Merge<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName) => dataConnection.Merge_DB2iSeries(deletePredicate, delete, source, tableName, databaseName, schemaName);
    public override Task<int> MergeAsync<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName, CancellationToken token) => dataConnection.MergeAsync_DB2iSeries(deletePredicate, delete, source, tableName, databaseName, schemaName, token);
    protected override BasicMergeBuilder<TTarget, TSource> GetMergeBuilder<TTarget, TSource>(DataConnection connection, IMergeable<TTarget, TSource> merge) => new DB2iSeriesMergeBuilder<TTarget, TSource>(connection, merge);
    #endregion

  }
}