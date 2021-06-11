//using LinqToDB.Data;
//using LinqToDB.DataProvider;
//using LinqToDB.Mapping;
//using LinqToDB.SchemaProvider;
//using LinqToDB.SqlProvider;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.AS400 {

//  public abstract class AS400DataProviderBase<TDbConnection> : DataProviderBase where TDbConnection : IDbConnection, new() {

//    public AS400DataProviderBase(DB2iSeriesDataProviderOptions dataProviderOptions)
//      //IsMapGuidAsString : base(DB2iSeriesProviderName.ForConnection<TDbConnection>(), DB2iSeriesMappingSchema.GetInstance(dataProviderOptions.IsMapGuidAsString)) {
//      : base(DB2iSeriesProviderName.ForConnection<TDbConnection>(), DB2iSeriesMappingSchema.Instance) {
//      Options = dataProviderOptions;
//      //IsMapGuidAsString DB2iSeriesExpressions.MapMembers(Name, dataProviderOptions.IsMapGuidAsString);
//      DB2iSeriesExpressions.MapMembers(Name);
//      SetSqlProviderFlags();
//      MappingSchema_AddScalarTypes();
//      SetCharField("CHAR", (r, i) => r.GetString(i).TrimEnd());
//      //SetCharFieldToType<char>("CHAR", (r, i) => DataTools.GetChar(r, i));
//      _sqlOptimizer = new DB2iSeriesSqlOptimizer(SqlProviderFlags);
//      if (DataConnection.TraceSwitch.TraceInfo) {
//        DataConnection.WriteTraceLine(DataReaderType.Assembly.FullName, DataConnection.TraceSwitch.DisplayName, TraceLevel.Info);
//      }
//      LinqToDB.DataProvider.DB2iSeries.sqlop
//      //var conn = new TConnection ();
//      //var cmd = conn.CreateCommand()        ;
//      //var param = cmd.CreateParameter();

//    }
//    readonly DB2iSeriesSqlOptimizer _sqlOptimizer;
//    public DB2iSeriesDataProviderOptions Options { get; }

//    //https://github.com/shiningrise/MidExam/blob/master/src/Wxy.CodeGen/Settings/DbTargets.xml
//    public abstract void MappingSchema_AddScalarTypes();

//    public override string? ConnectionNamespace { get; } = typeof(TDbConnection).Namespace;
//    public abstract override Type DataReaderType { get; }
//    public abstract Type BinaryType { get; }
//    public abstract ParameterType ParameterType { get; }
//    public bool DoUseNamedParameters => ParameterType == ParameterType.Named;

//    public abstract string GetProviderTypeName(IDbDataParameter parameter);

//    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesSqlBuilder<TDbConnection>(this, mappingSchema);
//    public override ISchemaProvider GetSchemaProvider() => new DB2iSeriesSchemaProvider(Options, typeof(TDbConnection).Namespace);
//    public override ISqlOptimizer GetSqlOptimizer() => _sqlOptimizer;
//    protected override IDbConnection CreateConnectionInternal(string connectionString) => new TDbConnection { ConnectionString = connectionString };

//    void SetSqlProviderFlags() {
//      SqlProviderFlags.AcceptsTakeAsParameter = false;
//      SqlProviderFlags.AcceptsTakeAsParameterIfSkip = true;
//      SqlProviderFlags.CanCombineParameters = false;
//      SqlProviderFlags.IsCommonTableExpressionsSupported = true;
//      SqlProviderFlags.IsDistinctOrderBySupported = true;
//      //SqlProviderFlags.IsParameterOrderDependent = ParameterType == ParameterType.Positional;
//      //SqlProviderFlags.IsUpdateFromSupported = false;
//      //IsMapGuidAsString if (Options.IsMapGuidAsString)
//      //IsMapGuidAsString  SqlProviderFlags.CustomFlags.Add(DB2iSeriesTools.MapGuidAsString);
//    }

//    public string DummyTableName => DB2iSeriesTools.iSeriesDummyTableName();
//    DB2iSeriesBulkCopy? _bulkCopy;

//    public override BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) {
//      if (_bulkCopy == null)
//        //IsMapGuidAsString _bulkCopy = new DB2iSeriesBulkCopy(Options.IsMapGuidAsString);
//        _bulkCopy = new DB2iSeriesBulkCopy();
//      return _bulkCopy.BulkCopy(options.BulkCopyType == BulkCopyType.Default ? DB2iSeriesTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source);
//    }

//    public override Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source, CancellationToken cancellationToken) {
//      if (_bulkCopy == null)
//        //IsMapGuidAsString _bulkCopy = new DB2iSeriesBulkCopy(Options.IsMapGuidAsString);
//        _bulkCopy = new DB2iSeriesBulkCopy();
//      return _bulkCopy.BulkCopyAsync(options.BulkCopyType == BulkCopyType.Default ? DB2iSeriesTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source, cancellationToken);
//    }

//    public override void InitCommand(DataConnection dataConnection, CommandType commandType, string commandText, DataParameter[]? parameters, bool withParameters) {
//      dataConnection.DisposeCommand();
//      base.InitCommand(dataConnection, commandType, commandText, parameters, withParameters);
//    }

//  }
//}