﻿using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Expressions;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;
using LinqToDB.SqlProvider;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using static LinqToDB.Linq.Expressions;

namespace LinqToDB.DataProvider;

public interface IGenericDataProvider : IDataProvider {
  //string ConnectionString { get; }
  DataSourceInformationRow DataSourceInformationRow { get; }
}

//public class GenericDataProvider<TConnection, TDataReader> : GenericDataProvider<TConnection> where TConnection : DbConnection, new() where TDataReader : IDataReader {
//  public GenericDataProvider(DataSourceInformationRow dataSourceInformationRow) : base(dataSourceInformationRow, typeof(TDataReader)) { }
//  //public GenericDataProvider(TConnection connection) : base(connection, typeof(TDataReader)) { }
//  //public GenericDataProvider(string connectionString) : base(connectionString, typeof(TDataReader)) { }

//  //public GenericDataProvider(TConnection connection) {
//  //  return GetInstance<TConnection, TDataReader>(connection.ConnectionString);
//  //}
//}

public class GenericDataProvider<TConnection> : DataProviderBase<TConnection>, IGenericDataProvider where TConnection : DbConnection, new() {

  // https://docs.envobi.com/articles/adb/adb-supported-databases.html

  #region Instances

  public static Dictionary<string, IGenericDataProvider> Instances = new Dictionary<string, IGenericDataProvider>();

  //public static GenericDataProvider<TConnection> GetInstance(string connectionString, DataSourceInformationRow dataSourceInformationRow, Type dataReaderType) {
  //  GenericDataProvider<TConnection> genericDataProvider;
  //  Instances.TryGetValue(connectionString, out var dataProvider);
  //  if (dataProvider == null) {
  //    genericDataProvider = new GenericDataProvider<TConnection>(dataSourceInformationRow, dataReaderType);
  //    Instances[connectionString] = genericDataProvider;
  //    return genericDataProvider;
  //  }
  //  return (GenericDataProvider<TConnection>)dataProvider;
  //}

  public static GenericDataProvider<TConnection> GetInstance(TConnection connection, Type dataReaderType) {
    var connectionStringWithoutPasswordOrUser = connection.ConnectionStringWithoutPasswordOrUser();
    Instances.TryGetValue(connectionStringWithoutPasswordOrUser, out var dataProvider);
    if (dataProvider != null) {
      return (GenericDataProvider<TConnection>)dataProvider;
    }
    var dataSourceInformationRow = connection.GetDataSourceInformationRow();
    if (dataSourceInformationRow != null) {
      var genericDataProvider = new GenericDataProvider<TConnection>(dataSourceInformationRow, dataReaderType);
      Instances[connectionStringWithoutPasswordOrUser] = genericDataProvider;
      Console.WriteLine($"{nameof(GenericDataProvider<TConnection>)}: {connectionStringWithoutPasswordOrUser}");
      return genericDataProvider;
    }
    throw new NotImplementedException(connectionStringWithoutPasswordOrUser);
  }

  public static GenericDataProvider<TConnection> GetInstance(string connectionString, Type dataReaderType) => GetInstance(new TConnection { ConnectionString = connectionString }, dataReaderType);
  public static GenericDataProvider<TConnection> GetInstance<TDataReader>(string connectionString) where TDataReader : IDataReader => GetInstance(connectionString, typeof(TDataReader));
  //public static GenericDataProvider<TConnection> GetInstance<TDataReader>(IConfiguration configuration, string connectionStringName) where TDataReader : IDataReader => GetInstance(configuration.GetConnectionString(connectionStringName), typeof(TDataReader));
  public static GenericDataProvider<TConnection> GetInstance<TDataReader>(TConnection connection) where TDataReader : IDataReader => GetInstance(connection, typeof(TDataReader));

  #endregion

  #region Contexts

