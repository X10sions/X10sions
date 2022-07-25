using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.SchemaProvider;
using System.Data;
using LinqToDB.Mapping;
using LinqToDB.SqlProvider;
using LinqToDB.SqlQuery;
using LinqToDB.Extensions;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using System.Data.Common;

namespace LinqToDB.DataProvider.DB2iSeries.MTGFS01_V2_9_8;

public enum DB2iSeriesNamingConvention {
  Sql,
  System
}

public static class DB2iSeriesNamingConventionExtensions {
  public static string Separator(this DB2iSeriesNamingConvention naming) => naming != DB2iSeriesNamingConvention.System ? "." : "/";
  public static string DummyTableName(this DB2iSeriesNamingConvention naming) => "SYSIBM" + naming.Separator() + "SYSDUMMY1";
}

public static class DB2iSeriesProviderNameExtensions {
  public static string Name(this DB2iSeriesProvider provider) => "DB2.iSeries" + provider;
}

public static class SqlDataTypeExtensions {

  public static string GetiSeriesType(this SqlDataType dataType, bool mapGuidAsString) {
    switch (dataType.Type.DataType) {
      case DataType.Binary:
      case DataType.Variant:
        return $"BINARY({((dataType.Type.Length == 0) ? new int?(1) : dataType.Type.Length)})";
      case DataType.Int64:
      case DataType.UInt32:
        return "BIGINT";
      case DataType.Blob:
        return $"BLOB({((dataType.Type.Length == 0) ? new int?(1) : dataType.Type.Length)})";
      case DataType.VarBinary:
        return $"VARBINARY({((dataType.Type.Length == 0) ? new int?(1) : dataType.Type.Length)})";
      case DataType.Char:
        return "CHAR";
      case DataType.Date:
        return "DATE";
      case DataType.UInt64:
        return "DECIMAL(28,0)";
      case DataType.Decimal:
        return "DECIMAL";
      case DataType.Double:
        return "DOUBLE";
      case DataType.Int32:
      case DataType.UInt16:
        return "INTEGER";
      case DataType.Single:
        return "REAL";
      case DataType.Boolean:
      case DataType.Int16:
      case DataType.Byte:
        return "SMALLINT";
      case DataType.Time:
      case DataType.DateTimeOffset:
        return "TIME";
      case DataType.DateTime:
      case DataType.DateTime2:
      case DataType.Timestamp:
        return "TIMESTAMP";
      case DataType.VarChar:
        return $"VARCHAR({((dataType.Type.Length == 0) ? new int?(1) : dataType.Type.Length)})";
      case DataType.NVarChar:
        return $"NVARCHAR({((dataType.Type.Length == 0) ? new int?(1) : dataType.Type.Length)})";
      case DataType.Guid:
        if (!mapGuidAsString) {
          return "char(16) for bit data";
        }
        return "CHAR(32)";
      default:
        return dataType.Type.DataType.ToString();
    }
  }
}

public static class _Extensions {

  public static string GetTypeForCast(this Type dataType, bool mapGuidAsString) {
    string colType = "CHAR";
    if (dataType != null) {
      colType = SqlDataType.GetDataType(dataType).GetiSeriesType(mapGuidAsString);
    }
    return colType;
  }

  public static void SetColumnParameters(this ColumnInfo ci, int? size, int? scale) {
    switch (ci.DataType) {
      case "INTEGER":
        break;
      case "SMALLINT":
        break;
      case "BIGINT":
        break;
      case "TIMESTMP":
        break;
      case "DATE":
        break;
      case "TIME":
        break;
      case "VARG":
        break;
      case "DECFLOAT":
        break;
      case "FLOAT":
        break;
      case "ROWID":
        break;
      case "VARBIN":
        break;
      case "XML":
        break;
      case "DECIMAL":
      case "NUMERIC":
        if (size.GetValueOrDefault() > 0) {
          ci.Precision = size.Value;
        }
        if (scale.GetValueOrDefault() > 0) {
          ci.Scale = scale;
        }
        break;
      case "BINARY":
      case "BLOB":
      case "CHAR":
      case "CHAR FOR BIT DATA":
      case "CLOB":
      case "DATALINK":
      case "DBCLOB":
      case "GRAPHIC":
      case "VARBINARY":
      case "VARCHAR":
      case "VARCHAR FOR BIT DATA":
      case "VARGRAPHIC":
        ci.Length = size;
        break;
      default:
        throw new NotImplementedException("unknown data type: " + ci.DataType);
    }
  }
}

public enum DB2iSeriesProvider {
  [Description("DB2.iSeries.AS400")] AS400,
  [Description("DB2.iSeries")] DB2,
  [Description("DB2.iSeries.GAS")] DB2_GAS,
  [Description("DB2.iSeries.73")] DB2_73,
  [Description("DB2.iSeries.73.GAS")] DB2_73_GAS
}

public class DB2iSeriesMappingSchema : MappingSchema {
  public static readonly DB2iSeriesMappingSchema BlobGuidInstance = new DB2iSeriesMappingSchema(new DB2iSeriesConfiguration {
    Provider = DB2iSeriesProvider.DB2
  });

  public static readonly DB2iSeriesMappingSchema StringGuidInstance = new DB2iSeriesMappingSchema(new DB2iSeriesConfiguration {
    Provider = DB2iSeriesProvider.DB2_GAS
  });

  public DB2iSeriesMappingSchema()
    : this(new DB2iSeriesConfiguration()) {
  }

