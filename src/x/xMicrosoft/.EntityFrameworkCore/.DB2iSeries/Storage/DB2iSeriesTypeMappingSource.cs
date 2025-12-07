using Microsoft.EntityFrameworkCore.Storage;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;

public class DB2iSeriesTypeMappingSource : RelationalTypeMappingSource {
  private readonly RelationalTypeMapping _int = new IntTypeMapping("INTEGER");
  private readonly RelationalTypeMapping _bigint = new LongTypeMapping("BIGINT");
  private readonly RelationalTypeMapping _smallint = new ShortTypeMapping("SMALLINT");
  private readonly RelationalTypeMapping _decimal = new DecimalTypeMapping("DECIMAL(18,2)");
  private readonly RelationalTypeMapping _double = new DoubleTypeMapping("DOUBLE");
  private readonly RelationalTypeMapping _real = new FloatTypeMapping("REAL");
  private readonly RelationalTypeMapping _varchar = new StringTypeMapping("VARCHAR(255)", System.Data.DbType.String);
  private readonly RelationalTypeMapping _char = new StringTypeMapping("CHAR(1)", System.Data.DbType.StringFixedLength);
  private readonly RelationalTypeMapping _date = new DateTimeTypeMapping("DATE", System.Data.DbType.Date);
  private readonly RelationalTypeMapping _timestamp = new DateTimeTypeMapping("TIMESTAMP", System.Data.DbType.DateTime);
  private readonly RelationalTypeMapping _time = new TimeSpanTypeMapping("TIME");
  private readonly RelationalTypeMapping _bool = new BoolTypeMapping("SMALLINT");

  public DB2iSeriesTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies) : base(dependencies, relationalDependencies) { }

  protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) {
    var clrType = mappingInfo.ClrType;
    if (clrType == typeof(int)) return _int;
    if (clrType == typeof(long)) return _bigint;
    if (clrType == typeof(short)) return _smallint;
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
    return base.FindMapping(mappingInfo);
  }
}