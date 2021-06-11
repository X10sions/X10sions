//using LinqToDB.Data;
//using LinqToDB.DataProvider;
//using LinqToDB.Mapping;
//using LinqToDB.Metadata;
//using LinqToDB.SchemaProvider;
//using LinqToDB.SqlProvider;
//using LinqToDB.SqlQuery;
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Reflection;
//using System.Text;

//namespace xLinqToDB.DataProvider.DB2iSeries.V3_1_6_1.AS400 {

//  public class DB2iSeriesDataProviderTB<TDbConnection> : DataProviderBase where TDbConnection : IDbConnection, new() {
//    public const string DefaultName = "DB2.iSeries";
//    public DB2iSeriesDataProviderTB(Type dataReaderType) : base(nameof(DB2iSeriesDataProviderTB<TDbConnection>), LinqToDB.DataProvider.DB2iSeries.DB2iSeriesMappingSchema.Instance) {
//      this.dataReaderType = dataReaderType;
//      SqlProviderFlags.IsCommonTableExpressionsSupported = true;
//      sqlOptimizer = new xLinqToDB.DataProvider.DB2iSeries.DB2iSeriesSqlOptimizer(SqlProviderFlags);
//      //schemaProvider=new DB2iSeriesSchemaProviderTB();
//      if (DataConnection.TraceSwitch.TraceInfo) {
//        DataConnection.WriteTraceLine(DataReaderType.Assembly.FullName, DataConnection.TraceSwitch.DisplayName, TraceLevel.Info);
//      }
//    }
//    Type dataReaderType;
//    ISqlOptimizer sqlOptimizer;
//    //ISchemaProvider schemaProvider;

//    public override string? ConnectionNamespace { get; } = typeof(TDbConnection).Namespace;
//    public override Type DataReaderType => dataReaderType;

//    protected override IDbConnection CreateConnectionInternal(string connectionString) => new TDbConnection { ConnectionString = connectionString };
//    public override ISqlBuilder CreateSqlBuilder(MappingSchema mappingSchema) => new DB2iSeriesSqlBuilderTB(mappingSchema, sqlOptimizer, SqlProviderFlags);
//    public override ISqlOptimizer GetSqlOptimizer() => sqlOptimizer;
//    public override ISchemaProvider GetSchemaProvider() => throw new NotImplementedException();

//    //public BulkCopyRowsCopied BulkCopy<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source) => throw new NotImplementedException();
//    //public Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IEnumerable<T> source, CancellationToken cancellationToken) => throw new NotImplementedException();
//    //public Task<BulkCopyRowsCopied> BulkCopyAsync<T>(ITable<T> table, BulkCopyOptions options, IAsyncEnumerable<T> source, CancellationToken cancellationToken) => throw new NotImplementedException();
//    //public Type ConvertParameterType(Type type, DbDataType dataType) => throw new NotImplementedException();
//    //public static Func<IDataProvider, IDbConnection, IDbConnection>? OnConnectionCreated { get; set; }
//    //public Expression GetReaderExpression(IDataReader reader, int idx, Expression readerExpression, Type toType) => throw new NotImplementedException();

//    //public void SetParameter(DataConnection dataConnection, IDbDataParameter parameter, string name, DbDataType dataType, object? value) => throw new NotImplementedException();
//  }

//  public class DB2iSeriesSqlBuilderTB : BasicSqlBuilder {
//    public DB2iSeriesSqlBuilderTB(MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags) : base(mappingSchema, sqlOptimizer, sqlProviderFlags) { }
//    protected override ISqlBuilder CreateSqlBuilder() => new DB2iSeriesSqlBuilderTB(MappingSchema, SqlOptimizer, SqlProviderFlags);

