﻿using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using LinqToDB.SqlQuery;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Data.GetSchemaTyped.DataRows;
using LinqToDB.DataProvider.DB2iSeries;
using System.Data;
using System.Data.Common;

namespace LinqToDB.DataProvider {

  public static class GenericExtensions {

    public static ISqlExpression AlternativeExists_DB2iSeries(this SqlFunction func) {
      var query = (SelectQuery)func.Parameters[0];
      if (query.Select.Columns.Count == 0) {
        query.Select.Columns.Add(new SqlColumn(query, new SqlExpression("'.'")));
      }
      query.Select.Take(1, null);
      return new SqlSearchCondition(new SqlCondition(false, new SqlPredicate.IsNull(query, true)));
    }

    [UrlAsAt.AccessMappingSchema_2021_05_07] static void AppendConversion_Access(this StringBuilder stringBuilder, int value) => stringBuilder.Append($"chr({value})");
    [UrlAsAt.DB2MappingSchema_2021_05_07] static void AppendConversion_DB2(this StringBuilder stringBuilder, int value) => stringBuilder.Append($"chr({value})");
    static void AppendConversion_DB2iSeries(this StringBuilder stringBuilder, int value) => stringBuilder.Append($"varchar({value})");
    [UrlAsAt.SapHanaMappingSchema_2021_05_07] static void AppendConversion_SapHana(this StringBuilder stringBuilder, int value) => stringBuilder.Append($"char({value})");
    [UrlAsAt.SqlServerMappingSchema_2021_05_07] static void AppendConversion_SqlServer(this StringBuilder stringBuilder, int value) => stringBuilder.Append($"char({value})");

    [UrlAsAt.AccessMappingSchema_2021_05_07] static readonly Action<StringBuilder, int> AppendConversionAction_Access = AppendConversion_Access;
    [UrlAsAt.DB2MappingSchema_2021_05_07] static readonly Action<StringBuilder, int> AppendConversionAction_DB2 = AppendConversion_DB2;
    [UrlAsAt.SqlServerMappingSchema_2021_05_07] static readonly Action<StringBuilder, int> AppendConversionAction_SqlServer = AppendConversion_SqlServer;

    public static void ConvertBinaryToSql(this StringBuilder stringBuilder, byte[] value, string prefix, string suffix) {
      stringBuilder.Append(prefix);
      foreach (var b in value)
        stringBuilder.Append(b.ToString("X2"));
      stringBuilder.Append(suffix);
    }
    [UrlAsAt.AccessMappingSchema_2021_05_07] public static void ConvertBinaryToSql_Access(this StringBuilder stringBuilder, byte[] value) => stringBuilder.ConvertBinaryToSql(value, "0x", "");
    [UrlAsAt.DB2MappingSchema_2021_05_07] public static void ConvertBinaryToSql_DB2(this StringBuilder stringBuilder, byte[] value) => stringBuilder.ConvertBinaryToSql(value, "BX", "'");
     public static void ConvertBinaryToSql_DB2iSeries(this StringBuilder stringBuilder, byte[] value) => stringBuilder.ConvertBinaryToSql(value, "BX", "'");
    [UrlAsAt.SapHanaMappingSchema_2021_05_07] public static void ConvertBinaryToSql_SapHana(this StringBuilder stringBuilder, byte[] value) => stringBuilder.ConvertBinaryToSql(value, "x'", "'");
    [UrlAsAt.SqlServerMappingSchema_2021_05_07] public static void ConvertBinaryToSql_SqlServer(this StringBuilder stringBuilder, byte[] value) => stringBuilder.Append("0x").AppendByteArrayAsHexViaLookup32(value);

