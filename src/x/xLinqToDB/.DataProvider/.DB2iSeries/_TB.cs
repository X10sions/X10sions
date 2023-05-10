//using IBM.Data.DB2.iSeries;
//using LinqToDB;
//using LinqToDB.Common;
//using LinqToDB.Expressions;
//using LinqToDB.Linq;
//using LinqToDB.Mapping;
//using LinqToDB.Metadata;
//using LinqToDB.SqlProvider;
//using LinqToDB.SqlQuery;
//using System.Data;
//using System.Reflection;
//using System.Text;

//namespace LinqToDB.DataProvider.DB2iSeries.TB_V1_9_0 {

//  public class DB2iSeriesDataProvider_TB : DataProviderBase {
//    public readonly iDB2NamingConvention NamingConvention;

//    private readonly DB2iSeriesSqlOptimizer_TB _sqlOptimizer;

//    public override string ConnectionNamespace { get; }

//    public override Type DataReaderType { get; }

//    public override MappingSchema MappingSchema => new DB2iSeriesMappingSchema();

//    public DB2iSeriesDataProvider_TB(iDB2NamingConvention namingConvention)
//      : base("DB2.iSeries", null) {
//      ConnectionNamespace = typeof(iDB2Connection).Namespace;
//      DataReaderType = typeof(iDB2DataReader);
//      NamingConvention = namingConvention;
//      base.SqlProviderFlags.AcceptsTakeAsParameter = false;
//      base.SqlProviderFlags.AcceptsTakeAsParameterIfSkip = true;
//      base.SqlProviderFlags.IsDistinctOrderBySupported = true;
//      SetCharField("CHAR", (IDataReader r, int i) => r.GetString(i).TrimEnd());
//      _sqlOptimizer = new DB2iSeriesSqlOptimizer_TB(base.SqlProviderFlags);
//      SetProviderFields();
//      MappingSchema_AddScalarTypes();
//      DB2iSeriesExpressions.LoadExpressions();
//    }

//    protected override IDbConnection CreateConnectionInternal(string connectionString) {
//      return new iDB2Connection(connectionString);
//    }

//    public override ISqlBuilder CreateSqlBuilder() {
//      return new DB2iSeriesSqlBuilder_TB(GetSqlOptimizer(), base.SqlProviderFlags, MappingSchema.ValueToSqlConverter, NamingConvention);
//    }

//    public override ISchemaProvider GetSchemaProvider() {
//      return new DB2iSeriesSchemaProvider();
//    }

//    public override ISqlOptimizer GetSqlOptimizer() {
//      return _sqlOptimizer;
//    }

//    public override bool IsCompatibleConnection(IDbConnection connection) {
//      return typeof(iDB2Connection).IsSameOrParentOf(connection.GetType());
//    }

//    public override void SetParameter(IDbDataParameter parameter, string name, DataType dataType, object value) {
//      DataType dataType2 = dataType;
//      if (dataType2 == DataType.DateTime2) {
//        dataType = DataType.DateTime;
//      }
//      base.SetParameter(parameter, "@" + name, dataType, dataType.GetiDB2NativeFormat(value));
//    }

//    public override void InitCommand(DataConnection dataConnection, CommandType commandType, string commandText, DataParameter[] parameters) {
//      base.InitCommand(dataConnection, commandType, commandText.SafeSql(), parameters);
//    }

//    public override int Merge<T>(DataConnection dataConnection, Expression<Func<T, bool>> deletePredicate, bool delete, IEnumerable<T> source, string tableName, string databaseName, string schemaName) {
//      if (delete) {
//        throw new LinqToDBException("DB2 iSeries MERGE statement does not support DELETE by source.");
//      }
//      return new DB2iSeriesMerge().Merge(dataConnection, deletePredicate, delete, source, tableName, databaseName, schemaName);
//    }

//    public void SetProviderFields() {
//    }