//    protected override void BuildColumnExpression(SelectQuery? selectQuery, ISqlExpression expr, string? alias, ref bool addAlias) {
//      var wrap = expr.SystemType == typeof(bool)
//        ? expr is SqlSearchCondition
//        ? true
//        : expr is SqlExpression ex && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SqlSearchCondition : false;
//      if ((expr is SqlParameter parameter && parameter.Name != null) || (expr is SqlValue value && value.Value == null)) {
//        //IsMapGuidAsString var colType = DB2iSeriesTools.GetiSeriesType(SqlDataType.GetDataType(expr.SystemType ?? typeof(object)), dataProvider.Options.IsMapGuidAsString);
//        var colType = xLinqToDB.DataProvider.DB2iSeries.DB2iSeriesTools.GetiSeriesType(SqlDataType.GetDataType(expr.SystemType ?? typeof(object)));
//        expr = new SqlExpression(expr.SystemType, "Cast({0} as {1})", Precedence.Primary, expr, new SqlExpression(colType, Precedence.Primary));
//      }
//      if (wrap)
//        StringBuilder.Append("CASE WHEN ");
//      base.BuildColumnExpression(selectQuery, expr, alias, ref addAlias);
//      if (wrap)
//        StringBuilder.Append(" THEN 1 ELSE 0 END");
//    }

//  }

//  //  public class DB2iSeriesMappingSchemaTB : MappingSchema {
//  //    public static readonly MappingSchema Instance = new DB2iSeriesMappingSchemaTB("");
//  //    public DB2iSeriesMappingSchemaTB(string configuration) : base(configuration) {
//  //      ColumnNameComparer = StringComparer.OrdinalIgnoreCase;
//  //      SetDataType(typeof(string), new SqlDataType(LinqToDB.DataType.VarChar, typeof(string), 255));
//  //      //SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => ConvertBinaryToSql(sb, (byte[])v)); 
//  //      //SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => ConvertBinaryToSql(sb, ((Binary)v).ToArray()));
//  //      SetValueToSqlConverter(typeof(char), (sb, dt, v) => ConvertCharToSql(sb, (char)v));
//  //      SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => ConvertDateTimeToSql(sb, dt, (DateTime)v));
//  //      //SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.Append($"'{(Guid)v}'"));
//  //      SetValueToSqlConverter(typeof(string), (sb, dt, v) => ConvertStringToSql(sb, v.ToString()));
//  //      //SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => ConvertTimeToSql(sb, (TimeSpan)v));
//  //      //SetConverter<string, DateTime>(DB2iSeriesTools.ParseDateTime);
//  //      AddMetadataReader(new DB2iSeriesMetadataReaderTB(configuration));
//  //#if !NETSTANDARD2_0
//  //      //      AddMetadataReader(new DB2iSeriesAttributeReader());
//  //#endif
//  //    } 

//  //    private static void AppendConversion(StringBuilder stringBuilder, int value) => stringBuilder.Append("varchar(").Append(value).Append(")");

//  //    private static void ConvertStringToSql(StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", "", AppendConversion, value, null);

//  //    private static void ConvertCharToSql(StringBuilder stringBuilder, char value) => DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion, value);

//  //    private static void ConvertDateTimeToSql(StringBuilder stringBuilder, SqlDataType datatype, DateTime value) {
//  //      var format = value.Millisecond == 0 ?
//  //            "'{0:yyyy-MM-dd HH:mm:ss}'" :
//  //            "'{0:yyyy-MM-dd HH:mm:ss.fff}'";

//  //      if(datatype.Type.DataType == LinqToDB.DataType.Date)
//  //        format = "'{0:yyyy-MM-dd}'";

//  //      if(datatype.Type.DataType == LinqToDB.DataType.Time) {
//  //        format = value.Millisecond == 0 ?
//  //              "'{0:HH:mm:ss}'" :
//  //              "'{0:HH:mm:ss.fff}'";
//  //      }
//  //      stringBuilder.AppendFormat(format, value);
//  //    }

//  //  }

//  //  public class DB2iSeriesSqlOptimizerTB : BasicSqlOptimizer {
//  //    public DB2iSeriesSqlOptimizerTB(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }
//  //  }

//  //  public class DB2iSeriesSchemaProviderTB : ISchemaProvider {
//  //    public static readonly ISchemaProvider Instance = new DB2iSeriesSchemaProviderTB();

//  //    public DatabaseSchema GetSchema(DataConnection dataConnection, GetSchemaOptions? options = null) => throw new NotImplementedException();
//  //  }

//  //  public class DB2iSeriesMetadataReaderTB : IMetadataReader {
//  //    private readonly string providerName;