    [UrlAsAt.AccessMappingSchema_2021_05_07] public static void ConvertCharToSql_Access(this StringBuilder stringBuilder, char value) => DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion_Access, value);
    [UrlAsAt.DB2MappingSchema_2021_05_07] public static void ConvertCharToSql_DB2(this StringBuilder stringBuilder, char value) => DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion_DB2, value);
    public static void ConvertCharToSql_DB2iSeries(this StringBuilder stringBuilder, char value) => DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion_DB2iSeries, value);
    [UrlAsAt.SapHanaMappingSchema_2021_05_07] public static void ConvertCharToSql_SapHana(this StringBuilder stringBuilder, char value) => DataTools.ConvertCharToSql(stringBuilder, "'", AppendConversion_SapHana, value);
    [UrlAsAt.SqlServerMappingSchema_2021_05_07] public static void ConvertCharToSql_SqlServer(this StringBuilder stringBuilder, SqlDataType sqlDataType, char value) => DataTools.ConvertCharToSql(stringBuilder, new[] { DataType.Char, DataType.Text, DataType.VarChar }.Contains(sqlDataType.Type.DataType) ? "'" : "N'", AppendConversionAction_SqlServer, value);

    [UrlAsAt.AccessMappingSchema_2021_05_07] public static void ConvertDateTimeToSql_Access(this StringBuilder stringBuilder, DateTime value) => stringBuilder.Append(value.ToString("#" + DateTimeConstants.IsoDateFormat + (value.Hour == 0 && value.Minute == 0 && value.Second == 0 ? "" : " " + DateTimeConstants.IsoTimeFormat) + "#"));
    [UrlAsAt.DB2MappingSchema_2021_05_07] public static void ConvertDateTimeToSql_DB2(this StringBuilder stringBuilder, SqlDataType sqlDataType, DateTime value) => stringBuilder.Append(value.ToSqlString_ISO(sqlDataType, "'", "-", 6, "'"));
    public static void ConvertDateTimeToSql_DB2iSeries(this StringBuilder stringBuilder, SqlDataType sqlDataType, DateTime value) => stringBuilder.Append(value.ToSqlString_ISO(sqlDataType, "'", " ", 6, "'"));
    [UrlAsAt.SqlServerMappingSchema_2021_05_07] public static void ConvertDateTimeToSql_SqlServer(this StringBuilder stringBuilder, SqlDataType sqlDataType, DateTime value) => stringBuilder.Append(value.ToSqlString_ISO(sqlDataType, "'", "T", sqlDataType.Type.DataType == DataType.DateTime2 ? 7 : 3, "'"));

    [UrlAsAt.SqlServerMappingSchema_2021_05_07] public static void ConvertDateTimeOffsetToSql_SqlServer(this StringBuilder stringBuilder, SqlDataType sqlDataType, DateTimeOffset value) => stringBuilder.Append(value.DateTime.ToSqlString_ISO(sqlDataType, "'", " ", 7, " zzz'"));

    [UrlAsAt.AccessMappingSchema_2021_05_07] public static void ConvertGuidToSql_Access(this StringBuilder stringBuilder, Guid value) => stringBuilder.Append($"'{value.ToString("B")}'");
    [UrlAsAt.DB2MappingSchema_2021_05_07]
    public static void ConvertGuidToSql_DB2(this StringBuilder stringBuilder, Guid value) {
      var s = value.ToString("N");
      stringBuilder
        .Append("Cast(x'")
        .Append(s.Substring(6, 2))
        .Append(s.Substring(4, 2))
        .Append(s.Substring(2, 2))
        .Append(s.Substring(0, 2))
        .Append(s.Substring(10, 2))
        .Append(s.Substring(8, 2))
        .Append(s.Substring(14, 2))
        .Append(s.Substring(12, 2))
        .Append(s.Substring(16, 16))
        .Append("' as char(16) for bit data)");
    }
    public static void ConvertGuidToSql_DB2iSeries(this StringBuilder stringBuilder, Guid value) => stringBuilder.ConvertGuidToSql_DB2(value);

    [UrlAsAt.AccessMappingSchema_2021_05_07] public static void ConvertStringToSql_Access(this StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "+", null, AppendConversion_Access, value, null);
    [UrlAsAt.DB2MappingSchema_2021_05_07] public static void ConvertStringToSql_DB2(this StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", null, AppendConversion_DB2, value, null);
    public static void ConvertStringToSql_DB2iSeries(this StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", "", AppendConversion_DB2iSeries, value, null);
    [UrlAsAt.SapHanaMappingSchema_2021_05_07] public static void ConvertStringToSql_SapHana(this StringBuilder stringBuilder, string value) => DataTools.ConvertStringToSql(stringBuilder, "||", null, AppendConversion_SapHana, value, null);
    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public static void ConvertStringToSql_SqlServer(this StringBuilder stringBuilder, SqlDataType sqlDataType, string value) {
      string? startPrefix;
      switch (sqlDataType.Type.DataType) {
        case DataType.Char:
        case DataType.VarChar:
        case DataType.Text:
          startPrefix = null;
          break;
        default:
          startPrefix = "N";
          break;
      }
      DataTools.ConvertStringToSql(stringBuilder, "+", startPrefix, AppendConversionAction_SqlServer, value, null);
    }

    [UrlAsAt.DB2MappingSchema_2021_05_07] public static void ConvertTimeToSql_DB2(this StringBuilder stringBuilder, TimeSpan time) => stringBuilder.Append($"'{time.ToString(DateTimeConstants.IsoTimeFormat)}'");
    public static void ConvertTimeToSql_DB2iSeries(this StringBuilder stringBuilder, TimeSpan time) => stringBuilder.Append($"'{time.ToString(DateTimeConstants.IsoTimeFormat)}'");
    [UrlAsAt.SqlServerMappingSchema_2021_05_07]
    public static void ConvertTimeSpanToSql_SqlServer(this StringBuilder stringBuilder, SqlDataType sqlDataType, TimeSpan value) {
      if (sqlDataType.Type.DataType == DataType.Int64) {
        stringBuilder.Append(value.Ticks);
      } else {
        var format = value.Days > 0
          ? DateTimeConstants.TimeSpanFormat(value.Ticks % 10000000 != 0 ? 7 : 0)
          : DateTimeConstants.SqlTimeFormat(value.Ticks % 10000000 != 0 ? 7 : 0);
        stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, value);
      }
    }

    public static IDbConnection CreateIDbConnection(string connectionTypeName, string connectionString)
      => CreateIDbConnection(Type.GetType(connectionTypeName, true, true), connectionString);

    public static IDbConnection CreateIDbConnection(Type connectionType, string connectionString) {
      var instance = Activator.CreateInstance(connectionType);
      if (instance != null) {
        var conn = (IDbConnection)instance;
        conn.ConnectionString = connectionString;
        return conn;
      }
      throw new Exception();
    }

    public static T CreateIDbConnection<T>(string connectionString) where T : IDbConnection, new() => new() { ConnectionString = connectionString };

    [UrlAsAt.DB2MappingSchema_2021_05_07]
    static readonly string[] DateParseFormats_DB2 = new[]    {
      "yyyy-MM-dd",
      "yyyy-MM-dd-HH.mm.ss",
      "yyyy-MM-dd-HH.mm.ss.f",
      "yyyy-MM-dd-HH.mm.ss.ff",
      "yyyy-MM-dd-HH.mm.ss.fff",
      "yyyy-MM-dd-HH.mm.ss.ffff",
      "yyyy-MM-dd-HH.mm.ss.fffff",
      "yyyy-MM-dd-HH.mm.ss.ffffff",
      "yyyy-MM-dd-HH.mm.ss.fffffff",
      "yyyy-MM-dd-HH.mm.ss.ffffffff",
      "yyyy-MM-dd-HH.mm.ss.fffffffff",
      "yyyy-MM-dd-HH.mm.ss.ffffffffff",
      "yyyy-MM-dd-HH.mm.ss.fffffffffff",
      "yyyy-MM-dd-HH.mm.ss.ffffffffffff",
    };

    //private static readonly string[] DateParseFormats_DB2iSeries = new[]    {
    //  "yyyy-MM-dd",
    //  "yyyy-MM-dd-HH.mm.ss",
    //  "yyyy-MM-dd-HH.mm.ss.f",
    //  "yyyy-MM-dd-HH.mm.ss.ff",
    //  "yyyy-MM-dd-HH.mm.ss.fff",
    //  "yyyy-MM-dd-HH.mm.ss.ffff",
    //  "yyyy-MM-dd-HH.mm.ss.fffff",
    //  "yyyy-MM-dd-HH.mm.ss.ffffff",
    //  "yyyy-MM-dd-HH.mm.ss.fffffff",
    //  "yyyy-MM-dd-HH.mm.ss.ffffffff",
    //  "yyyy-MM-dd-HH.mm.ss.fffffffff",
    //  "yyyy-MM-dd-HH.mm.ss.ffffffffff",
    //  "yyyy-MM-dd-HH.mm.ss.fffffffffff",
    //  "yyyy-MM-dd-HH.mm.ss.ffffffffffff",
    //};

    public static string? FixUnderscore_DB2iSeries(this string? text, string alternative) {
      if (string.IsNullOrWhiteSpace(text)) {
        return null;
      }
      if (text == "_") {
        return "underscore_";
      }
      if (!text.All((char t) => Enumerable.Contains("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", t))) {
        return alternative;
      }
      return text;
    }

    public static SqlDataType GetTypeOrUnderlyingTypeDataType_DB2iSeries(this MappingSchema mappingSchema, Type type) {
      var sqlDataType = mappingSchema.GetDataType(type);
      if (sqlDataType.Type.DataType == DataType.Undefined) {
        sqlDataType = mappingSchema.GetUnderlyingDataType(type, out var _);
      }
      if (sqlDataType.Type.DataType != 0) {
        return sqlDataType;
      }
      return SqlDataType.Undefined;
    }

    public static string GetNameWithVersion(this Version version, string name) => $"{name}.v{version}";

    public static TableOptions GetTableOptions(this DataSourceInformationRow dataSourceInformationRow) => dataSourceInformationRow.DataSourceProduct?.DbSystem?.Name switch {
      DbSystem.Names.Access => TableOptions.None,
      DataSourceProduct.Names.DB2_400_SQL => dataSourceInformationRow.Version.GetTableOptions_DB2iSeries(),
      //{ DataSourceProductName }
      _ => TableOptions.None
    };

    public static TableOptions GetTableOptions_DB2iSeries(this Version? version) => version switch {
      //{ Major: > 5 } => TableOptions.None,
      _ => TableOptions.IsTemporary
         | TableOptions.IsLocalTemporaryStructure
         | TableOptions.IsGlobalTemporaryStructure
         | TableOptions.IsLocalTemporaryData
         | TableOptions.IsGlobalTemporaryData
    };

    [Obsolete("Please use the BulkCopy extension methods within DataConnectionExtensions")]
    public static BulkCopyRowsCopied MultipleRowsCopy_DB2iSeries<T>(this DataConnection dataConnection, IEnumerable<T> source, int maxBatchSize = 1000, Action<BulkCopyRowsCopied>? rowsCopiedCallback = null) where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
      BulkCopyType = BulkCopyType.MultipleRows,
      MaxBatchSize = maxBatchSize,
      RowsCopiedCallback = rowsCopiedCallback
    }, source);

    [UrlAsAt.DB2MappingSchema_2021_05_07]
    public static DateTime ParseDateTime_DB2(string value) {
      if (DateTime.TryParse(value, out var res))
        return res;
      return DateTime.ParseExact(value, DateParseFormats_DB2, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    //public static DateTime ParseDateTime_DB2iSeries(this string value) {
    //  if (DateTime.TryParse(value, out var res))
    //    return res;
    //  return DateTime.ParseExact(value, DateParseFormats_DB2iSeries, CultureInfo.InvariantCulture, DateTimeStyles.None);
    //}

    [Obsolete("Please use the BulkCopy extension methods within DataConnectionExtensions")]
    public static BulkCopyRowsCopied ProviderSpecificBulkCopy_DB2iSeries<T>(this DataConnection dataConnection, IEnumerable<T> source, int bulkCopyTimeout = 0, bool keepIdentity = false, int notifyAfter = 0, Action<BulkCopyRowsCopied>? rowsCopiedCallback = null) where T : class => dataConnection.BulkCopy(new BulkCopyOptions {
      BulkCopyType = BulkCopyType.ProviderSpecific,
      BulkCopyTimeout = bulkCopyTimeout,
      KeepIdentity = keepIdentity,
      NotifyAfter = notifyAfter,
      RowsCopiedCallback = rowsCopiedCallback
    }, source);

    //public static string TimestampFormat_DB2(this SqlDataType type) {
    //  var precision = type.Type.Precision;
    //  if (precision == null && type.Type.DbType != null) {
    //    var dbtype = type.Type.DbType.ToLowerInvariant();
    //    if (dbtype.StartsWith("timestamp(") && int.TryParse(dbtype.Substring(10, dbtype.Length - 11), out var fromDbType)) {
    //      precision = fromDbType;
    //    }
    //  }
    //  precision = precision == null || precision < 0 ? 6 : (precision > 7 ? 7 : precision);
    //  return precision switch {
    //    0 => "yyyy-MM-dd-HH.mm.ss",
    //    1 => "yyyy-MM-dd-HH.mm.ss.f",
    //    2 => "yyyy-MM-dd-HH.mm.ss.ff",
    //    3 => "yyyy-MM-dd-HH.mm.ss.fff",
    //    4 => "yyyy-MM-dd-HH.mm.ss.ffff",
    //    5 => "yyyy-MM-dd-HH.mm.ss.fffff",
    //    6 => "yyyy-MM-dd-HH.mm.ss.ffffff",
    //    7 => "yyyy-MM-dd-HH.mm.ss.fffffff",
    //    _ => throw new InvalidOperationException(),
    //  };
    //}


    //public static string TimestampFormat_ISO(int milliSecondsPrecision, string dateTimeSeparator)
    //  => DateFormat_ISO + dateTimeSeparator + TimeFormat_ISO + ((milliSecondsPrecision > 0) ? ("." + new string('f', milliSecondsPrecision)) : "");

    public static string ToSqlString_ISO(this DateTime value, SqlDataType sqlDatatype, string prefix, string dateTimeSeparator, int maxMilliSecondsPrecision, string suffix) {
      var format = (sqlDatatype.Type.DataType == DataType.Date || "date".Equals(sqlDatatype.Type.DbType, StringComparison.OrdinalIgnoreCase))
        ? value.SqlLiteralDate(prefix, suffix)
        : (sqlDatatype.Type.DataType == DataType.Time || "time".Equals(sqlDatatype.Type.DbType, StringComparison.OrdinalIgnoreCase))
        ? value.SqlLiteralTime(0, prefix, suffix) : value.SqlLiteralTimestamp(value.Millisecond == 0 ? 0 : maxMilliSecondsPrecision, dateTimeSeparator);
      return $"{prefix}{value.ToString(format)}{suffix}";
    }

    //public static string xToSqlString_ISO(this DateTimeOffset value, SqlDataType sqlDatatype, string prefix, string dateTimeSeparator, int maxMilliSecondsPrecision, string suffix) {
    //  var format = (sqlDatatype.Type.DataType == DataType.Date || "date".Equals(sqlDatatype.Type.DbType, StringComparison.OrdinalIgnoreCase))
    //    ? value.SqlLiteralDate(prefix, suffix)
    //    : (sqlDatatype.Type.DataType == DataType.Time || "time".Equals(sqlDatatype.Type.DbType, StringComparison.OrdinalIgnoreCase))
    //    ? value.SqlLiteralTime(0, prefix, suffix) : value.SqlLiteralTimestamp(value.Millisecond == 0 ? 0 : maxMilliSecondsPrecision, dateTimeSeparator);
    //  return $"{prefix}{DateTimeConstants.IsoDateFormat}{value.ToString(format)}{DateTimeConstants.IsoTimeFormat}{suffix}";
    //}

    public static string ToSqlDataTypeString_DB2iSeries(this SqlDataType dataType, bool isMapGuidAsString = true) {
      switch (dataType.Type.DataType) {
        case DataType.Binary: return dataType.AppendLength("BINARY", 255);
        case DataType.Blob: return dataType.AppendLength("BLOB", int.MaxValue);
        case DataType.Boolean: return "SMALLINT";
        case DataType.Byte: return "SMALLINT";
        case DataType.Char: return "CHAR";
        case DataType.Date: return "DATE";
        case DataType.DateTime: return "TIMESTAMP";
        case DataType.DateTime2: return "TIMESTAMP";
        case DataType.DateTimeOffset: return "TIME";
        case DataType.Decimal: return "DECIMAL";
        case DataType.Double: return "DOUBLE";
        case DataType.Guid: return isMapGuidAsString ? "CHAR(32)" : "char(16) for bit data";
        case DataType.Int16: return "SMALLINT";
        case DataType.Int32: return "INTEGER";
        case DataType.Int64: return "BIGINT";
        case DataType.NVarChar: return dataType.AppendLength("VARCHAR", 32704);
        case DataType.Single: return "REAL";
        case DataType.Time: return "TIME";
        case DataType.Timestamp: return "TIMESTAMP";
        case DataType.UInt16: return "INTEGER";
        case DataType.UInt32: return "BIGINT";
        case DataType.UInt64: return "DECIMAL(28,0)";
        case DataType.VarBinary: return dataType.AppendLength("VARBINARY", 32704);
        case DataType.VarChar: return dataType.AppendLength("VARCHAR", 32704);
        case DataType.Variant: return dataType.AppendLength("BINARY", 255);
        default: throw new NotImplementedException($"{dataType.Type.DataType}");
      }
    }

    public static string ToSqlDbTypeString_DB2iSeries(string name, int? length, int? precision, int? scale) {
      if (name.Contains("(")) {
        return name;
      }
      var stringBuilder = new StringBuilder();
      stringBuilder.Append(name);
      if (length > 0) {
        stringBuilder.Append($"({length})");
      } else if (precision >= 0) {
        stringBuilder.Append($"({precision}");
        if (scale >= 0) {
          stringBuilder.Append($",{scale}");
        }
        stringBuilder.Append(")");
      }
      return stringBuilder.ToString();
    }

    public static string ToSqlString_DB2iSeries(this DbDataType dbDataType) => ToSqlDbTypeString_DB2iSeries(dbDataType.DbType, dbDataType.Length, dbDataType.Precision, dbDataType.Scale);



  }
}