//    public void MappingSchema_AddScalarTypes() {
//      MappingSchema.AddScalarType(typeof(iDB2BigInt), iDB2BigInt.Null, canBeNull: true, DataType.Int64);
//      MappingSchema.AddScalarType(typeof(iDB2Char), iDB2Char.Null, canBeNull: true, DataType.Char);
//      MappingSchema.AddScalarType(typeof(iDB2Date), iDB2Date.Null, canBeNull: true, DataType.Date);
//      MappingSchema.AddScalarType(typeof(iDB2Decimal), iDB2Decimal.Null, canBeNull: true, DataType.Decimal);
//      MappingSchema.AddScalarType(typeof(iDB2Double), iDB2Double.Null, canBeNull: true, DataType.Double);
//      MappingSchema.AddScalarType(typeof(iDB2Integer), iDB2Integer.Null, canBeNull: true, DataType.Int32);
//      MappingSchema.AddScalarType(typeof(iDB2SmallInt), iDB2SmallInt.Null, canBeNull: true, DataType.Int16);
//      MappingSchema.AddScalarType(typeof(iDB2Time), iDB2Time.Null, canBeNull: true, DataType.Time);
//      MappingSchema.AddScalarType(typeof(iDB2TimeStamp), iDB2TimeStamp.Null, canBeNull: true, DataType.DateTime2);
//      MappingSchema.AddScalarType(typeof(iDB2VarChar), iDB2VarChar.Null, canBeNull: true, DataType.VarChar);
//    }
//  }

//  public static class DB2iSeriesExpressions {
//    public static void LoadExpressions() {
//      Expressions.MapMember("DB2.iSeries", (double d) => Math.Floor(d), (double x) => Sql.Convert<int, double>(x));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.Space(0)), Expressions.N(() => Expressions.L((int? p0) => Sql.Convert(Sql.VarChar(1000), Expressions.Replicate(" ", p0)))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.Stuff("", 0, 0, "")), Expressions.N(() => Expressions.L((string p0, int? p1, int? p2, string p3) => Expressions.AltStuff(p0, p1, p2, p3))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.PadRight("", 0, ' ')), Expressions.N(() => Expressions.L((string p0, int? p1, char? p2) => ((int?)p0.Length > p1) ? p0 : (p0 + Expressions.VarChar(Expressions.Replicate(p2, p1 - (int?)p0.Length), 1000)))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.PadLeft("", 0, ' ')), Expressions.N(() => Expressions.L((string p0, int? p1, char? p2) => ((int?)p0.Length > p1) ? p0 : (Expressions.VarChar(Expressions.Replicate(p2, p1 - (int?)p0.Length), 1000) + p0))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.ConvertTo<string>.From(0m)), Expressions.N(() => Expressions.L((decimal p) => Sql.TrimLeft(Sql.Convert<string, decimal>(p), '0'))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.ConvertTo<string>.From(Guid.Empty)), Expressions.N(() => Expressions.L((Guid p) => Sql.Lower(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(Sql.Substring(Expressions.Hex(p), 7, 2) + Sql.Substring(Expressions.Hex(p), 5, 2), Sql.Substring(Expressions.Hex(p), 3, 2)), Sql.Substring(Expressions.Hex(p), 1, 2)), "-"), Sql.Substring(Expressions.Hex(p), 11, 2)), Sql.Substring(Expressions.Hex(p), 9, 2)), "-"), Sql.Substring(Expressions.Hex(p), 15, 2)), Sql.Substring(Expressions.Hex(p), 13, 2)), "-"), Sql.Substring(Expressions.Hex(p), 17, 4)), "-"), Sql.Substring(Expressions.Hex(p), 21, 12))))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.Log(0m, 0m)), Expressions.N(() => Expressions.L((decimal? m, decimal? n) => Sql.Log(n) / Sql.Log(m))));
//      Expressions.MapMember("DB2.iSeries", Expressions.M(() => Sql.Log(0.0, 0.0)), Expressions.N(() => Expressions.L((double? m, double? n) => Sql.Log(n) / Sql.Log(m))));
//    }

//    public static void MapMembers() {
//    }
//  }

//  public class DB2iSeriesMappingSchema : MappingSchema {
//    internal static readonly DB2iSeriesMappingSchema Instance = new DB2iSeriesMappingSchema();

//    public DB2iSeriesMappingSchema()
//      : this("DB2.iSeries") {
//    }

//    public class DB2iSeriesAttributeReader : IMetadataReader {
//      private readonly AttributeReader _reader;

//      public DB2iSeriesAttributeReader() {
//        _reader = new AttributeReader();
//      }