//  //    public DB2iSeriesMetadataReaderTB(string providerName) {
//  //      this.providerName = providerName;
//  //    }

//  //    public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute {
//  //      if(typeof(T) == typeof(Sql.ExpressionAttribute)) {
//  //        switch(memberInfo.Name) {
//  //          case "CharIndex":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute("Locate") };

//  //          case "Trim":
//  //            if(memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase)) {
//  //              return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "Strip({0}, B, {1})") };
//  //            }
//  //            break;
//  //          case "TrimLeft":
//  //            if(memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) ||
//  //              memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
//  //              return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "Strip({0}, L, {1})") };
//  //            }
//  //            break;
//  //          case "TrimRight":
//  //            if(memberInfo.ToString().EndsWith("(Char[])", StringComparison.CurrentCultureIgnoreCase) ||
//  //              memberInfo.ToString().EndsWith("System.Nullable`1[System.Char])", StringComparison.CurrentCultureIgnoreCase)) {
//  //              return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "Strip({0}, T, {1})") };
//  //            }
//  //            break;
//  //          case "Truncate":
//  //            return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "Truncate({0}, 0)") };
//  //          //case "DateAdd":            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") { ServerSideOnly = false, PreferServerSide = false, BuilderType = typeof(DateAddBuilderDB2i) });
//  //          //case "DatePart":            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") { ServerSideOnly = false, PreferServerSide = false, BuilderType = typeof(DatePartBuilderDB2i) });
//  //          //case "DateDiff":            return GetExtension<T>(() => new Sql.ExtensionAttribute(providerName, "") { BuilderType = typeof(DateDiffBuilderDB2i) });

//  //          //case "DateAdd":            return new[] { (T)(object)new Sql.DatePartAttribute(providerName, "{{1}} + {0}", Precedence.Additive, true, new[] { "{0} Year", "({0} * 3) Month", "{0} Month", "{0} Day", "{0} Day", "({0} * 7) Day", "{0} Day", "{0} Hour", "{0} Minute", "{0} Second", "({0} * 1000) Microsecond" }, 0, 1, 2) };
//  //          //case "DatePart":            return new[] { (T)(object)new Sql.DatePartAttribute(providerName, "{0}", false, new[] { null, null, null, null, null, null, "DayOfWeek", null, null, null, null }, 0, 1) };
//  //          case "TinyInt":
//  //            return new[] { (T)(object)new Sql.ExpressionAttribute(providerName, "SmallInt") { ServerSideOnly = true } };
//  //          case "DefaultNChar":
//  //          case "DefaultNVarChar":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Char") { ServerSideOnly = true } };
//  //          case "Substring":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Substr") { PreferServerSide = true } };
//  //          case "Atan2":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Atan2", 1, 0) };
//  //          case "Log":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Ln") };
//  //          case "Log10":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Log") };
//  //          case "NChar":
//  //          case "NVarChar":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Char") { ServerSideOnly = true } };
//  //          case "Replicate":
//  //            return new[] { (T)(object)new Sql.FunctionAttribute(providerName, "Repeat") };
//  //        }
//  //      }

//  //      return new T[] { };
//  //    }

//  //    public MemberInfo[] GetDynamicColumns(Type type) => new MemberInfo[] { };

//  //    public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute => new T[] { };

//  //    private T[] GetExpression<T>(Func<Sql.ExpressionAttribute> build) => typeof(T) == typeof(Sql.ExpressionAttribute) ? (new[] { (T)(object)build() }) : (new T[] { });
//  //    private T[] GetExtension<T>(Func<Sql.ExpressionAttribute> build) => typeof(T) == typeof(Sql.ExpressionAttribute) || typeof(T) == typeof(Sql.ExtensionAttribute) ? (new[] { (T)(object)build() }) : (new T[] { });
//  //    private T[] GetFunction<T>(Func<Sql.ExpressionAttribute> build) => typeof(T) == typeof(Sql.ExpressionAttribute) || typeof(T) == typeof(Sql.FunctionAttribute) ? (new[] { (T)(object)build() }) : (new T[] { });

//  //  }

//}