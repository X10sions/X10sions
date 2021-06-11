using IBM.Data.DB2.iSeries;
using System;
using LinqToDB;
using LinqToDB.Mapping;

namespace LinqToDB.DataProvider.DB2iSeries {
  public static class _Extensions {

    public static bool AddScalarTypes_iDB2(this MappingSchema mappingSchema) {
      mappingSchema.AddScalarType(typeof(iDB2BigInt), iDB2BigInt.Null, canBeNull: true, DataType.Int64);
      mappingSchema.AddScalarType(typeof(iDB2Char), iDB2Char.Null, canBeNull: true, DataType.Char);
      mappingSchema.AddScalarType(typeof(iDB2Date), iDB2Date.Null, canBeNull: true, DataType.Date);
      mappingSchema.AddScalarType(typeof(iDB2Decimal), iDB2Decimal.Null, canBeNull: true, DataType.Decimal);
      mappingSchema.AddScalarType(typeof(iDB2Double), iDB2Double.Null, canBeNull: true, DataType.Double);
      mappingSchema.AddScalarType(typeof(iDB2Integer), iDB2Integer.Null, canBeNull: true, DataType.Int32);
      mappingSchema.AddScalarType(typeof(iDB2SmallInt), iDB2SmallInt.Null, canBeNull: true, DataType.Int16);
      mappingSchema.AddScalarType(typeof(iDB2Time), iDB2Time.Null, canBeNull: true, DataType.Time);
      mappingSchema.AddScalarType(typeof(iDB2TimeStamp), iDB2TimeStamp.Null, canBeNull: true, DataType.DateTime2);
      mappingSchema.AddScalarType(typeof(iDB2VarChar), iDB2VarChar.Null, canBeNull: true, DataType.VarChar);
      return true;
    }

    [Obsolete("TODO: To be deleted")]
    public static iDB2DbType GetiDB2DbType(this DataType dataType) => dataType switch {
      DataType.Date => iDB2DbType.iDB2Date,
      DataType.DateTime => iDB2DbType.iDB2TimeStamp,
      DataType.DateTime2 => iDB2DbType.iDB2TimeStamp,
      DataType.Decimal => iDB2DbType.iDB2Decimal,
      DataType.Int16 => iDB2DbType.iDB2SmallInt,
      DataType.Int32 => iDB2DbType.iDB2Integer,
      DataType.Int64 => iDB2DbType.iDB2BigInt,
      DataType.NChar => iDB2DbType.iDB2Char,
      DataType.NVarChar => iDB2DbType.iDB2VarChar,
      _ => throw new NotImplementedException($"DataType {dataType} not mapped yet."),
    };

    [Obsolete("TODO: To be deleted")]
    public static string GetiDB2NativeFormat(this DataType dataType, object value)
      => value == null ? DBNull.Value.ToString() : dataType switch {
        DataType.Boolean => new iDB2Integer(Convert.ToBoolean(value) ? 1 : 0).ToString(),
        DataType.Date => new iDB2Date((string)value).Value.ToSqlDate(),
        DataType.DateTime => new iDB2TimeStamp((string)value).Value.ToSqlDate(),
        DataType.DateTime2 => new iDB2TimeStamp((string)value).Value.ToSqlTimestamp(6),
        DataType.Decimal => new iDB2Decimal((string)value),
        DataType.Double => new iDB2Double((string)value).ToString(),
        DataType.Int16 => new iDB2SmallInt((string)value).ToString(),
        DataType.Int32 => new iDB2Integer((string)value).ToString(),
        DataType.Int64 => new iDB2BigInt((string)value).ToString(),
        DataType.NChar => new iDB2Char((string)value),
        DataType.NVarChar => new iDB2VarChar((string)value),
        DataType.Time => new iDB2Time((string)value).Value.ToSqlTime(),
        DataType.Timestamp => new iDB2TimeStamp((string)value).Value.ToSqlTimestamp(6),
        DataType.VarChar => new iDB2VarChar((string)value),
        _ => throw new NotImplementedException($"DataType value {dataType} not mapped yet."),
      };

  }
}