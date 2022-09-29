using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System.Text;
using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;
using System.Data.Linq;
using LinqToDB.Common;
using System.Xml;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using LinqToDB.Expressions;
using LinqToDB.Metadata;
using LinqToDB.DataProvider;

namespace LinqToDB.DataProvider {
  public class GenericMappingSchema : LockedMappingSchema {

    #region Instances

    public static Dictionary<string, GenericMappingSchema> Instances = new Dictionary<string, GenericMappingSchema>();

    public static GenericMappingSchema GetInstance(DataSourceInformationRow dataSourceInformationRow) {
      //var key = dataSourceInformationRow.GetHashCode();
      var key = dataSourceInformationRow.GetDataSourceProductNameWithVersion();
      //var key = $"{dbSystemEnum}-v{version}";
      Instances.TryGetValue(key, out var mappingSchema);
      if (mappingSchema == null) {
        mappingSchema = new GenericMappingSchema(dataSourceInformationRow);
        Instances[key] = mappingSchema;
      }
      return mappingSchema;
    }

    #endregion

    GenericMappingSchema(DataSourceInformationRow dataSourceInformationRow) : base("Generic") {
      DataSourceInformationRow = dataSourceInformationRow;
      //GenericDataProvider = genericDataProvider;
      DbSystem = DataSourceInformationRow.GetDbSystem();
      //DbSystemVersion = version;
      var initDone = DbSystem switch {
        //{ DataSourceProduct?.DbSystem?.Name:   DbSystem.Names.Access } => GenericMappingSchema_InitAccess(),
        var _ when DbSystem  == DbSystem.DB2iSeries => this.GenericMappingSchema_InitDB2iSeries(DataSourceInformationRow.Version),
        //  DbSystem.Names.SapHana => GenericMappingSchema_InitSapHana(),
        // DbSystem.Names.SqlServer => GenericMappingSchema_InitSqlServer(dataSourceInformationRow.Version),
        _ => throw new NotImplementedException($"{DbSystem}: v{DataSourceInformationRow.Version}")
      };
    }
    //IGenericDataProvider GenericDataProvider { get; }
    //DbSystem.Enum DbSystemEnum { get; }
    //Version? DbSystemVersion { get; }
    //string connectionString { get; }
    //DataSourceInformationRow dataSourceInformationRow => DataSourceInformationRow.GetInstance<TConnection>(connection);
    DataSourceInformationRow DataSourceInformationRow { get; }
    DbSystem? DbSystem { get; }

    public override LambdaExpression? TryGetConvertExpression(Type from, Type to) {
      return DbSystem  switch {
        var _ when DbSystem == DbSystem.SqlServer => this.TryGetConvertExpression_SqlServer((from, to) => base.TryGetConvertExpression(from, to), from, to, DataSourceInformationRow.Version),
        _ => base.TryGetConvertExpression(from, to)
      };
    }

  }
}


namespace LinqToDB.Mapping {

  public static class MapingSchemaExtensions {