//      public T[] GetAttributes<T>(Type type, bool inherit = true) where T : Attribute {
//        return Array<T>.Empty;
//      }

//      public T[] GetAttributes<T>(Type type, MemberInfo memberInfo, bool inherit = true) where T : Attribute {
//        if (typeof(T) == typeof(LinqToDB.Mapping.ColumnAttribute)) {
//          System.Data.Linq.Mapping.ColumnAttribute[] attrs = _reader.GetAttributes<System.Data.Linq.Mapping.ColumnAttribute>(type, memberInfo, inherit);
//          if (attrs.Length == 1) {
//            System.Data.Linq.Mapping.ColumnAttribute c = attrs[0];
//            LinqToDB.Mapping.ColumnAttribute attr = new LinqToDB.Mapping.ColumnAttribute {
//              Name = c.Name,
//              DbType = c.DbType,
//              CanBeNull = c.CanBeNull,
//              Storage = c.Storage,
//              SkipOnInsert = c.IsDbGenerated,
//              SkipOnUpdate = c.IsDbGenerated
//            };
//            return new T[1] { (T)(Attribute)attr };
//          }
//        }
//        return Array<T>.Empty;
//      }
//    }


//    protected DB2iSeriesMappingSchema(string configuration)
//      : base(configuration) {
//      SetDataType(typeof(string), new SqlDataType(DataType.VarChar, typeof(string), 255));
//      SetValueToSqlConverter(typeof(char), delegate (StringBuilder sb, SqlDataType dt, object v)
//      {
//        ConvertCharToSql(sb, (char)v);
//      });
//      SetValueToSqlConverter(typeof(DateTime), delegate (StringBuilder sb, SqlDataType dt, object v)
//      {
//        ConvertDateTimeToSql(sb, dt, (DateTime)v);
//      });
//      SetValueToSqlConverter(typeof(Guid), delegate (StringBuilder sb, SqlDataType dt, object v)
//      {
//        ConvertGuidToSql(sb, (Guid)v);
//      });
//      SetValueToSqlConverter(typeof(string), delegate (StringBuilder sb, SqlDataType dt, object v)
//      {
//        ConvertStringToSql(sb, v.ToString());
//      });
//      AddMetadataReader(new DB2iSeriesMetadataReader());
//      AddMetadataReader(new DB2iSeriesAttributeReader());
//    }

//    private static void AppendConversion(StringBuilder stringBuilder, int value) {
//      stringBuilder.Append($"varchar({value})");
//    }

//    private static void ConvertCharToSql(StringBuilder stringBuilder, char value) {
//      xDataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion, value);
//    }

//    public static void ConvertDateTimeToSql(StringBuilder sb, SqlDataType dt, DateTime value) {
//      object fmt = string.Empty;
//      sb.AppendFormat("'{0:" + (dt.DataType switch {
//        DataType.Date => "yyyy-MM-dd",
//        DataType.Time => "HH:mm:ss",
//        _ => (value.Millisecond == 0) ? DB2iSeriesTools.ShortTimestampFormat : DB2iSeriesTools.LongTimestampFormat,
//      }).ToString() + "}'", value);
//    }

//    private static void ConvertGuidToSql(StringBuilder stringBuilder, Guid value) {
//      string s = value.ToString("N");
//      stringBuilder.Append("Cast(x'").Append(s.Substring(6, 2)).Append(s.Substring(4, 2))
//        .Append(s.Substring(2, 2))
//        .Append(s.Substring(0, 2))
//        .Append(s.Substring(10, 2))
//        .Append(s.Substring(8, 2))
//        .Append(s.Substring(14, 2))
//        .Append(s.Substring(12, 2))
//        .Append(s.Substring(16, 16))
//        .Append("' as char(16) for bit data)");
//    }

//    private static void ConvertStringToSql(StringBuilder stringBuilder, string value) {
//      xDataTools.ConvertStringToSql(stringBuilder, "||", "'", AppendConversion, value);
//    }
//  }


//  public enum DB2iSeriesIdentifierQuoteMode {
//    None,
//    Quote,
//    Auto
//  }

//  public class DB2iSeriesSqlBuilder_TB : BasicSqlBuilder {
//    public readonly iDB2NamingConvention NamingConvention = iDB2NamingConvention.System;

