﻿using Common.Data;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace LinqToDB.DataProvider {
  public abstract class WrappedDataProvider<TConn, TDataReader> : IDataProvider where TConn : DbConnection, new() where TDataReader : DbDataReader {

    //static Dictionary<string, IDataProvider> Instances = new Dictionary<string, IDataProvider>();

    //public static TDataProvider GetInstance<TDataProvider>(DbSystem dbSystem, IDataProvider baseDataProvider)
    //  where TDataProvider : WrappedDataProvider<TConn, TDataReader>, new() {

    //  var key = $"{dbSystem.Name}:{typeof(TConn).Name}:{baseDataProvider.ID}";
    //  var exists = Instances.TryGetValue(key, out IDataProvider instance);
    //  if (!exists) {
    //    var typedInstance = new WrappedDataProvider<TConn, TDataReader>(dbSystem, 900 + Instances.Count, baseDataProvider);
    //    Instances[key] = typedInstance;
    //  }
    //  return (TDataProvider)instance;
    //}

    protected WrappedDataProvider(DbSystem dbSystem, int id, IDataProvider baseDataProvider) {
      Name = GetProviderName(dbSystem);
      ID = id;
      this.baseDataProvider = baseDataProvider;
      //mappingSchema = baseDataProvider.MappingSchema;
      mappingSchema = new MappingSchema(baseDataProvider.MappingSchema);
      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.VarChar, typeof(string), 255));
    }

    protected static string GetProviderName(DbSystem dbSystem) => $"{typeof(TConn).Namespace}.{dbSystem.Name}";

    IDataProvider baseDataProvider;
    MappingSchema mappingSchema;
    public string Name { get; }
    public int ID { get; }// => _id ??= new IdentifierBuilder(Name).CreateID();
    public string? ConnectionNamespace => typeof(TConn).Namespace;
    public Type DataReaderType => typeof(TDataReader);
    public MappingSchema MappingSchema => mappingSchema;
    public SqlProviderFlags SqlProviderFlags => baseDataProvider.SqlProviderFlags;
    public TableOptions SupportedTableOptions => baseDataProvider.SupportedTableOptions;
    public bool TransactionsSupported => baseDataProvider.TransactionsSupported;
    public BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) where T : notnull => baseDataProvider.BulkCopy(table, options, source);
    public async Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source, CancellationToken cancellationToken) where T : notnull => await baseDataProvider.BulkCopyAsync(table, options, source, cancellationToken);
    public async Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IAsyncEnumerable<T> source, CancellationToken cancellationToken) where T : notnull => await baseDataProvider.BulkCopyAsync(table, options, source, cancellationToken);
    public Type ConvertParameterType(Type type, DbDataType dataType) => baseDataProvider.ConvertParameterType(type, dataType);
    public DbConnection CreateConnection(string connectionString) => new TConn() { ConnectionString = connectionString };
    public ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => baseDataProvider.CreateSqlBuilder(mappingSchema);
    public void DisposeCommand(DbCommand command) => baseDataProvider.DisposeCommand(command);
#if NETSTANDARD2_1PLUS
		public async ValueTask DisposeCommandAsync(DbCommand command) => await baseDataProvider.DisposeCommandAsync(command);
#else
    public ValueTask DisposeCommandAsync(DbCommand command) {
      baseDataProvider.DisposeCommand(command);
      return new ValueTask(Task.CompletedTask);
    }
#endif
    public IExecutionScope? ExecuteScope(DataConnection dataConnection) => baseDataProvider.ExecuteScope(dataConnection);
    public CommandBehavior GetCommandBehavior(CommandBehavior commandBehavior) => baseDataProvider.GetCommandBehavior(commandBehavior);
    public object? GetConnectionInfo(DataConnection dataConnection, string parameterName) => baseDataProvider.GetConnectionInfo(dataConnection, parameterName);
    public Expression GetReaderExpression(DbDataReader reader, int idx, Expression readerExpression, Type toType) => baseDataProvider.GetReaderExpression(reader, idx, readerExpression, toType);
    public ISchemaProvider GetSchemaProvider() => baseDataProvider.GetSchemaProvider();
    public ISqlOptimizer GetSqlOptimizer() => baseDataProvider.GetSqlOptimizer();
    public DbCommand InitCommand(DataConnection dataConnection, DbCommand command, CommandType commandType, string commandText, DataParameter[]? parameters, bool withParameters) => baseDataProvider.InitCommand(dataConnection, command, commandType, commandText, parameters, withParameters);
    public void InitContext(IDataContext dataContext) => baseDataProvider.InitContext(dataContext);
    public bool? IsDBNullAllowed(DbDataReader reader, int idx) => baseDataProvider.IsDBNullAllowed(reader, idx);
    public void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object? value) => baseDataProvider.SetParameter(dataConnection, parameter, name, dataType, value);

  }
}