    [UrlAsAt.AccessMappingSchema_2021_05_07]
    public static bool GenericMappingSchema_InitAccess(this MappingSchema mappingSchema) {
      mappingSchema.SetDataType(typeof(DateTime), DataType.DateTime);
      mappingSchema.SetDataType(typeof(DateTime?), DataType.DateTime);
      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      mappingSchema.SetValueToSqlConverter(typeof(bool), (sb, dt, v) => sb.Append(v));
      mappingSchema.SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_Access(((Binary)v).ToArray()));
      mappingSchema.SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_Access((byte[])v));
      mappingSchema.SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_Access((char)v));
      mappingSchema.SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_Access((Guid)v));
      mappingSchema.SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_Access((DateTime)v));
      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_Access(v.ToString()!));
      return true;
    }

    [UrlAsAt.DB2MappingSchema_2021_05_07]
    public static bool GenericMappingSchema_InitDB2(this MappingSchema mappingSchema) {
      void xAppendConversion_DB2(StringBuilder stringBuilder, int value) => stringBuilder.Append($"chr({value})");
      void xConvertStringToSql_DB2(StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", null, xAppendConversion_DB2, value, null);

      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      mappingSchema.SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_DB2((byte[])v));
      mappingSchema.SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_DB2(((Binary)v).ToArray()));
      mappingSchema.SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_DB2((char)v));
      mappingSchema.SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_DB2(dt, (DateTime)v));
      mappingSchema.SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_DB2((Guid)v));
      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => xConvertStringToSql_DB2(sb, v.ToString()!));
      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_DB2(v.ToString()!));
      mappingSchema.SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeToSql_DB2((TimeSpan)v));

      mappingSchema.SetConverter<string, DateTime>(  GenericExtensions.ParseDateTime_DB2);
      return true;
    }

    public static bool GenericMappingSchema_InitDB2iSeries(this MappingSchema mappingSchema, Version? version) {
      mappingSchema.ColumnNameComparer = StringComparer.OrdinalIgnoreCase;

      //SetDataType(typeof(bool), DataType.Int16);
      //SetDataType(typeof(DateTime), DataType.Timestamp);
      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.VarChar, typeof(string), 255));

      mappingSchema.SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_DB2iSeries((byte[])v));
      mappingSchema.SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_DB2iSeries(((Binary)v).ToArray()));
      mappingSchema.SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_DB2iSeries((char)v));
      mappingSchema.SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_DB2iSeries(dt, (DateTime)v));
      mappingSchema.SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_DB2iSeries((Guid)v));
      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_DB2iSeries(v.ToString()));
      mappingSchema.SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeToSql_DB2iSeries((TimeSpan)v));

      //SetConverter<string, DateTime>(GenericExtensions.ParseDateTime_DB2iSeries);

      //AddMetadataReader(new DB2iSeriesMetadataReader(providerName));
      //AddMetadataReader(new DB2iSeriesAttributeReader());

      return true;
    }

    [UrlAsAt.SapHanaMappingSchema_2021_05_07]
    public static bool GenericMappingSchema_InitSapHana(this MappingSchema mappingSchema) {
      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      mappingSchema.SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_SapHana(((Binary)v).ToArray()));
      mappingSchema.SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_SapHana((byte[])v));
      mappingSchema.SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_SapHana((char)v));
      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_SapHana(v.ToString()!));
      return true;
    }

    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public static bool GenericMappingSchema_InitSqlServer(this MappingSchema mappingSchema, Version version) {
      mappingSchema.ColumnNameComparer = StringComparer.OrdinalIgnoreCase;

      mappingSchema.SetConvertExpression<SqlXml, XmlReader>(s => s.IsNull ? DefaultValue<XmlReader>.Value : s.CreateReader(), s => s.CreateReader());
      mappingSchema.SetConvertExpression<string, SqlXml>(s => new SqlXml(new MemoryStream(Encoding.UTF8.GetBytes(s))));

      mappingSchema.AddScalarType(typeof(SqlBinary), SqlBinary.Null, true, DataType.VarBinary);
      mappingSchema.AddScalarType(typeof(SqlBinary?), SqlBinary.Null, true, DataType.VarBinary);
      mappingSchema.AddScalarType(typeof(SqlBoolean), SqlBoolean.Null, true, DataType.Boolean);
      mappingSchema.AddScalarType(typeof(SqlBoolean?), SqlBoolean.Null, true, DataType.Boolean);
      mappingSchema.AddScalarType(typeof(SqlByte), SqlByte.Null, true, DataType.Byte);
      mappingSchema.AddScalarType(typeof(SqlByte?), SqlByte.Null, true, DataType.Byte);
      mappingSchema.AddScalarType(typeof(SqlDateTime), SqlDateTime.Null, true, DataType.DateTime);
      mappingSchema.AddScalarType(typeof(SqlDateTime?), SqlDateTime.Null, true, DataType.DateTime);
      mappingSchema.AddScalarType(typeof(SqlDecimal), SqlDecimal.Null, true, DataType.Decimal);
      mappingSchema.AddScalarType(typeof(SqlDecimal?), SqlDecimal.Null, true, DataType.Decimal);
      mappingSchema.AddScalarType(typeof(SqlDouble), SqlDouble.Null, true, DataType.Double);
      mappingSchema.AddScalarType(typeof(SqlDouble?), SqlDouble.Null, true, DataType.Double);
      mappingSchema.AddScalarType(typeof(SqlGuid), SqlGuid.Null, true, DataType.Guid);
      mappingSchema.AddScalarType(typeof(SqlGuid?), SqlGuid.Null, true, DataType.Guid);
      mappingSchema.AddScalarType(typeof(SqlInt16), SqlInt16.Null, true, DataType.Int16);
      mappingSchema.AddScalarType(typeof(SqlInt16?), SqlInt16.Null, true, DataType.Int16);
      mappingSchema.AddScalarType(typeof(SqlInt32), SqlInt32.Null, true, DataType.Int32);
      mappingSchema.AddScalarType(typeof(SqlInt32?), SqlInt32.Null, true, DataType.Int32);
      mappingSchema.AddScalarType(typeof(SqlInt64), SqlInt64.Null, true, DataType.Int64);
      mappingSchema.AddScalarType(typeof(SqlInt64?), SqlInt64.Null, true, DataType.Int64);
      mappingSchema.AddScalarType(typeof(SqlMoney), SqlMoney.Null, true, DataType.Money);
      mappingSchema.AddScalarType(typeof(SqlMoney?), SqlMoney.Null, true, DataType.Money);
      mappingSchema.AddScalarType(typeof(SqlSingle), SqlSingle.Null, true, DataType.Single);
      mappingSchema.AddScalarType(typeof(SqlSingle?), SqlSingle.Null, true, DataType.Single);
      mappingSchema.AddScalarType(typeof(SqlString), SqlString.Null, true, DataType.NVarChar);
      mappingSchema.AddScalarType(typeof(SqlString?), SqlString.Null, true, DataType.NVarChar);
      mappingSchema.AddScalarType(typeof(SqlXml), SqlXml.Null, true, DataType.Xml);

      mappingSchema.AddScalarType(typeof(DateTime), DataType.DateTime);
      mappingSchema.AddScalarType(typeof(DateTime?), DataType.DateTime);

      // //DataProvider.SqlServer.SqlServerTypes.Configure(this);
      //AddScalarType(typeof(SqlHierarchyIdType), SqlHierarchyIdType.Null, true, DataType.Udt);
      //AddScalarType(typeof(SqlGeographyType), SqlGeographyType.Null, true, DataType.Udt);
      //AddScalarType(typeof(SqlGeometryType), SqlGeometryType.Null, true, DataType.Udt);

      mappingSchema.SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_SqlServer(dt, v.ToString()!));
      mappingSchema.SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_SqlServer(dt, (char)v));
      if (new[] { 10, 11, 13, 14 }.Contains(version.Major)) {
        mappingSchema.SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_SqlServer(dt, (DateTime)v));
      } else {
        mappingSchema.SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_SqlServer(null, (DateTime)v));
      }
      mappingSchema.SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeSpanToSql_SqlServer(dt, (TimeSpan)v));
      mappingSchema.SetValueToSqlConverter(typeof(DateTimeOffset), (sb, dt, v) => sb.ConvertDateTimeOffsetToSql_SqlServer(dt, (DateTimeOffset)v));
      mappingSchema.SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_SqlServer((byte[])v));
      mappingSchema.SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_SqlServer(((Binary)v).ToArray()));

      mappingSchema.SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string)));

      mappingSchema.AddMetadataReader(new SystemDataSqlServerAttributeReader());

      return true;
    }

    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public static LambdaExpression? TryGetConvertExpression_SqlServer(this MappingSchema mappingSchema, Func<Type, Type, LambdaExpression?> baseTryGetConvertExpression, Type from, Type to, Version version) {
      const string SqlServerTypes_TypesNamespace = "Microsoft.SqlServer.Types";
      if (new[] { 8, 9, 10, 11, 13, 14 }.Contains(version.Major)) {
        if (from != to && from.FullName == to.FullName && from.Namespace == SqlServerTypes_TypesNamespace) {
          var p = Expression.Parameter(from);
          return Expression.Lambda(Expression.Call(to, "Parse", Array<Type>.Empty, Expression.New(MemberHelper.ConstructorOf(() => new SqlString("")), Expression.Call(Expression.Convert(p, typeof(object)), "ToString", Array<Type>.Empty))), p);
        }
      }
      return baseTryGetConvertExpression(from, to);
    }

  }

}