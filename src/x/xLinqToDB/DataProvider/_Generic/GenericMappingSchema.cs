using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.DB2iSeries;
using System.Text;
using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;
using System.Data.Linq;
using System.Collections.Generic;
using LinqToDB.Common;
using System.Xml;
using System.Data.SqlTypes;
using System.IO;
using System.Linq.Expressions;
using LinqToDB.Expressions;
using LinqToDB.Metadata;
using System.Linq;

namespace LinqToDB.DataProvider {
  public class GenericMappingSchema : MappingSchema {

    public static Dictionary<int, GenericMappingSchema> Instances = new Dictionary<int, GenericMappingSchema>();

    public static GenericMappingSchema GetInstance(DataSourceInformationRow dataSourceInformationRow) {
      var hashCode = dataSourceInformationRow.GetHashCode();
      Instances.TryGetValue(hashCode, out var mappingSchema);
      if (mappingSchema == null) {
        mappingSchema = new GenericMappingSchema(dataSourceInformationRow);
        Instances[hashCode] = mappingSchema;
      }
      return mappingSchema;
    }

    GenericMappingSchema(DataSourceInformationRow dataSourceInformationRow) : base(dataSourceInformationRow.GetDataSourceProductNameWithVersion()) {
      this.dataSourceInformationRow = dataSourceInformationRow;
      var initDone = dataSourceInformationRow.DataSourceProduct?.DbSystem?.Name switch {
        DbSystem.Names.Access => GenericMappingSchema_InitAccess(),
        DbSystem.Names.DB2 => GenericMappingSchema_InitDB2(),
        DbSystem.Names.DB2iSeries => GenericMappingSchema_InitDB2iSeries(),
        DbSystem.Names.SapHana => GenericMappingSchema_InitSapHana(),
        DbSystem.Names.SqlServer => GenericMappingSchema_InitSqlServer(dataSourceInformationRow.Version),
        _ => false
      };
    }

    DataSourceInformationRow dataSourceInformationRow;