  public DB2iSeriesMappingSchema(DB2iSeriesConfiguration dB2ISeriesConfiguration)
    : this(dB2ISeriesConfiguration.Provider.Name()) {
    if (dB2ISeriesConfiguration.Provider != DB2iSeriesProvider.DB2_GAS) {
      SetValueToSqlConverter(typeof(Guid), (sb, dt, v) => sb.ConvertGuidToSql_DB2iSeries((Guid)v));
    }
  }

  public DB2iSeriesMappingSchema(string configuration)
    : base(configuration) {
    base.ColumnNameComparer = StringComparer.OrdinalIgnoreCase;
    SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));
    SetValueToSqlConverter(typeof(string), delegate (StringBuilder sb, SqlDataType dt, object v) {
      ConvertStringToSql(sb, v.ToString());
    });
    SetValueToSqlConverter(typeof(char), delegate (StringBuilder sb, SqlDataType dt, object v) {
      ConvertCharToSql(sb, (char)v);
    });
    SetValueToSqlConverter(typeof(DateTime), delegate (StringBuilder sb, SqlDataType dt, object v) {
      ConvertDateTimeToSql(sb, dt, (DateTime)v);
    });
    AddMetadataReader(new DB2iSeriesMetadataReader(configuration));
  }

  private static void AppendConversion(StringBuilder stringBuilder, int value) {
    stringBuilder.Append("varchar(").Append(value).Append(")");
  }

  private static void ConvertStringToSql(StringBuilder stringBuilder, string value) {
    DataTools.ConvertStringToSql(stringBuilder, "||", "", AppendConversion, value, null);
  }

  private static void ConvertCharToSql(StringBuilder stringBuilder, char value) {
    DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion, value);
  }

  private static void ConvertDateTimeToSql(StringBuilder stringBuilder, SqlDataType datatype, DateTime value) {
    string format = (value.Millisecond == 0) ? "'{0:yyyy-MM-dd HH:mm:ss}'" : "'{0:yyyy-MM-dd HH:mm:ss.fff}'";
    if (datatype.Type.DataType == DataType.Date) {
      format = "'{0:yyyy-MM-dd}'";
    }
    if (datatype.Type.DataType == DataType.Time) {
      format = ((value.Millisecond == 0) ? "'{0:HH:mm:ss}'" : "'{0:HH:mm:ss.fff}'");
    }
    stringBuilder.AppendFormat(format, value);
  }

}

public class DB2iSeriesSqlOptimizer : BasicSqlOptimizer {
  public DB2iSeriesSqlOptimizer(SqlProviderFlags sqlProviderFlags)
    : base(sqlProviderFlags) {
  }

  public override ISqlExpression ConvertExpressionImpl(ISqlExpression expr, ConvertVisitor<RunOptimizationContext> visitor) => expr.ConvertExpressionImpl_DB2iSeries_MTGFS01(
    visitor,
    (e, v) => base.ConvertExpressionImpl(e, v),
    (f, pn) => AlternativeConvertToBoolean(f, 1),
    (e, v) => Div(e, v)
    );

  protected override ISqlExpression ConvertFunction(SqlFunction func) => func.ConvertFunction_DB2iSeries_MTGFS01(
    (f, wp) => ConvertFunctionParameters(f, wp),
    f => base.ConvertFunction(f)
    );

  public override SqlStatement Finalize(SqlStatement statement) => statement.Finalize_DB2iSeries_MTGFS01(
    s => base.Finalize(s),
    ds => GetAlternativeDelete(ds),
    us => GetAlternativeUpdate(us)
    );

}

public enum DB2iSeriesVersion {
  v5r1 = 327936,
  v5r2 = 328192,
  v5r3 = 328448,
  v5r4 = 328704,
  v6r1 = 393472,
  v7r1 = 459008,
  v7r2 = 459264,
  v7r3 = 730000,
  v7r4 = 740000
}

public enum DB2iSeriesIdentifierQuoteMode {
  None,
  Quote,
  Auto
}

public class DB2iSeriesConfiguration {
  public bool MapGuidAsString { get; set; } = true;
  public MappingSchema MappingSchema => !MapGuidAsString ? DB2iSeriesMappingSchema.BlobGuidInstance : DB2iSeriesMappingSchema.StringGuidInstance;
  public DB2iSeriesIdentifierQuoteMode IdentifierQuoteMode { get; set; } = DB2iSeriesIdentifierQuoteMode.Auto;
  public DB2iSeriesProvider Provider { get; set; } = DB2iSeriesProvider.DB2;
  public DB2iSeriesNamingConvention NamingConvention { get; set; } = DB2iSeriesNamingConvention.System;
  public DB2iSeriesVersion Version { get; set; } = DB2iSeriesVersion.v5r4;

  public void InitDataProvider(IDataProvider dataProvider, Action<string, Expression<Func<DbDataReader, int, string>>> baseSetCharField) {
    LoadExpressions();
    dataProvider.SqlProviderFlags.AcceptsTakeAsParameter = false;
    dataProvider.SqlProviderFlags.AcceptsTakeAsParameterIfSkip = true;
    dataProvider.SqlProviderFlags.CanCombineParameters = false;
    dataProvider.SqlProviderFlags.IsCommonTableExpressionsSupported = true;
    dataProvider.SqlProviderFlags.IsDistinctOrderBySupported = false;
    dataProvider.SqlProviderFlags.IsParameterOrderDependent = true;
    if (MapGuidAsString) {
      dataProvider.SqlProviderFlags.CustomFlags.Add("MapGuidAsString");
    }
    if (DataConnection.TraceSwitch.TraceInfo) {
      DataConnection.WriteTraceLine(dataProvider.DataReaderType.Assembly.FullName, DataConnection.TraceSwitch.DisplayName, System.Diagnostics.TraceLevel.Info);
    }
    baseSetCharField("CHAR", (r, i) => r.GetString(i).TrimEnd(new char[1] { ' ' }));
    baseSetCharField("NCHAR", (r, i) => r.GetString(i).TrimEnd(new char[1] { ' ' }));
  }