  public static TContext GetDataContext<TContext, TDataReader>(string connectionString, DataOptions dataOptions, Func<DataOptions, TContext> newContext)
    where TContext : IDataContext
    //where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    using (var connection = new TConnection { ConnectionString = connectionString }) {
      return GetDataContext<TContext, TDataReader>(connection, dataOptions, newContext);
    }
  }

  public static TContext GetDataContext<TContext, TDataReader>(TConnection connection, Func<IDataProvider, string, TContext> newContext)
    where TContext : IDataContext
    //where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    try {
      if (connection.State != ConnectionState.Open) { connection.Open(); }
      var genericDataProvider = GenericDataProvider<TConnection>.GetInstance<TDataReader>(connection);
      if (genericDataProvider != null) {
        Console.WriteLine($"{nameof(GetDataContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
        return newContext(genericDataProvider, connection.ConnectionString);
      }
      if (connection.State == ConnectionState.Open) { connection.Close(); }
      Console.WriteLine($" GenericDataProvider: {genericDataProvider?.DataSourceInformationRow.GetDataSourceProductNameWithVersion()}");
    } catch (Exception ex) {
      throw new Exception(ex.Message);
    }
    throw new Exception("Not Implementation");
  }

  public static TContext GetDataContext<TContext, TDataReader>(TConnection connection, DataOptions dataOptions, Func<DataOptions, TContext> newContext)
    where TContext : IDataContext
    //where TConnection : DbConnection, new()
    where TDataReader : IDataReader {
    try {
      if (connection.State != ConnectionState.Open) { connection.Open(); }
      var genericDataProvider = GenericDataProvider<TConnection>.GetInstance<TDataReader>(connection);
      if (genericDataProvider != null) {
        Console.WriteLine($"{nameof(GetDataContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
        //var builder = new LinqToDBConnectionOptionsBuilder();
        var options = dataOptions.UseConnectionString(genericDataProvider, connection.ConnectionString);
        //var options = builder.Build<TContext>();
        return newContext(options);
      }
      if (connection.State == ConnectionState.Open) { connection.Close(); }
      Console.WriteLine($" GenericDataProvider: {genericDataProvider?.DataSourceInformationRow.GetDataSourceProductNameWithVersion()}");
    } catch (Exception ex) {
      throw new Exception(ex.Message);
    }
    throw new Exception("Not Implementation");
  }

  #endregion

  protected GenericDataProvider(DataSourceInformationRow dataSourceInformationRow, Type dataReaderType) : base(
    $"{dataSourceInformationRow.GetDataSourceProductNameWithVersion()}",
    //$"{typeof(TConnection).FullName}.{connectionString}",
    GenericMappingSchema.GetInstance(dataSourceInformationRow),
    dataReaderType
    ) {
    //ConnectionString = connectionString;
    DataSourceInformationRow = dataSourceInformationRow;
    DbSystem = DataSourceInformationRow?.GetDbSystem();
    //InitDataProvider();
    var initDbSystem = DbSystem switch {
      //DbSystem.Names.Access => GenericDataProvider_InitAccess(),
      //DbSystem.Names.DB2 => GenericDataProvider_InitDB2(),
      var _ when DbSystem == DbSystem.DB2iSeries => GenericDataProvider_InitDB2iSeries(DataSourceInformationRow.Version),
      _ => throw new NotImplementedException($"{DataSourceInformationRow.DataSourceProductName}: v{DataSourceInformationRow.Version}")
    };
    foreach (var member in MemberExpressions) {
      MapMember(Name, member.Key.MemberInfo, member.Value);
    }

    if (DataConnection.TraceSwitch.TraceInfo) {
      DataConnection.WriteTraceLine(DataReaderType.Assembly.FullName, DataConnection.TraceSwitch.DisplayName, TraceLevel.Info);
    }
  }



  Dictionary<MemberHelper.MemberInfoWithType, IExpressionInfo> MemberExpressions = new Dictionary<MemberHelper.MemberInfoWithType, IExpressionInfo>();
  //public TConnection Connection { get; }
  //public string ConnectionString { get; }
  //public string ConnectionString => Connection.ConnectionString;
  public DataSourceInformationRow? DataSourceInformationRow { get; }// => DataSourceInformationRow.GetInstance<TConnection>(ConnectionString);
  public DbSystem? DbSystem { get; }
  //public Version? DbSystemVrsion => DataSourceInformationRow.Version;
  public override ISchemaProvider GetSchemaProvider() => new GenericSchemaProvider();
  public override TableOptions SupportedTableOptions => DataSourceInformationRow.GetTableOptions();



  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public bool GenericDataProvider_InitAccess() {
    SqlProviderFlags.AcceptsTakeAsParameter = false;
    SqlProviderFlags.IsSkipSupported = false;
    SqlProviderFlags.IsCountSubQuerySupported = false;
    SqlProviderFlags.IsInsertOrUpdateSupported = false;
    SqlProviderFlags.TakeHintsSupported = TakeHints.Percent;
    SqlProviderFlags.IsCrossJoinSupported = false;
    SqlProviderFlags.IsInnerJoinAsCrossSupported = false;
    SqlProviderFlags.IsDistinctOrderBySupported = false;
    SqlProviderFlags.IsDistinctSetOperationsSupported = false;
    SqlProviderFlags.IsParameterOrderDependent = true;
    SqlProviderFlags.IsUpdateFromSupported = false;
    SqlProviderFlags.DefaultMultiQueryIsolationLevel = IsolationLevel.Unspecified;
    xLog.Debug($"DbProvider.Namespaces.System_Data_Odbc: {DbProviderNamespace.System_Data_Odbc}");

    if (base.ConnectionNamespace == DbProviderNamespace.System_Data_Odbc.Name) {
      SetCharField("CHAR", (r, i) => r.GetString(i).TrimEnd(' '));
      SetCharFieldToType<char>("CHAR", DataTools.GetCharExpression);

      SetProviderField<IDataReader, TimeSpan, DateTime>((r, i) => r.GetDateTime(i) - new DateTime(1899, 12, 30));

      SetToType<IDataReader, sbyte, int>("INTEGER", (r, i) => unchecked((sbyte)r.GetInt32(i)));
      SetToType<IDataReader, uint, int>("INTEGER", (r, i) => unchecked((uint)r.GetInt32(i)));
      SetToType<IDataReader, ulong, int>("INTEGER", (r, i) => unchecked((uint)r.GetInt32(i)));
      SetToType<IDataReader, ushort, short>("SMALLINT", (r, i) => unchecked((ushort)r.GetInt16(i)));
    } else if (base.ConnectionNamespace == DbProviderNamespace.System_Data_OleDb.Name) {
      SetCharField("DBTYPE_WCHAR", (r, i) => r.GetString(i).TrimEnd(' '));
      SetCharFieldToType<char>("DBTYPE_WCHAR", DataTools.GetCharExpression);

      SetProviderField<IDataReader, TimeSpan, DateTime>((r, i) => r.GetDateTime(i) - new DateTime(1899, 12, 30));
    } else {
    }
    return true;
  }

  [UrlAsAt.DB2DataProvider_2021_03_14]
  public bool GenericDataProvider_InitDB2() {
    SqlProviderFlags.AcceptsTakeAsParameter = false;
    SqlProviderFlags.AcceptsTakeAsParameterIfSkip = true;
    SqlProviderFlags.IsDistinctOrderBySupported = false;
    SqlProviderFlags.IsCommonTableExpressionsSupported = true;
    SqlProviderFlags.IsUpdateFromSupported = false;

    SetCharFieldToType<char>("CHAR", DataTools.GetCharExpression);
    SetCharField("CHAR", (r, i) => r.GetString(i).TrimEnd(' '));

    //SetProviderField(Adapter.DB2Int64Type, typeof(long), Adapter.GetDB2Int64ReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2Int32Type, typeof(int), Adapter.GetDB2Int32ReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2Int16Type, typeof(short), Adapter.GetDB2Int16ReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2DecimalType, typeof(decimal), Adapter.GetDB2DecimalReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2DecimalFloatType, typeof(decimal), Adapter.GetDB2DecimalFloatReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2RealType, typeof(float), Adapter.GetDB2RealReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2Real370Type, typeof(float), Adapter.GetDB2Real370ReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2DoubleType, typeof(double), Adapter.GetDB2DoubleReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2StringType, typeof(string), Adapter.GetDB2StringReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2ClobType, typeof(string), Adapter.GetDB2ClobReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2BinaryType, typeof(byte[]), Adapter.GetDB2BinaryReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2BlobType, typeof(byte[]), Adapter.GetDB2BlobReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2DateType, typeof(DateTime), Adapter.GetDB2DateReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2TimeType, typeof(TimeSpan), Adapter.GetDB2TimeReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2TimeStampType, typeof(DateTime), Adapter.GetDB2TimeStampReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2XmlType, typeof(string), Adapter.GetDB2XmlReaderMethod, dataReaderType: Adapter.DataReaderType);
    //SetProviderField(Adapter.DB2RowIdType, typeof(byte[]), Adapter.GetDB2RowIdReaderMethod, dataReaderType: Adapter.DataReaderType);

    //if (Adapter.DB2DateTimeType != null)
    //  SetProviderField(Adapter.DB2DateTimeType, typeof(DateTime), Adapter.GetDB2DateTimeReaderMethod!, dataReaderType: Adapter.DataReaderType);

    return true;
  }

  public bool GenericDataProvider_InitDB2iSeries(Version? version) {
    MemberExpressions.Add(M(() => Sql.ConvertTo<string>.From(0)), N(() => L((decimal p) => Sql.TrimLeft(Sql.Convert<string, decimal>(p), '0'))));
    MemberExpressions.Add(M(() => Sql.ConvertTo<string>.From(decimal.Zero)), N(() => L((decimal p) => Sql.TrimLeft(Sql.Convert<string, decimal>(p), '0'))));
    MemberExpressions.Add(M(() => Sql.ConvertTo<string>.From(Guid.Empty)), N(() => L((Guid p) => Sql.Lower(Sql.Substring(Hex(p), 7, 2) + Sql.Substring(Hex(p), 5, 2) + Sql.Substring(Hex(p), 3, 2) + Sql.Substring(Hex(p), 1, 2) + "-" + Sql.Substring(Hex(p), 11, 2) + Sql.Substring(Hex(p), 9, 2) + "-" + Sql.Substring(Hex(p), 15, 2) + Sql.Substring(Hex(p), 13, 2) + "-" + Sql.Substring(Hex(p), 17, 4) + "-" + Sql.Substring(Hex(p), 21, 12)))));

    MemberExpressions.Add(M(() => Sql.Log(0m, 0m)), N(() => L((decimal? m, decimal? n) => Sql.Log(n) / Sql.Log(m))));
    MemberExpressions.Add(M(() => Sql.Log(0.0, 0.0)), N(() => L((double? m, double? n) => Sql.Log(n) / Sql.Log(m))));
    MemberExpressions.Add(M(() => Sql.PadLeft("", 0, ' ')), N(() => L((string p0, int? p1, char? p2) => (p0.Length > p1) ? p0 : VarChar(Replicate(p2, p1 - p0.Length), 1000) + p0)));
    MemberExpressions.Add(M(() => Sql.PadRight("", 0, ' ')), N(() => L((string p0, int? p1, char? p2) => (p0.Length > p1) ? p0 : p0 + VarChar(Replicate(p2, p1 - p0.Length), 1000))));
    MemberExpressions.Add(M(() => Sql.Space(0)), N(() => L((int? p0) => Sql.Convert(Sql.Types.VarChar(1000), Replicate(" ", p0)))));
    MemberExpressions.Add(M(() => Sql.Stuff("", 0, 0, "")), N(() => L((string p0, int? p1, int? p2, string p3) => AltStuff(p0, p1, p2, p3))));

    //  MemberExpressions.Add(M(() => string.IsNullOrWhiteSpace("")), N(() => L((string s) => string.IsNullOrWhiteSpace(s))));
    //  MemberExpressions.Add(M(() => string.IsNullOrWhiteSpace("")), N(() => L((string s) => s == null || s.TrimEnd() == string.Empty)));
    //  MemberExpressions.Add(M(() => "".ToLowerInvariant()), N(() => L((string s) => "".ToLower())));
    //  MemberExpressions.Add(M(() => "".ToUpperInvariant()), N(() => L((string s) => "".ToUpper())));

    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Day, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Day, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.DayOfYear, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.DayOfYear, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.DayOfWeek, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.WeekDay, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Hour, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Hour, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Millisecond, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Millisecond, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Minute, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Minute, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Month, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Month, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Second, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Second, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) d.Year, Function(d As Date) DB2iSeriesSql.DatePart(Sql.DateParts.Year, d).Value)

    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddDays(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Day, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddHours(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Hour, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddMilliseconds(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Millisecond, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddMinutes(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Minute, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddMonths(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Month, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddSeconds(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Second, n, d).Value)
    //20170904      Linq.Expressions.MapMember(Function(d As Date, n As Double) d.AddYears(n), Function(d As Date, n As Double) DB2iSeriesSql.DateAdd(Sql.DateParts.Year, n, d).Value)

    //20170904      Linq.Expressions.MapMember(Function(d As Date) Day(d), Function(d As Date) d.Day)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) Month(d), Function(d As Date) d.Month)
    //20170904      Linq.Expressions.MapMember(Function(d As Date) Year(d), Function(d As Date) d.Year)

    //Fix Cannot be converted to SQL
    //20170904      Linq.Expressions.MapMember(Function(s As String, l As Integer) Left(s, l), Function(s, l) Sql.Left(s, l))
    //20170904      Linq.Expressions.MapMember(Function(s As String, si As Integer?, l As Integer?) Mid(s, si, l), Function(s, si, l) Sql.Substring(s, si, l))
    //20170904      Linq.Expressions.MapMember(Function(s As String, l As Integer) Right(s, l), Function(s, l) Sql.Right(s, l))


    //      Linq.Expressions.MapMember(Function(s As String) s.Trim(), Function(s As String) DB2iSeriesSql._Trim(s))
    //      Linq.Expressions.MapMember(Function(s As String, chars As Char()) s.Trim(chars), Function(s As String, chars As Char()) DB2iSeriesSql._Trim(s, chars(0)))
    //      Linq.Expressions.MapMember(Function(s As String) s.TrimStart(), Function(s As String) DB2iSeriesSql._TrimLeading(s))
    //      Linq.Expressions.MapMember(Function(s As String, chars As Char()) s.TrimStart(chars), Function(s As String, chars As Char()) DB2iSeriesSql.PadRight(Sql.DateParts.Year, n, d).Value)
    //      Linq.Expressions.MapMember(Function(s As String) s.TrimEnd(), Function(s As String) DB2iSeriesSql._TrimTrailing(s))

    //      Linq.Expressions.MapMember(Function(s As String, ch As Char()) s.TrimEnd(ch), Function(s, ch) DB2iSeriesSql._TrimTrailing(ch, s))
    //      Linq.Expressions.MapMember(Function(s As String, ch As Char()) s.TrimStart(ch), Function(s, ch) DB2iSeriesSql._TrimLeading(ch, s))

    //      Linq.Expressions.MapMember(Function(s As String) String.IsNullOrWhiteSpace(s), Function(s) s Is Nothing OrElse s.TrimEnd() = String.Empty)

    //      Linq.Expressions.MapMember(Function(s As String, chars As Char()) s.TrimEnd(chars), Function(s As String, chars As Char()) DB2iSeriesSql.PadRight(Sql.DateParts.Year, n, d).Value)


    //Linq.Expressions.MapMember(Function(a As String, b As String, c As StringComparison) a.Equals(b, c), Function(a, b, c) a.ToUpper = b.ToUpper)

    //SetGenericInfoProvider(GetType(GenericInfoProvider(Of )))
    //Linq.Expressions.SetGenericInfoProvider(GetType(Linq2DB.Linq.xExpressions.GenericInfoProvider(Of )))

    SqlProviderFlags.AcceptsTakeAsParameter = false;
    SqlProviderFlags.AcceptsTakeAsParameterIfSkip = true;
    SqlProviderFlags.CanCombineParameters = false;
    SqlProviderFlags.IsCommonTableExpressionsSupported = true;
    SqlProviderFlags.IsDistinctOrderBySupported = true;
    //SqlProviderFlags.IsParameterOrderDependent = true;
    SetCharField("CHAR", (DbDataReader r, int i) => r.GetString(i).TrimEnd());

    return true;
  }

  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public override BulkCopyRowsCopied BulkCopy<T>(DataOptions dataOptions, ITable<T> table, IEnumerable<T> source) {
    switch (DbSystem) {
      case var _ when DbSystem == DbSystem.Access: return new Access.AccessBulkCopy().BulkCopy(dataOptions.BulkCopyOptions.BulkCopyType == BulkCopyType.Default ? Access.AccessTools.DefaultBulkCopyType : dataOptions.BulkCopyOptions.BulkCopyType, table, dataOptions, source);
      default: return base.BulkCopy(dataOptions, table, source);
    };
  }

  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public override Task<BulkCopyRowsCopied> BulkCopyAsync<T>(DataOptions dataOptions, ITable<T> table, IAsyncEnumerable<T> source, CancellationToken cancellationToken) {
    switch (DbSystem) {
      case var _ when DbSystem == DbSystem.Access: return new Access.AccessBulkCopy().BulkCopyAsync(dataOptions.BulkCopyOptions.BulkCopyType == BulkCopyType.Default ? Access.AccessTools.DefaultBulkCopyType : dataOptions.BulkCopyOptions.BulkCopyType, table, dataOptions, source, cancellationToken);
      default: return base.BulkCopyAsync(dataOptions, table, source, cancellationToken);
    };
  }

  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public override Task<BulkCopyRowsCopied> BulkCopyAsync<T>(DataOptions dataOptions, ITable<T> table, IEnumerable<T> source, CancellationToken cancellationToken) {
    switch (DbSystem) {
      case var _ when DbSystem == DbSystem.Access: return new Access.AccessBulkCopy().BulkCopyAsync(dataOptions.BulkCopyOptions.BulkCopyType == BulkCopyType.Default ? Access.AccessTools.DefaultBulkCopyType : dataOptions.BulkCopyOptions.BulkCopyType, table, dataOptions, source, cancellationToken);
      default: return base.BulkCopyAsync(dataOptions, table, source, cancellationToken);
    };
  }

  public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema, DataOptions dataOptions) => new GenericSqlBuilder(this, DataSourceInformationRow, mappingSchema, dataOptions, GetSqlOptimizer(dataOptions), SqlProviderFlags);

  public override ISqlOptimizer GetSqlOptimizer(DataOptions dataOptions) => new GenericSqlOptimizer(DataSourceInformationRow, dataOptions, SqlProviderFlags);
  //protected override ISqlOptimizer SqlOptimizer { get; }

  public override void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object? value) {
    switch (DbSystem) {
      case var _ when DbSystem == DbSystem.Access: SetParameter_Access(dataConnection, parameter, name, dataType, value); break;
        //case DbSystem.Names.DB2iSeries: SetParameter_DB2iSeries_MTGFS01(dataConnection, parameter, name, dataType, value); break;
        //default: base.SetParameter(dataConnection, parameter, name, dataType, value); break;
    };
    base.SetParameter(dataConnection, parameter, name, dataType, value);
  }

  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public void SetParameter_Access(DataConnection dataConnection, IDbDataParameter parameter, string name, DbDataType dataType, object? value) {
    if (base.ConnectionNamespace == DbProviderNamespace.System_Data_Odbc.Name) {
      switch (dataType.DataType) {
        case DataType.SByte:
          if (value is sbyte sbyteVal)
            value = (byte)sbyteVal;
          break;
        case DataType.UInt16:
          if (value is ushort ushortVal)
            value = (short)ushortVal;
          break;
        case DataType.UInt32:
          if (value is uint uintVal)
            value = (int)uintVal;
          break;
        case DataType.Int64:
          if (value is long longValue)
            value = (int)longValue;
          break;
        case DataType.UInt64:
          if (value is ulong ulongValue)
            value = (int)(uint)ulongValue;
          break;
      }
    }
    if (base.ConnectionNamespace == DbProviderNamespace.System_Data_OleDb.Name) { }
  }

  public void SetParameter_DB2iSeries_MTGFS01(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object? value) {
    if (dataType.DataType == DataType.DateTime2) {
      dataType = dataType.WithDataType(DataType.DateTime);
    }
    base.SetParameter(dataConnection, parameter, "@" + name, dataType, value);
  }

  protected override void SetParameterType(DataConnection dataConnection, DbParameter parameter, DbDataType dataType) {
    switch (DbSystem) {
      case var _ when DbSystem == DbSystem.Access: SetParameterType_Access(dataConnection, parameter, dataType); break;
      default: base.SetParameterType(dataConnection, parameter, dataType); break;
    };
  }

  [UrlAsAt.AccessOdbcDataProviderDataProvider_2021_03_14]
  [UrlAsAt.AccessOleDbDataProviderDataProvider_2021_03_14]
  public void SetParameterType_Access(DataConnection dataConnection, DbParameter parameter, DbDataType dataType) {
    if (base.ConnectionNamespace == DbProviderNamespace.System_Data_Odbc.Name) {
      //OdbcType? type = null;
      //switch (dataType.DataType) {
      //  case DataType.Variant: type = OdbcType.Binary; break;
      //}
      //if (type != null) {
      //  var param = TryGetProviderParameter(parameter, dataConnection.MappingSchema);
      //  if (param != null) {
      //    Adapter.SetDbType(param, type.Value);
      //    return;
      //  }
      //}
      switch (dataType.DataType) {
        case DataType.SByte: parameter.DbType = DbType.Byte; return;
        case DataType.UInt16: parameter.DbType = DbType.Int16; return;
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Int64: parameter.DbType = DbType.Int32; return;
        case DataType.Money:
        case DataType.SmallMoney:
        case DataType.VarNumeric:
        case DataType.Decimal: parameter.DbType = DbType.AnsiString; return;
        // fallback
        case DataType.Variant: parameter.DbType = DbType.Binary; return;
      }
    } else if (base.ConnectionNamespace == DbProviderNamespace.System_Data_OleDb.Name) {
      //OleDbType? type = null;
      //switch (dataType.DataType) {
      //  case DataType.DateTime:
      //  case DataType.DateTime2: type = OleDbType.Date; break;
      //  case DataType.Text: type = OleDbType.LongVarChar; break;
      //  case DataType.NText: type = OleDbType.LongVarWChar; break;
      //}
      //if (type != null) {
      //  var param = TryGetProviderParameter(parameter, dataConnection.MappingSchema);
      //  if (param != null) {
      //    Adapter.SetDbType(param, type.Value);
      //    return;
      //  }
      //}
      switch (dataType.DataType) {
        // "Data type mismatch in criteria expression" fix for culture-aware number decimal separator
        // unfortunatelly, regular fix using ExecuteScope=>InvariantCultureRegion
        // doesn't work for all situations
        case DataType.Decimal:
        case DataType.VarNumeric: parameter.DbType = DbType.AnsiString; return;
        case DataType.DateTime:
        case DataType.DateTime2: parameter.DbType = DbType.DateTime; return;
        case DataType.Text: parameter.DbType = DbType.AnsiString; return;
        case DataType.NText: parameter.DbType = DbType.String; return;
      }
    }
    base.SetParameterType(dataConnection, parameter, dataType);
  }

}
