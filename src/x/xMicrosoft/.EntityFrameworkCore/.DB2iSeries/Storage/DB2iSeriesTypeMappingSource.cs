using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Runtime.ConstrainedExecution;
using System;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesTypeMappingSource : RelationalTypeMappingSource {

  private static readonly RelationalTypeMapping _int = new IntTypeMapping("INTEGER");
  private static readonly RelationalTypeMapping _bigint = new LongTypeMapping("BIGINT");
  private static readonly RelationalTypeMapping _smallint = new ShortTypeMapping("SMALLINT");
  private static readonly RelationalTypeMapping _decimal = new DecimalTypeMapping("DECIMAL(18,2)");
  private static readonly RelationalTypeMapping _double = new DoubleTypeMapping("DOUBLE");
  private static readonly RelationalTypeMapping _real = new FloatTypeMapping("REAL");
  private static readonly RelationalTypeMapping _varchar = new StringTypeMapping("VARCHAR(255)", System.Data.DbType.String);
  //private static readonly RelationalTypeMapping VarChar = new StringTypeMapping("VARCHAR(256)", dbType: System.Data.DbType.String, unicode: true);
  private static readonly RelationalTypeMapping _char = new StringTypeMapping("CHAR(1)", System.Data.DbType.StringFixedLength);
  private static readonly RelationalTypeMapping _date = new DateTimeTypeMapping("DATE", System.Data.DbType.Date);
  private static readonly RelationalTypeMapping _timestamp = new DateTimeTypeMapping("TIMESTAMP", System.Data.DbType.DateTime);
  private static readonly RelationalTypeMapping _time = new TimeSpanTypeMapping("TIME", System.Data.DbType.Time);
  private static readonly RelationalTypeMapping _bool = new BoolTypeMapping("SMALLINT", System.Data.DbType.Boolean);

  //private static readonly RelationalTypeMapping BoolSmallInt = new RelationalTypeMapping("SMALLINT", typeof(bool), System.Data.DbType.Int16, converter: new BoolToZeroOneConverter<int>(false).ComposeWith(new CastingConverter<int, short>()));

  private static readonly RelationalTypeMapping Decimal = new DecimalTypeMapping("DECIMAL(18, 6)");
  private static readonly RelationalTypeMapping Double = new DoubleTypeMapping("DOUBLE");
  private static readonly RelationalTypeMapping Blob = new ByteArrayTypeMapping("BLOB");

  //private static readonly RelationalTypeMapping _date = new DateOnlyTypeMapping("DATE");
  //private static readonly RelationalTypeMapping _time = new TimeOnlyTypeMapping("TIME");


  private static readonly RelationalTypeMapping _guid = new GuidTypeMapping("CHAR(36)");
  private static readonly RelationalTypeMapping _stringVar = new StringTypeMapping("VARCHAR(256)", dbType: System.Data.DbType.String, unicode: true);
  private static readonly RelationalTypeMapping _stringChar = new StringTypeMapping("CHAR(1)", dbType: System.Data.DbType.String, unicode: true, size: 1);// fixedLength: true);
  //private static readonly RelationalTypeMapping _boolSmallint = new RelationalTypeMapping("SMALLINT", typeof(bool), System.Data.DbType.Int16, converter: new BoolToZeroOneConverter<int>(false).ComposeWith(new CastingConverter<int, short>()));


  public DB2iSeriesTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies) : base(dependencies, relationalDependencies) { }

  protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) {
    var clrType = mappingInfo.ClrType;
    if (clrType == typeof(int)) return _int;
    if (clrType == typeof(long)) return _bigint;
    if (clrType == typeof(short)) return _smallint;
    if (clrType == typeof(Guid)) return _guid;
    if (clrType == typeof(decimal)) return _decimal;
    if (clrType == typeof(double)) return _double;
    if (clrType == typeof(float)) return _real;
    if (clrType == typeof(bool)) return _bool;
    if (clrType == typeof(DateTime)) return _timestamp;
    if (clrType == typeof(TimeSpan)) return _time;
    if (clrType == typeof(string)) {
      var size = mappingInfo.Size ?? 255;
      return mappingInfo.IsFixedLength == true
          ? new StringTypeMapping($"CHAR({size})", System.Data.DbType.StringFixedLength, size: size)
          : new StringTypeMapping($"VARCHAR({size})", System.Data.DbType.String, size: size);
    }

    //if (clrType == typeof(string)) return _stringVar;
    //if (clrType == typeof(bool)) return _boolSmallint;
    //if (clrType == typeof(DateOnly)) return _date;
    //if (clrType == typeof(TimeOnly)) return _time;
    //if (clrType == typeof(DateTime)) return _timestamp;

    //// Fallbacks: byte arrays, decimals, etc.
    //if (clrType == typeof(byte[])) return new ByteArrayTypeMapping("BLOB");
    //if (clrType == typeof(decimal)) return new DecimalTypeMapping("DECIMAL(18, 6)", System.Data.DbType.Decimal);

    //if (clr == typeof(string)) return VarChar;
    //if (clr == typeof(bool)) return BoolSmallInt;
    //if (clr == typeof(decimal)) return info.WithPrecision(18).WithScale(6) is var mi ? new DecimalTypeMapping($"DECIMAL({mi.Precision ?? 18}, {mi.Scale ?? 6})") : Decimal;
    //if (clr == typeof(double) || clr == typeof(float)) return Double;
    //if (clr == typeof(byte[])) return Blob;
    //if (clr == typeof(DateTime)) return new DateTimeTypeMapping("TIMESTAMP");
    //if (clr == typeof(DateOnly)) return new DateOnlyTypeMapping("DATE");
    //if (clr == typeof(TimeOnly)) return new TimeOnlyTypeMapping("TIME");

    //// Enums to SMALLINT
    //if (clr != null && clr.IsEnum) return SmallInt;


    return base.FindMapping(mappingInfo);
  }



}
