//using LinqToDB.Common;
//using LinqToDB.Data;
//using LinqToDB.Mapping;
//using LinqToDB.SchemaProvider;
//using LinqToDB.SqlProvider;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Runtime.InteropServices;

//namespace LinqToDB.DataProvider.Access {
//  public abstract class AccessDataProvider_Base : DataProviderBase {
//    public AccessDataProvider_Base()
//      : this(ProviderName.Access, new AccessMappingSchema()) {
//    }

//    protected AccessDataProvider_Base(string name, MappingSchema mappingSchema) : base(name, mappingSchema) {
//      SqlProviderFlags.AcceptsTakeAsParameter = false;
//      SqlProviderFlags.IsSkipSupported = false;
//      SqlProviderFlags.IsCountSubQuerySupported = false;
//      SqlProviderFlags.IsInsertOrUpdateSupported = false;
//      SqlProviderFlags.TakeHintsSupported = TakeHints.Percent;
//      SqlProviderFlags.IsCrossJoinSupported = false;
//      SqlProviderFlags.IsInnerJoinAsCrossSupported = false;
//      SqlProviderFlags.IsDistinctOrderBySupported = false;
//      SqlProviderFlags.IsDistinctSetOperationsSupported = false;
//      // Use positional parameters rather than named parameters;
//      SqlProviderFlags.IsParameterOrderDependent = true;

//      SetCharField("DBTYPE_WCHAR", (r, i) => r.GetString(i).TrimEnd(' '));
//      SetCharFieldToType<char>("DBTYPE_WCHAR", (r, i) => DataTools.GetChar(r, i));

//      SetProviderField<IDataReader, TimeSpan, DateTime>((r, i) => r.GetDateTime(i) - new DateTime(1899, 12, 30));
//      SetProviderField<IDataReader, DateTime, DateTime>((r, i) => GetDateTime(r, i));

//      _sqlOptimizer = new AccessSqlOptimizer(SqlProviderFlags);
//    }

//    static DateTime GetDateTime(IDataReader dr, int idx) {
//      var value = dr.GetDateTime(idx);

//      if (value.Year == 1899 && value.Month == 12 && value.Day == 30)
//        return new DateTime(1, 1, 1, value.Hour, value.Minute, value.Second, value.Millisecond);

//      return value;
//    }
//    public abstract override string ConnectionNamespace { get; }
//    public abstract override Type DataReaderType { get; }

//    protected abstract override IDbConnection CreateConnectionInternal(string connectionString);

//    public abstract override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema);

//    readonly ISqlOptimizer _sqlOptimizer;
//    public override ISqlOptimizer GetSqlOptimizer() => _sqlOptimizer;

//    public abstract override bool IsCompatibleConnection(IDbConnection connection);

//    public abstract override ISchemaProvider GetSchemaProvider();

//    protected abstract override void SetParameterType(IDbDataParameter parameter, DbDataType dataType);
//    protected void base_SetParameterType(IDbDataParameter parameter, DbDataType dataType) => base.SetParameterType(parameter, dataType);

//    [ComImport, Guid("00000602-0000-0010-8000-00AA006D2EA4")] class CatalogClass { }

//    public void CreateDatabase(string databaseName, bool deleteIfExists = false) {
//      if (databaseName == null)
//        throw new ArgumentNullException(nameof(databaseName));
//      databaseName = databaseName.Trim();
//      if (!databaseName.EndsWith(".accdb", StringComparison.OrdinalIgnoreCase))
//        databaseName += ".accdb";
//      if (File.Exists(databaseName)) {
//        if (!deleteIfExists)
//          return;
//        File.Delete(databaseName);
//      }
//      var connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={databaseName};Locale Identifier=1033;Jet OLEDB:Engine Type=5";
//      AccessTools.CreateFileDatabase(databaseName, deleteIfExists, ".accdb", dbName => {
//        dynamic catalog = new CatalogClass();
//        var conn = catalog.Create(connectionString);
//        if (conn != null)
//          conn.Close();
//      });
//    }

//    public void DropDatabase(string databaseName) {
//      if (databaseName == null)
//        throw new ArgumentNullException(nameof(databaseName));
//      AccessTools.DropFileDatabase(databaseName, ".accdb");
//    }

//    public override BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source)
//      => new AccessBulkCopy().BulkCopy(options.BulkCopyType == BulkCopyType.Default ? AccessTools.DefaultBulkCopyType : options.BulkCopyType, table, options, source);

//  }
//}