    [UrlAsAt.AccessMappingSchema_2021_05_07]
    public bool GenericMappingSchema_InitAccess() {
      SetDataType(typeof(DateTime), DataType.DateTime);
      SetDataType(typeof(DateTime?), DataType.DateTime);
      SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      SetValueToSqlConverter(typeof(bool), (sb, dt, v) => sb.Append(v));
      SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_Access(((Binary)v).ToArray()));
      SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_Access((byte[])v));
      SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_Access((char)v));
      SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_Access((Guid)v));
      SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_Access((DateTime)v));
      SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_Access(v.ToString()!));
      return true;
    }

    [UrlAsAt.DB2MappingSchema_2021_05_07]
    public bool GenericMappingSchema_InitDB2() {
      void xAppendConversion_DB2(StringBuilder stringBuilder, int value) => stringBuilder.Append($"chr({value})");
      void xConvertStringToSql_DB2(StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", null, xAppendConversion_DB2, value, null);


      SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_DB2((byte[])v));
      SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_DB2(((Binary)v).ToArray()));
      SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_DB2((char)v));
      SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_DB2(dt, (DateTime)v));
      SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_DB2((Guid)v));
      SetValueToSqlConverter(typeof(string), (sb, dt, v) => xConvertStringToSql_DB2(sb, v.ToString()!));
      SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_DB2(v.ToString()!));
      SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeToSql_DB2((TimeSpan)v));

      SetConverter<string, DateTime>(GenericExtensions.ParseDateTime_DB2);
      return true;
    }

    public bool GenericMappingSchema_InitDB2iSeries() {
      ColumnNameComparer = StringComparer.OrdinalIgnoreCase;

      //SetDataType(typeof(bool), DataType.Int16);
      //SetDataType(typeof(DateTime), DataType.Timestamp);
      //SetDataType(typeof(string), new SqlDataType(DataType.VarChar, typeof(string), 255));

      //SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_DB2iSeries((byte[])v));
      //SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_DB2iSeries(((Binary)v).ToArray()));
      //SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_DB2iSeries((char)v));
      //SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_DB2iSeries(dt, (DateTime)v));
      //SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_DB2iSeries((Guid)v));
      //SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_DB2iSeries(v.ToString()));
      //SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeToSql_DB2iSeries((TimeSpan)v));

      //SetConverter<string, DateTime>(GenericExtensions.ParseDateTime_DB2iSeries);

      //AddMetadataReader(new DB2iSeriesMetadataReader(providerName));
      //AddMetadataReader(new DB2iSeriesAttributeReader());

      return true;
    }

    [UrlAsAt.SapHanaMappingSchema_2021_05_07]
    public bool GenericMappingSchema_InitSapHana(){
      SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

      SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_SapHana(((Binary)v).ToArray()));
      SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_SapHana( (byte[])v));
      SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_SapHana((char)v));
      SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_SapHana(v.ToString()!));
      return true;
    }

    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public bool GenericMappingSchema_InitSqlServer(Version version) {
      ColumnNameComparer = StringComparer.OrdinalIgnoreCase;

      SetConvertExpression<SqlXml, XmlReader>(s => s.IsNull ? DefaultValue<XmlReader>.Value : s.CreateReader(), s => s.CreateReader());
      SetConvertExpression<string, SqlXml>(s => new SqlXml(new MemoryStream(Encoding.UTF8.GetBytes(s))));

      AddScalarType(typeof(SqlBinary), SqlBinary.Null, true, DataType.VarBinary);
      AddScalarType(typeof(SqlBinary?), SqlBinary.Null, true, DataType.VarBinary);
      AddScalarType(typeof(SqlBoolean), SqlBoolean.Null, true, DataType.Boolean);
      AddScalarType(typeof(SqlBoolean?), SqlBoolean.Null, true, DataType.Boolean);
      AddScalarType(typeof(SqlByte), SqlByte.Null, true, DataType.Byte);
      AddScalarType(typeof(SqlByte?), SqlByte.Null, true, DataType.Byte);
      AddScalarType(typeof(SqlDateTime), SqlDateTime.Null, true, DataType.DateTime);
      AddScalarType(typeof(SqlDateTime?), SqlDateTime.Null, true, DataType.DateTime);
      AddScalarType(typeof(SqlDecimal), SqlDecimal.Null, true, DataType.Decimal);
      AddScalarType(typeof(SqlDecimal?), SqlDecimal.Null, true, DataType.Decimal);
      AddScalarType(typeof(SqlDouble), SqlDouble.Null, true, DataType.Double);
      AddScalarType(typeof(SqlDouble?), SqlDouble.Null, true, DataType.Double);
      AddScalarType(typeof(SqlGuid), SqlGuid.Null, true, DataType.Guid);
      AddScalarType(typeof(SqlGuid?), SqlGuid.Null, true, DataType.Guid);
      AddScalarType(typeof(SqlInt16), SqlInt16.Null, true, DataType.Int16);
      AddScalarType(typeof(SqlInt16?), SqlInt16.Null, true, DataType.Int16);
      AddScalarType(typeof(SqlInt32), SqlInt32.Null, true, DataType.Int32);
      AddScalarType(typeof(SqlInt32?), SqlInt32.Null, true, DataType.Int32);
      AddScalarType(typeof(SqlInt64), SqlInt64.Null, true, DataType.Int64);
      AddScalarType(typeof(SqlInt64?), SqlInt64.Null, true, DataType.Int64);
      AddScalarType(typeof(SqlMoney), SqlMoney.Null, true, DataType.Money);
      AddScalarType(typeof(SqlMoney?), SqlMoney.Null, true, DataType.Money);
      AddScalarType(typeof(SqlSingle), SqlSingle.Null, true, DataType.Single);
      AddScalarType(typeof(SqlSingle?), SqlSingle.Null, true, DataType.Single);
      AddScalarType(typeof(SqlString), SqlString.Null, true, DataType.NVarChar);
      AddScalarType(typeof(SqlString?), SqlString.Null, true, DataType.NVarChar);
      AddScalarType(typeof(SqlXml), SqlXml.Null, true, DataType.Xml);

      AddScalarType(typeof(DateTime), DataType.DateTime);
      AddScalarType(typeof(DateTime?), DataType.DateTime);

      // //DataProvider.SqlServer.SqlServerTypes.Configure(this);
      //AddScalarType(typeof(SqlHierarchyIdType), SqlHierarchyIdType.Null, true, DataType.Udt);
      //AddScalarType(typeof(SqlGeographyType), SqlGeographyType.Null, true, DataType.Udt);
      //AddScalarType(typeof(SqlGeometryType), SqlGeometryType.Null, true, DataType.Udt);

      SetValueToSqlConverter(typeof(string), (sb, dt, v) => sb.ConvertStringToSql_SqlServer(dt, v.ToString()!));
      SetValueToSqlConverter(typeof(char), (sb, dt, v) => sb.ConvertCharToSql_SqlServer(dt, (char)v));
      if (new[] { 10, 11, 13, 14 }.Contains(version.Major)) {
        SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_SqlServer(dt, (DateTime)v));
      } else {
        SetValueToSqlConverter(typeof(DateTime), (sb, dt, v) => sb.ConvertDateTimeToSql_SqlServer(null, (DateTime)v));
      }
      SetValueToSqlConverter(typeof(TimeSpan), (sb, dt, v) => sb.ConvertTimeSpanToSql_SqlServer(dt, (TimeSpan)v));
      SetValueToSqlConverter(typeof(DateTimeOffset), (sb, dt, v) => sb.ConvertDateTimeOffsetToSql_SqlServer(dt, (DateTimeOffset)v));
      SetValueToSqlConverter(typeof(byte[]), (sb, dt, v) => sb.ConvertBinaryToSql_SqlServer((byte[])v));
      SetValueToSqlConverter(typeof(Binary), (sb, dt, v) => sb.ConvertBinaryToSql_SqlServer(((Binary)v).ToArray()));

      SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string)));

      AddMetadataReader(new SystemDataSqlServerAttributeReader());

      return true;
    }

    public override LambdaExpression? TryGetConvertExpression(Type from, Type to) {
      return dataSourceInformationRow.DataSourceProduct?.DbSystem?.Name switch {
        DbSystem.Names.SqlServer => TryGetConvertExpression_SqlServer(from, to, dataSourceInformationRow.Version),
        _ => base.TryGetConvertExpression(from, to)
      };
    }

    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public LambdaExpression? TryGetConvertExpression_SqlServer(Type from, Type to, Version version) {
      const string SqlServerTypes_TypesNamespace = "Microsoft.SqlServer.Types";
      if (new[] { 8, 9, 10, 11, 13, 14 }.Contains(version.Major)) {
        if (from != to && from.FullName == to.FullName && from.Namespace == SqlServerTypes_TypesNamespace) {
          var p = Expression.Parameter(from);
          return Expression.Lambda(Expression.Call(to, "Parse", Array<Type>.Empty, Expression.New(MemberHelper.ConstructorOf(() => new SqlString("")), Expression.Call(Expression.Convert(p, typeof(object)), "ToString", Array<Type>.Empty))), p);
        }
      }
      return base.TryGetConvertExpression(from, to);
    }

  }
}

namespace LinqToDB.DataProvider.DB2iSeries {
  public class DB2iSeriesMappingSchema : MappingSchema {
    public DB2iSeriesMappingSchema(string? configuration) : base(configuration) {
      //this.dataSourceInformationRow = dataSourceInformationRow;
    }
    //DataSourceInformationRow dataSourceInformationRow;

    //    public static MappingSchema GetMappingSchema<T>(DataSourceInformationRow dataSourceInformationRow) where T : IDbConnection => new GenericMappingSchema();

  }

}