  private void LoadExpressions() {
    string providerName = Provider.Name();
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.Space(0)), Linq.Expressions.L((int? p0) => Sql.Convert(Sql.Types.VarChar(1000), Linq.Expressions.Replicate(" ", p0))));
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.Stuff("", 0, 0, "")), Linq.Expressions.L((string p0, int? p1, int? p2, string p3) => Linq.Expressions.AltStuff(p0, p1, p2, p3)));
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.PadRight("", 0, ' ')), Linq.Expressions.L((string p0, int? p1, char? p2) => (p0.Length > p1) ? p0 : (p0 + Linq.Expressions.VarChar(Linq.Expressions.Replicate(p2, p1 - p0.Length), 1000))));
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.PadLeft("", 0, ' ')), Linq.Expressions.L((string p0, int? p1, char? p2) => (p0.Length > p1) ? p0 : (Linq.Expressions.VarChar(Linq.Expressions.Replicate(p2, p1 - p0.Length), 1000) + p0)));
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.ConvertTo<string>.From(0m)), Linq.Expressions.L((decimal p) => Sql.TrimLeft(Sql.Convert<string, decimal>(p), '0')));
    if (!MapGuidAsString) {
      Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.ConvertTo<string>.From(Guid.Empty)), Linq.Expressions.L((Guid p) => Sql.Lower(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(Sql.Substring(Linq.Expressions.Hex(p), 7, 2) + Sql.Substring(Linq.Expressions.Hex(p), 5, 2), Sql.Substring(Linq.Expressions.Hex(p), 3, 2)), Sql.Substring(Linq.Expressions.Hex(p), 1, 2)), "-"), Sql.Substring(Linq.Expressions.Hex(p), 11, 2)), Sql.Substring(Linq.Expressions.Hex(p), 9, 2)), "-"), Sql.Substring(Linq.Expressions.Hex(p), 15, 2)), Sql.Substring(Linq.Expressions.Hex(p), 13, 2)), "-"), Sql.Substring(Linq.Expressions.Hex(p), 17, 4)), "-"), Sql.Substring(Linq.Expressions.Hex(p), 21, 12)))));
    }
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.Log(0m, 0m)), Linq.Expressions.L((decimal? m, decimal? n) => Sql.Log(n) / Sql.Log(m)));
    Linq.Expressions.MapMember(providerName, Linq.Expressions.M(() => Sql.Log(0.0, 0.0)), Linq.Expressions.L(((double? m, double? n) => Sql.Log(n) / Sql.Log(m))));
  }
}

public abstract class DB2iSeriesDataProvider_Base<TConnection, TDataReader> : DataProviderBase<TConnection, TDataReader>
  where TConnection : DbConnection, new()
  where TDataReader : IDataReader {
  protected DB2iSeriesConfiguration dB2ISeriesConfiguration;

  //public override string ConnectionNamespace { get; } = typeof(TConnection).Namespace;
  //public override Type DataReaderType { get; } = typeof(TDataReader);
  public DB2iSeriesDataProvider_Base(Func<ISchemaProvider> getSchemaProvider, DB2iSeriesVersion version = DB2iSeriesVersion.v5r4) : this(getSchemaProvider, new DB2iSeriesConfiguration {
    Version = version
  }) { }

  public DB2iSeriesDataProvider_Base(Func<ISchemaProvider> getSchemaProvider, DB2iSeriesConfiguration dB2ISeriesConfiguration)
    : base(dB2ISeriesConfiguration.Provider.Name(), dB2ISeriesConfiguration.MappingSchema, getSchemaProvider, GenericExtensions.GetTableOptions_DB2iSeries(null)) {
    this.dB2ISeriesConfiguration = dB2ISeriesConfiguration;
    dB2ISeriesConfiguration.InitDataProvider(this, base.SetCharField);
  }

  public override ISqlOptimizer GetSqlOptimizer() => new DB2iSeriesSqlOptimizer(base.SqlProviderFlags);

  //protected override IDbConnection CreateConnectionInternal(string connectionString) {
  //  TConnection val = new TConnection();
  //  val.ConnectionString = connectionString;
  //  return val;
  //}

  public override void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object? value) {
    if (dataType.DataType == DataType.DateTime2) {
      dataType = dataType.WithDataType(DataType.DateTime);
    }
    base.SetParameter(dataConnection, parameter, "@" + name, dataType, value);
  }

}

public abstract class DB2iSeriesSchemaProvider_Base<TConnection> : SchemaProviderBase where TConnection : IDbConnection {
  public abstract string GetLibList(DataConnection dataConnection);