//    public static DB2iSeriesIdentifierQuoteMode IdentifierQuoteMode = DB2iSeriesIdentifierQuoteMode.None;

//    protected override string LimitFormat => (SelectQuery.Select.SkipValue == null) ? " FETCH FIRST {0} ROWS ONLY " : null;

//    public DB2iSeriesSqlBuilder_TB(ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags, ValueToSqlConverter valueToSqlConverter, iDB2NamingConvention namingConvention)
//      : base(sqlOptimizer, sqlProviderFlags, valueToSqlConverter) {
//      NamingConvention = namingConvention;
//    }

//    protected override void BuildColumnExpression(ISqlExpression expr, string alias, ref bool addAlias) {
//      bool wrap = false;
//      if (expr.SystemType == typeof(bool)) {
//        wrap = expr is SelectQuery.SearchCondition || (expr is SqlExpression ex && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SelectQuery.SearchCondition);
//      }
//      if (wrap) {
//        StringBuilder.Append("CASE WHEN ");
//      }
//      base.BuildColumnExpression(expr, alias, ref addAlias);
//      if (wrap) {
//        StringBuilder.Append(" THEN 1 ELSE 0 END");
//      }
//    }

//    protected override void BuildCommand(int commandNumber) {
//      StringBuilder.AppendLine(NamingConvention.SelectIdentitySqlString());
//    }

//    protected override void BuildCreateTableIdentityAttribute1(SqlField field) {
//      StringBuilder.Append(" GENERATED ALWAYS As IDENTITY ");
//    }

//    protected override void BuildDataType(SqlDataType type, bool createDbType) {
//      switch (type.DataType) {
//        case DataType.DateTime:
//          StringBuilder.Append("timestamp");
//          break;
//        case DataType.DateTime2:
//          StringBuilder.Append("timestamp");
//          break;
//        default:
//          base.BuildDataType(type, createDbType);
//          break;
//      }
//    }

//    protected override void BuildEmptyInsert() {
//      StringBuilder.Append("VALUES");
//      foreach (KeyValuePair<string, SqlField> col in SelectQuery.Insert.Into.Fields) {
//        StringBuilder.Append("(Default)");
//      }
//      StringBuilder.AppendLine();
//    }

//    protected override void BuildFunction(SqlFunction func) {
//      func = ConvertFunctionParameters(func);
//      switch (func.Name.ToLower()) {
//        case "coalesce":
//          func = new SqlFunction(func.SystemType, "Value", func.Parameters[0], func.Parameters[1]);
//          break;
//        case "replicate":
//          func = new SqlFunction(func.SystemType, "Repeat", func.Parameters[0], func.Parameters[1]);
//          break;
//        case "x_upper":
//          func = new SqlFunction(func.SystemType, "sqlbuilder_UCase", func.Parameters[0]);
//          break;
//      }
//      base.BuildFunction(func);
//    }

//    protected override void BuildFromClause() {
//      if (!SelectQuery.IsUpdate) {
//        base.BuildFromClause();
//      }
//    }

//    protected override void BuildInsertOrUpdateQuery() {
//      BuildInsertOrUpdateQueryAsMerge("FROM " + NamingConvention.QualifiedDummyTable() + " FETCH FIRST 1 ROW ONLY ");
//    }

//    protected override void BuildSelectClause() {
//      if (SelectQuery.From.Tables.Count == 0) {
//        AppendIndent().AppendLine("Select");
//        BuildColumns();
//        AppendIndent().AppendLine("FROM " + NamingConvention.QualifiedDummyTable() + " FETCH FIRST 1 ROW ONLY");
//      } else {
//        base.BuildSelectClause();
//      }
//    }

//    protected override void BuildSql() {
//      AlternativeBuildSql(implementOrderBy: true, base.BuildSql);
//    }

//    public override int CommandCount(SelectQuery selectQuery) {
//      return (!selectQuery.IsInsert || !selectQuery.Insert.WithIdentity) ? 1 : 2;
//    }

//    public override object Convert(object value, ConvertType _convertType) {
//      switch (_convertType) {
//        case ConvertType.NameToQueryParameter:
//          return $"@{value}";
//        case ConvertType.NameToCommandParameter:
//        case ConvertType.NameToSprocParameter:
//          return $":{value}";
//        case ConvertType.SprocParameterToName:
//          if (value != null) {
//            string str = value.ToString();
//            return (str.Length > 0 && str[0] == ':') ? str.Substring(1) : str;
//          }
//          break;
//        case ConvertType.NameToQueryField:
//        case ConvertType.NameToQueryFieldAlias:
//        case ConvertType.NameToQueryTable:
//        case ConvertType.NameToQueryTableAlias:
//          if (value != null && IdentifierQuoteMode != 0) {
//            string name = value.ToString();
//            if (name.Length > 0 && name[0] == '"') {
//              return name;
//            }
//            if (IdentifierQuoteMode == DB2iSeriesIdentifierQuoteMode.Quote || name.StartsWith("_", StringComparison.Ordinal) || name.ToCharArray().Any((char c) => char.IsWhiteSpace(c))) {
//              return "\"" + name + "\"";
//            }
//          }
//          break;
//      }
//      return value;
//    }

//    protected override ISqlBuilder CreateSqlBuilder() {
//      return new DB2iSeriesSqlBuilder_TB(SqlOptimizer, SqlProviderFlags, ValueToSqlConverter, NamingConvention);
//    }

//    protected override void BuildCreateTableNullAttribute(SqlField field, DefaulNullable defaulNullable) {
//      if ((defaulNullable != DefaulNullable.Null || !field.CanBeNull) && (defaulNullable != DefaulNullable.NotNull || field.CanBeNull)) {
//        StringBuilder.Append(field.CanBeNull ? "        " : "NOT NULL");
//      }
//    }

//    protected override string GetProviderTypeName(IDbDataParameter parameter) {
//      iDB2Parameter p = (iDB2Parameter)parameter;
//      return p.iDB2DbType.ToString();
//    }
//  }

//  internal class DB2iSeriesSqlOptimizer_TB : BasicSqlOptimizer {
//    public DB2iSeriesSqlOptimizer_TB(SqlProviderFlags sqlProviderFlags)
//      : base(sqlProviderFlags) {
//    }

//    private static void SetQueryParameter(IQueryElement element) {
//      if (element.ElementType == QueryElementType.SqlParameter) {
//        ((SqlParameter)element).IsQueryParameter = false;
//      }
//    }

//    public override SelectQuery Finalize(SelectQuery selectQuery) {
//      new QueryVisitor().Visit(selectQuery.Select, SetQueryParameter);
//      selectQuery = base.Finalize(selectQuery);
//      return selectQuery.QueryType switch {
//        QueryType.Delete => GetAlternativeDelete(selectQuery),
//        QueryType.Update => GetAlternativeUpdate(selectQuery),
//        _ => selectQuery,
//      };
//    }