  protected override List<ColumnInfo> GetColumns(DataConnection dataConnection, GetSchemaOptions option) {
    string sql = "\r\n        Select \r\n          Column_text \r\n        , case when CCSID = 65535 and Data_Type in ('CHAR', 'VARCHAR') then Data_Type || ' FOR BIT DATA' else Data_Type end as Data_Type\r\n        , Is_Identity\r\n        , Is_Nullable\r\n        , Length\r\n        , COALESCE(Numeric_Scale,0) Numeric_Scale\r\n        , Ordinal_Position\r\n        , Column_Name\r\n        , Table_Name\r\n        , Table_Schema\r\n        , Column_Name\r\n        From QSYS2/SYSCOLUMNS\r\n        where System_Table_Schema in('" + GetLibList(dataConnection) + "')\r\n         ";
    return dataConnection.Query(drf, sql).ToList();
    ColumnInfo drf(IDataReader dr) {
      ColumnInfo obj = new ColumnInfo {
        DataType = dr["Data_Type"].ToString().TrimEnd(Array.Empty<char>()),
        Description = dr["Column_Text"].ToString().TrimEnd(Array.Empty<char>()),
        IsIdentity = (dr["Is_Identity"].ToString().TrimEnd(Array.Empty<char>()) == "YES"),
        IsNullable = (dr["Is_Nullable"].ToString().TrimEnd(Array.Empty<char>()) == "Y"),
        Name = dr["Column_Name"].ToString().TrimEnd(Array.Empty<char>()),
        Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_Position"]),
        TableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["Table_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Table_Name"]).TrimEnd(Array.Empty<char>())
      };
      obj.SetColumnParameters(Convert.ToInt32(dr["Length"]), Convert.ToInt32(dr["Numeric_Scale"]));
      return obj;
    }
  }

  protected override DataType GetDataType(string? dataType, string? columnType, int? length, int? prec, int? scale) {
    switch (dataType) {
      case "BIGINT":
        return DataType.Int64;
      case "BINARY":
        return DataType.Binary;
      case "BLOB":
        return DataType.Blob;
      case "CHAR":
        return DataType.Char;
      case "CHAR FOR BIT DATA":
        return DataType.Binary;
      case "CLOB":
        return DataType.Text;
      case "DATALINK":
        return DataType.Undefined;
      case "DATE":
        return DataType.Date;
      case "DBCLOB":
        return DataType.Undefined;
      case "DECIMAL":
        return DataType.Decimal;
      case "DOUBLE":
        return DataType.Double;
      case "GRAPHIC":
        return DataType.Text;
      case "INTEGER":
        return DataType.Int32;
      case "NUMERIC":
        return DataType.Decimal;
      case "REAL":
        return DataType.Single;
      case "ROWID":
        return DataType.Undefined;
      case "SMALLINT":
        return DataType.Int16;
      case "TIME":
        return DataType.Time;
      case "TIMESTAMP":
        return DataType.Timestamp;
      case "VARBINARY":
        return DataType.VarBinary;
      case "VARCHAR":
        return DataType.VarChar;
      case "VARCHAR FOR BIT DATA":
        return DataType.VarBinary;
      case "VARGRAPHIC":
        return DataType.Text;
      default:
        return DataType.Undefined;
    }
  }

  protected override IReadOnlyCollection<ForeignKeyInfo> GetForeignKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
    string sql = "\r\n      Select ref.Constraint_Name \r\n      , fk.Ordinal_Position\r\n      , fk.Column_Name  As ThisColumn\r\n      , fk.Table_Name   As ThisTable\r\n      , fk.Table_Schema As ThisSchema\r\n      , uk.Column_Name  As OtherColumn\r\n      , uk.Table_Schema As OtherSchema\r\n      , uk.Table_Name   As OtherTable\r\n      From QSYS2/SYSREFCST ref\r\n      Join QSYS2/SYSKEYCST fk on(fk.Constraint_Schema, fk.Constraint_Name) = (ref.Constraint_Schema, ref.Constraint_Name)\r\n      Join QSYS2/SYSKEYCST uk on(uk.Constraint_Schema, uk.Constraint_Name) = (ref.Unique_Constraint_Schema, ref.Unique_Constraint_Name)\r\n      Where uk.Ordinal_Position = fk.Ordinal_Position\r\n      And fk.System_Table_Schema in('" + GetLibList(dataConnection) + "')\r\n      Order By ThisSchema, ThisTable, Constraint_Name, Ordinal_Position\r\n      ";
    Func<DbDataReader, ForeignKeyInfo> drf = (dr) => new ForeignKeyInfo {
      Name = dr["Constraint_Name"].ToString().TrimEnd(Array.Empty<char>()),
      Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_Position"]),
      OtherColumn = dr["OtherColumn"].ToString().TrimEnd(Array.Empty<char>()),
      OtherTableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["OtherSchema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["OtherTable"]).TrimEnd(Array.Empty<char>()),
      ThisColumn = dr["ThisColumn"].ToString().TrimEnd(Array.Empty<char>()),
      ThisTableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["ThisSchema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["ThisTable"]).TrimEnd(Array.Empty<char>())
    };
    return dataConnection.Query(drf, sql).ToList();
  }

  protected override IReadOnlyCollection<PrimaryKeyInfo> GetPrimaryKeys(DataConnection dataConnection, IEnumerable<TableSchema> tables, GetSchemaOptions options) {
    string sql = "\r\n      Select cst.constraint_Name  \r\n         , cst.table_SCHEMA\r\n         , cst.table_NAME \r\n         , col.Ordinal_position \r\n         , col.Column_Name   \r\n      From QSYS2/SYSKEYCST col\r\n      Join QSYS2/SYSCST    cst On(cst.constraint_SCHEMA, cst.constraint_NAME, cst.constraint_type) = (col.constraint_SCHEMA, col.constraint_NAME, 'PRIMARY KEY')\r\n      And cst.System_Table_Schema in('" + GetLibList(dataConnection) + "')\r\n      Order By cst.table_SCHEMA, cst.table_NAME, col.Ordinal_position\r\n      ";
    return dataConnection.Query(drf, sql).ToList();
    PrimaryKeyInfo drf(IDataReader dr) {
      return new PrimaryKeyInfo {
        ColumnName = Convert.ToString(dr["Column_Name"]).TrimEnd(Array.Empty<char>()),
        Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_position"]),
        PrimaryKeyName = Convert.ToString(dr["constraint_Name"]).TrimEnd(Array.Empty<char>()),
        TableID = dataConnection.Connection.Database + "." + Convert.ToString(dr["table_SCHEMA"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["table_NAME"]).TrimEnd(Array.Empty<char>())
      };
    }
  }

  protected override List<ProcedureInfo>? GetProcedures(DataConnection dataConnection, GetSchemaOptions options) {
    string sql = "\r\n      Select\r\n      CAST(CURRENT_SERVER AS VARCHAR(128)) AS Catalog_Name\r\n      , Function_Type\r\n      , Routine_Definition\r\n      , Routine_Name\r\n      , Routine_Schema\r\n      , Routine_Type\r\n      , Specific_Name\r\n      , Specific_Schema\r\n      From QSYS2/SYSROUTINES \r\n      Where Specific_Schema in('" + GetLibList(dataConnection) + "')\r\n      Order By Specific_Schema, Specific_Name\r\n      ";
    string defaultSchema = dataConnection.Execute<string>("select current_schema from sysibm/sysdummy1");
    return dataConnection.Query(drf, sql).ToList();
    ProcedureInfo drf(IDataReader dr) {
      return new ProcedureInfo {
        CatalogName = Convert.ToString(dr["Catalog_Name"]).TrimEnd(Array.Empty<char>()),
        IsDefaultSchema = (Convert.ToString(dr["Routine_Schema"]).TrimEnd(Array.Empty<char>()) == defaultSchema),
        IsFunction = (Convert.ToString(dr["Routine_Type"]) == "FUNCTION"),
        IsTableFunction = (Convert.ToString(dr["Function_Type"]) == "T"),
        ProcedureDefinition = Convert.ToString(dr["Routine_Definition"]).TrimEnd(Array.Empty<char>()),
        ProcedureID = dataConnection.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Specific_Name"]).TrimEnd(Array.Empty<char>()),
        ProcedureName = Convert.ToString(dr["Routine_Name"]).TrimEnd(Array.Empty<char>()),
        SchemaName = Convert.ToString(dr["Routine_Schema"]).TrimEnd(Array.Empty<char>())
      };
    }
  }

  protected override List<ProcedureParameterInfo>? GetProcedureParameters(DataConnection dataConnection, IEnumerable<ProcedureInfo> procedures, GetSchemaOptions options) {
    string sql = "\r\n      Select \r\n      CHARACTER_MAXIMUM_LENGTH\r\n      , Data_Type\r\n      , Numeric_Precision\r\n      , Numeric_Scale\r\n      , Ordinal_position\r\n      , Parameter_Mode\r\n      , Parameter_Name\r\n      , Specific_Name\r\n      , Specific_Schema\r\n      From QSYS2/SYSPARMS \r\n      where Specific_Schema in('" + GetLibList(dataConnection) + "')\r\n      Order By Specific_Schema, Specific_Name, Parameter_Name\r\n      ";
    Func<DbDataReader, ProcedureParameterInfo> drf = (dr) => new ProcedureParameterInfo {
      DataType = Convert.ToString(dr["Parameter_Name"]),
      IsIn = dr["Parameter_Mode"].ToString().Contains("IN"),
      IsOut = dr["Parameter_Mode"].ToString().Contains("OUT"),
      Length = Converter.ChangeTypeTo<int?>(dr["CHARACTER_MAXIMUM_LENGTH"]),
      Ordinal = Converter.ChangeTypeTo<int>(dr["Ordinal_position"]),
      ParameterName = Convert.ToString(dr["Parameter_Name"]).TrimEnd(Array.Empty<char>()),
      Precision = Converter.ChangeTypeTo<int?>(dr["Numeric_Precision"]),
      ProcedureID = dataConnection.Connection.Database + "." + Convert.ToString(dr["Specific_Schema"]).TrimEnd(Array.Empty<char>()) + "." + Convert.ToString(dr["Specific_Name"]).TrimEnd(Array.Empty<char>()),
      Scale = Converter.ChangeTypeTo<int?>(dr["Numeric_Scale"])
    };
    return dataConnection.Query(drf, sql).ToList();
  }

  protected override string GetProviderSpecificTypeNamespace() {
    return typeof(TConnection).Namespace;
  }

  protected override List<TableInfo> GetTables(DataConnection dataConnection, GetSchemaOptions options) {
    string sql = "\r\n          Select \r\n          CAST(CURRENT_SERVER AS VARCHAR(128)) AS Catalog_Name\r\n          , Table_Schema\r\n          , Table_Name\r\n          , Table_Text\r\n          , Table_Type\r\n          , System_Table_Schema\r\n          From QSYS2/SYSTABLES \r\n          Where Table_Type In('L', 'P', 'T', 'V')\r\n          And System_Table_Schema in ('" + GetLibList(dataConnection) + "')\t\r\n          Order By System_Table_Schema, System_Table_Name\r\n         ";
    string defaultSchema = dataConnection.Execute<string>("select current_schema from sysibm/sysdummy1");
    Func<DbDataReader, TableInfo> drf = (dr) => new TableInfo {
      CatalogName = dr["Catalog_Name"].ToString().TrimEnd(Array.Empty<char>()),
      Description = dr["Table_Text"].ToString().TrimEnd(Array.Empty<char>()),
      IsDefaultSchema = (dr["System_Table_Schema"].ToString().TrimEnd(Array.Empty<char>()) == defaultSchema),
      IsView = new string[2]
      {
        "L",
        "V"
      }.Contains(dr["Table_Type"].ToString()),
      SchemaName = dr["Table_Schema"].ToString().TrimEnd(Array.Empty<char>()),
      TableID = dataConnection.Connection.Database + "." + dr["Table_Schema"].ToString().TrimEnd(Array.Empty<char>()) + "." + dr["Table_Name"].ToString().TrimEnd(Array.Empty<char>()),
      TableName = dr["Table_Name"].ToString().TrimEnd(Array.Empty<char>())
    };
    return dataConnection.Query(drf, sql).ToList();
  }
}

public abstract partial class DB2iSeriesSqlBuilder_Base : BasicSqlBuilder {
  protected DB2iSeriesSqlBuilder_Base(BasicSqlBuilder parentBuilder, DB2iSeriesConfiguration dB2ISeriesConfiguration) : base(parentBuilder) {
    this.dB2ISeriesConfiguration = dB2ISeriesConfiguration;
  }

  protected DB2iSeriesSqlBuilder_Base(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags, DB2iSeriesConfiguration dB2ISeriesConfiguration)
    : base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags) {
    this.dB2ISeriesConfiguration = dB2ISeriesConfiguration;
    MapGuidAsString = sqlProviderFlags.CustomFlags.Contains("MapGuidAsString");
  }

  protected DB2iSeriesConfiguration dB2ISeriesConfiguration  = new DB2iSeriesConfiguration();

  public bool MapGuidAsString { get; protected set; }
  private bool IsVersion7_2orLater => dB2ISeriesConfiguration.Version >= DB2iSeriesVersion.v7r2;
  protected override bool OffsetFirst => IsVersion7_2orLater || base.OffsetFirst;
  private string DummyTableSql(string prefix, string suffix) => $"{prefix}{dB2ISeriesConfiguration.NamingConvention.DummyTableName()}{suffix}";
  protected override void BuildColumnExpression(SelectQuery selectQuery, ISqlExpression expr, string alias, ref bool addAlias) => BuildColumnExpression(selectQuery, expr, alias, ref addAlias, wrapParameter: true);

  protected void BuildColumnExpression(SelectQuery selectQuery, ISqlExpression expr, string alias, ref bool addAlias, bool wrapParameter) {
    bool wrap = false;
    if (expr.SystemType == typeof(bool)) {
      if (expr is SqlSearchCondition) {
        wrap = true;
      } else {
        wrap = expr is SqlExpression ex && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SqlSearchCondition;
      }
    }
    if (wrapParameter) {
      if (expr is SqlParameter) {
        if (((SqlParameter)expr).Name != null) {
          SqlDataType dataType = SqlDataType.GetDataType(expr.SystemType);
          expr = new SqlFunction(expr.SystemType, dataType.Type.DataType.ToString(), expr);
        }
      } else if (expr is SqlValue && ((SqlValue)expr).Value == null) {
        string colType = expr.SystemType.GetTypeForCast(MapGuidAsString);
        expr = new SqlExpression(expr.SystemType, "Cast({0} as {1})", 100, expr, new SqlExpression(colType, 100));
      }
    }
    if (wrap) {
      StringBuilder.Append("CASE WHEN ");
    }
    base.BuildColumnExpression(selectQuery, expr, alias, ref addAlias);
    if (wrap) {
      StringBuilder.Append(" THEN 1 ELSE 0 END");
    }
  }

  protected override void BuildCommand(SqlStatement selectQuery, int commandNumber) {
    StringBuilder.AppendLine(DummyTableSql("SELECT identity_val_local() FROM ", string.Empty));
  }

  protected override void BuildCreateTableIdentityAttribute1(SqlField field) => StringBuilder.Append("GENERATED ALWAYS AS IDENTITY");

  protected override void BuildDataTypeFromDataType(SqlDataType type, bool forCreateTable) {
    DataType dataType = type.Type.DataType;
    if ((uint)(dataType - 28) <= 1u) {
      StringBuilder.Append("timestamp");
    } else {
      base.BuildDataTypeFromDataType(type, forCreateTable);
    }
  }

  protected override void BuildEmptyInsert(SqlInsertClause insertClause) {
    StringBuilder.Append("VALUES");
    foreach (var field in insertClause.Into.Fields) {
      StringBuilder.Append("(DEFAULT)");
    }
    StringBuilder.AppendLine();
  }



  protected override void BuildFromClause(SqlStatement statement, SelectQuery selectQuery) {
    if (!statement.IsUpdate()) {
      base.BuildFromClause(statement, selectQuery);
    }
  }

  protected override void BuildInsertOrUpdateQuery(SqlInsertOrUpdateStatement insertOrUpdate) => BuildInsertOrUpdateQueryAsMerge(insertOrUpdate, DummyTableSql("FROM ", " FETCH FIRST 1 ROW ONLY"));

  protected override void BuildInsertOrUpdateQueryAsMerge(SqlInsertOrUpdateStatement insertOrUpdate, string fromDummyTable) {
    SqlTable table = insertOrUpdate.Insert.Into;
    string targetAlias = Convert(StringBuilder, insertOrUpdate.SelectQuery.From.Tables[0].Alias, ConvertType.NameToQueryTableAlias).ToString();
    string sourceAlias = Convert(StringBuilder, GetTempAliases(1, "s")[0], ConvertType.NameToQueryTableAlias).ToString();
    List<SqlSetExpression> keys = insertOrUpdate.Update.Keys;
    AppendIndent().Append("MERGE INTO ");
    BuildPhysicalTable(table, null);
    StringBuilder.Append(' ').AppendLine(targetAlias);
    AppendIndent().Append("USING (SELECT ");
    //ExtractMergeParametersIfCannotCombine(insertOrUpdate, keys);
    for (int j = 0; j < keys.Count; j++) {
      SqlSetExpression key = keys[j];
      ISqlExpression expr = key.Expression;
      if (expr is SqlParameter || expr is SqlValue) {
        string asType = SqlDataType.GetDataType(expr.SystemType).GetiSeriesType(MapGuidAsString);
        StringBuilder.Append("CAST(");
        BuildExpression(expr, buildTableName: false, checkParentheses: false);
        StringBuilder.AppendFormat(" AS {0})", asType);
      } else {
        BuildExpression(expr, buildTableName: false, checkParentheses: false);
      }
      StringBuilder.Append(" AS ");
      BuildExpression(key.Column, buildTableName: false, checkParentheses: false);
      if (j + 1 < keys.Count) {
        StringBuilder.Append(", ");
      }
    }
    if (!string.IsNullOrEmpty(fromDummyTable)) {
      StringBuilder.Append(' ').Append(fromDummyTable);
    }
    StringBuilder.Append(") ").Append(sourceAlias).AppendLine(" ON");
    AppendIndent().AppendLine("(");
    Indent++;
    for (int i = 0; i < keys.Count; i++) {
      SqlSetExpression key2 = keys[i];
      AppendIndent();
      StringBuilder.Append(targetAlias).Append('.');
      BuildExpression(key2.Column, buildTableName: false, checkParentheses: false);
      StringBuilder.Append(" = ").Append(sourceAlias).Append('.');
      BuildExpression(key2.Column, buildTableName: false, checkParentheses: false);
      if (i + 1 < keys.Count) {
        StringBuilder.Append(" AND");
      }
      StringBuilder.AppendLine();
    }
    Indent--;
    AppendIndent().AppendLine(")");
    if (insertOrUpdate.Update.Items.Any()) {
      AppendIndent().AppendLine("WHEN MATCHED THEN");
      Indent++;
      AppendIndent().AppendLine("UPDATE ");
      BuildUpdateSet(insertOrUpdate.SelectQuery, insertOrUpdate.Update);
      Indent--;
    }
    AppendIndent().AppendLine("WHEN NOT MATCHED THEN");
    Indent++;
    BuildInsertClause(insertOrUpdate, insertOrUpdate.Insert, "INSERT", appendTableName: false, addAlias: false);
    Indent--;
    while (EndLine.Contains(StringBuilder[StringBuilder.Length - 1])) {
      StringBuilder.Length--;
    }
  }

  protected override void BuildUpdateSet(SelectQuery selectQuery, SqlUpdateClause updateClause) {
    AppendIndent().AppendLine("SET");
    Indent++;
    bool first = true;
    foreach (SqlSetExpression expr in updateClause.Items) {
      if (!first) {
        StringBuilder.Append(',').AppendLine();
      }
      first = false;
      AppendIndent();
      BuildExpression(expr.Column, SqlProviderFlags.IsUpdateSetTableAliasSupported, checkParentheses: true, throwExceptionIfTableNotFound: false);
      StringBuilder.Append(" = ");
      bool addAlias = false;
      BuildColumnExpression(selectQuery, expr.Expression, null, ref addAlias, wrapParameter: false);
    }
    Indent--;
    StringBuilder.AppendLine();
  }

  protected override void BuildSelectClause(SelectQuery selectQuery) {
    if (selectQuery.HasSetOperators) {
      SelectQuery topquery = selectQuery;
      while (topquery.ParentSelect != null && topquery.ParentSelect.HasSetOperators) {
        topquery = topquery.ParentSelect;
      }
      string[] alia = selectQuery.Select.Columns.Select((SqlColumn c) => c.Alias).ToArray();
      selectQuery.SetOperators.ForEach(delegate (SqlSetOperator u) {
        int colNo = 0;
        u.SelectQuery.Select.Columns.ForEach(delegate (SqlColumn c) {
          c.Alias = alia[colNo];
          colNo++;
        });
      });
    }
    if (selectQuery.From.Tables.Count == 0) {
      AppendIndent().AppendLine("SELECT");
      BuildColumns(selectQuery);
      AppendIndent().AppendLine(DummyTableSql("FROM ", "FETCH FIRST 1 ROW ONLY"));
    } else {
      base.BuildSelectClause(selectQuery);
    }
  }

  protected override IEnumerable<SqlColumn> GetSelectedColumns(SelectQuery selectQuery) => Obsolete_NeedSkip(selectQuery) && !selectQuery.OrderBy.IsEmpty
      ? AlternativeGetSelectedColumns(selectQuery, () => base.GetSelectedColumns(selectQuery))
      : base.GetSelectedColumns(selectQuery);

  public override int CommandCount(SqlStatement statement) {
    SqlInsertStatement insertStatement = statement as SqlInsertStatement;
    return insertStatement == null || !insertStatement.Insert.WithIdentity ? 1 : 2;
  }

  public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType) {
    switch (convertType) {
      case ConvertType.NameToQueryParameter:
        return sb.Append($"@{value}");
      case ConvertType.NameToCommandParameter:
      case ConvertType.NameToSprocParameter:
        return sb.Append($":{value}");
      case ConvertType.SprocParameterToName:
        if (value != null) {
          string str = value.ToString();
          if (str.Length <= 0 || str[0] != ':') {
            return sb.Append(str);
          }
          return sb.Append(str.Substring(1));
        }
        break;
      case ConvertType.NameToQueryField:
      case ConvertType.NameToQueryFieldAlias:
      case ConvertType.NameToQueryTable:
      case ConvertType.NameToQueryTableAlias:
        if (value != null && dB2ISeriesConfiguration.IdentifierQuoteMode != 0) {
          string name = value.ToString();
          if (name.Length > 0 && name[0] == '"') {
            return sb.Append(name);
          }
          if (dB2ISeriesConfiguration.IdentifierQuoteMode == DB2iSeriesIdentifierQuoteMode.Quote || name.StartsWith("_") || name.Any((char c) => char.IsWhiteSpace(c))) {
            return sb.Append("\"" + name + "\"");
          }
        }
        break;
    }
    return sb.Append(value);
  }

  protected abstract override ISqlBuilder CreateSqlBuilder();

  protected abstract override string? GetProviderTypeName(IDataContext dataContext, DbParameter parameter);

  protected override void BuildCreateTableNullAttribute(SqlField field, DefaultNullable defaulNullable) {
    if ((defaulNullable != DefaultNullable.Null || !field.CanBeNull) && (defaulNullable != DefaultNullable.NotNull || field.CanBeNull)) {
      StringBuilder.Append(field.CanBeNull ? " " : "NOT NULL");
    }
  }

  protected override void BuildPredicate(ISqlPredicate predicate) {
    ISqlPredicate newpredicate = predicate;
    switch (predicate.ElementType) {
      case QueryElementType.LikePredicate: {
          SqlPredicate.Like p = (SqlPredicate.Like)predicate;
          ISqlExpression param2 = (p.Expr2 as Obsolete_IValueContainer).GetParmeter(p.Expr1.SystemType);
          if (param2 != null) {
            SqlValue value = param2 as SqlValue;
            newpredicate = (value == null || value.Value != null)
              ? new SqlPredicate.Like(p.Expr1, p.IsNot, param2, p.Escape, p.FunctionName)
              : (!p.IsNot)
              ? new SqlPredicate.ExprExpr(p.Expr1, SqlPredicate.Operator.Equal, p.Expr2, p.CanBeNull)
              : new SqlPredicate.ExprExpr(p.Expr1, SqlPredicate.Operator.NotEqual, p.Expr2, p.CanBeNull);
          }
          break;
        }
      case QueryElementType.ExprExprPredicate: {
          SqlPredicate.ExprExpr ep = (SqlPredicate.ExprExpr)predicate;
          SqlFunction function = ep.Expr1 as SqlFunction;
          if (function != null && function.Name == "Date") {
            SqlParameter parameter = ep.Expr2 as SqlParameter;
            if (parameter != null) {
              parameter.Type = parameter.Type.WithDataType(DataType.Date);
            }
          }
          break;
        }
    }
    base.BuildPredicate(newpredicate);
  }

  protected override void BuildInsertQuery(SqlStatement statement, SqlInsertClause insertClause, bool addAlias) {
    BuildStep = Step.InsertClause;
    BuildInsertClause(statement, insertClause, addAlias);
    BuildStep = Step.WithClause;
    BuildWithClause(statement.GetWithClause());
    if (statement.QueryType == QueryType.Insert && statement.SelectQuery.From.Tables.Count != 0) {
      BuildStep = Step.SelectClause;
      BuildSelectClause(statement.SelectQuery);
      BuildStep = Step.FromClause;
      BuildFromClause(statement, statement.SelectQuery);
      BuildStep = Step.WhereClause;
      BuildWhereClause(statement.SelectQuery);
      BuildStep = Step.GroupByClause;
      BuildGroupByClause(statement.SelectQuery);
      BuildStep = Step.HavingClause;
      BuildHavingClause(statement.SelectQuery);
      BuildStep = Step.OrderByClause;
      BuildOrderByClause(statement.SelectQuery);
      BuildStep = Step.OffsetLimit;
      BuildOffsetLimit(statement.SelectQuery);
    }
    if (insertClause.WithIdentity) {
      BuildGetIdentity(insertClause);
    }
  }

  protected override void BuildDeleteQuery(SqlDeleteStatement deleteStatement) {
    if (deleteStatement.With != null) {
      throw new NotSupportedException("iSeries doesn't support Cte in Delete statement");
    }
    base.BuildDeleteQuery(deleteStatement);
  }

  protected override void BuildUpdateQuery(SqlStatement statement, SelectQuery selectQuery, SqlUpdateClause updateClause) {
    if (statement.GetWithClause() != null) {
      throw new NotSupportedException("iSeries doesn't support Cte in Update statement");
    }
    base.BuildUpdateQuery(statement, selectQuery, updateClause);
  }

  protected override string OffsetFormat(SelectQuery selectQuery) => !IsVersion7_2orLater ? base.OffsetFormat(selectQuery) : "OFFSET {0} ROWS";

  protected override string LimitFormat(SelectQuery selectQuery) => !IsVersion7_2orLater
      ? selectQuery.Select.SkipValue != null ? null : " FETCH FIRST {0} ROWS ONLY"
      : "FETCH FIRST {0} ROWS ONLY";

  protected override void BuildSql() {
    if (IsVersion7_2orLater) {
      base.BuildSql();
    } else {
      Obsolete_AlternativeBuildSql(implementOrderBy: true, base.BuildSql, "\t0");
    }
  }

  protected override void BuildTruncateTableStatement(SqlTruncateTableStatement truncateTable) {
    if (IsVersion7_2orLater) {
      SqlTable table = truncateTable.Table;
      AppendIndent();
      StringBuilder.Append("TRUNCATE TABLE ");
      BuildPhysicalTable(table, null);
      if (truncateTable.ResetIdentity) {
        StringBuilder.Append(" RESTART IDENTITY");
      }
    } else {
      base.BuildTruncateTableStatement(truncateTable);
    }
  }
}