//    public override ISqlExpression ConvertExpressionImpl(ISqlExpression expr, ConvertVisitor<RunOptimizationContext> visitor) {
//      expr = base.ConvertExpressionImpl(expr, visitor);
//      if (expr is SqlBinaryExpression) {
//        SqlBinaryExpression be = (SqlBinaryExpression)expr;
//        switch (be.Operation) {
//          case "%": {
//              ISqlExpression sqlExpression2;
//              if (be.Expr1.SystemType.IsIntegerType()) {
//                sqlExpression2 = be.Expr1;
//              } else {
//                ISqlExpression sqlExpression = new SqlFunction(typeof(int), "Int", be.Expr1);
//                sqlExpression2 = sqlExpression;
//              }
//              ISqlExpression expr2 = sqlExpression2;
//              return new SqlFunction(be.SystemType, "Mod", expr2, be.Expr2);
//            }
//          case "&":
//            return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
//          case "|":
//            return new SqlFunction(be.SystemType, "BitOr", be.Expr1, be.Expr2);
//          case "^":
//            return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
//          case "+": {
//              ISqlExpression result;
//              if (!(be.SystemType == typeof(string))) {
//                result = expr;
//              } else {
//                ISqlExpression sqlExpression = new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence);
//                result = sqlExpression;
//              }
//              return result;
//            }
//        }
//      } else if (expr is SqlFunction) {
//        SqlFunction func = (SqlFunction)expr;
//        switch (func.Name.ToLower()) {
//          case "convert": {
//              if (func.SystemType.ToUnderlying() == typeof(bool)) {
//                ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
//                if (ex != null) {
//                  return ex;
//                }
//              }
//              if (func.Parameters[0] is SqlDataType) {
//                SqlDataType type = (SqlDataType)func.Parameters[0];
//                if (type.Type.SystemType == typeof(string) && func.Parameters[1].SystemType != typeof(string)) {
//                  return new SqlFunction(func.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[1]));
//                }
//                int? length = type.Type.Length;
//                if ((length.HasValue ? new bool?(length.GetValueOrDefault() > 0) : null).GetValueOrDefault()) {
//                  return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Type.Length));
//                }
//                length = type.Type.Precision;
//                if ((length.HasValue ? new bool?(length.GetValueOrDefault() > 0) : null).GetValueOrDefault()) {
//                  return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1], new SqlValue(type.Type.Precision), new SqlValue(type.Type.Scale));
//                }
//                return new SqlFunction(func.SystemType, type.Type.DataType.ToString(), func.Parameters[1]);
//              }
//              if (func.Parameters[0] is SqlFunction) {
//                SqlFunction f = (SqlFunction)func.Parameters[0];
//                return (f.Name.ToLower() == "char") ? new SqlFunction(func.SystemType, f.Name, func.Parameters[1]) : ((f.Parameters.Length == 1) ? new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0]) : new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]));
//              }
//              SqlExpression e = (SqlExpression)func.Parameters[0];
//              return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
//            }
//          case "millisecond":
//            return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
//          case "smallDateTime":
//          case "dateTime":
//          case "dateTime2":
//            return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
//          case "uint16":
//            return new SqlFunction(func.SystemType, "Int", func.Parameters);
//          case "uint32":
//            return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
//          case "uint64":
//            return new SqlFunction(func.SystemType, "Decimal", func.Parameters);
//          case "byte":
//          case "sbyte":
//          case "int16":
//            return new SqlFunction(func.SystemType, "SmallInt", func.Parameters);
//          case "int32":
//            return new SqlFunction(func.SystemType, "Int", func.Parameters);
//          case "int64":
//            return new SqlFunction(func.SystemType, "BigInt", func.Parameters);
//          case "double":
//            return new SqlFunction(func.SystemType, "Float", func.Parameters);
//          case "single":
//            return new SqlFunction(func.SystemType, "Real", func.Parameters);
//          case "money":
//            return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(19), new SqlValue(4));
//          case "smallmoney":
//            return new SqlFunction(func.SystemType, "Decimal", func.Parameters[0], new SqlValue(10), new SqlValue(4));
//          case "varchar":
//            if (!(func.Parameters[0].SystemType.ToUnderlying() == typeof(decimal))) {
//              break;
//            }
//            return new SqlFunction(func.SystemType, "Char", func.Parameters[0]);
//          case "nchar":
//          case "nvarchar":
//            return new SqlFunction(func.SystemType, "Char", func.Parameters);
//          case "datediff":
//            switch ((Sql.DateParts)((SqlValue)func.Parameters[0]).Value) {
//              case Sql.DateParts.Day:
//                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 86400", 80, func.Parameters[2], func.Parameters[1]);
//              case Sql.DateParts.Hour:
//                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 3600", 80, func.Parameters[2], func.Parameters[1]);
//              case Sql.DateParts.Minute:
//                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) / 60", 80, func.Parameters[2], func.Parameters[1]);
//              case Sql.DateParts.Second:
//                return new SqlExpression(typeof(int), "(Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))", 60, func.Parameters[2], func.Parameters[1]);
//              case Sql.DateParts.Millisecond:
//                return new SqlExpression(typeof(int), "((Days({0}) - Days({1})) * 86400 + (MIDNIGHT_SECONDS({0}) - MIDNIGHT_SECONDS({1}))) * 1000 + (MICROSECOND({0}) - MICROSECOND({1})) / 1000", 60, func.Parameters[2], func.Parameters[1]);
//            }
//            break;
//          case "ltrim":
//            throw new NotImplementedException("ConvertExpression.ltrim");
//          case "rtrim":
//            throw new NotImplementedException("ConvertExpression.rtrim");
//        }
//      }
//      return expr;
//    }
//  }

//}
