namespace System.Data {
  public static class DbTypeExtensions {

    public static string GetQualifier(this DbType dbType)
      => dbType.IsDate() ? SqlDateTimeOptions.DefaultLiteralPrefix
      : dbType.IsString() ? SqlStringOptions.DefaultLiteralPrefix
      : string.Empty;

    public static string GetSqlValue(this DbType dbType, object value) {
      if (value == null || Convert.IsDBNull(value)) {
        return SqlOptions.SqlNullString;
      }
      switch (dbType) {
        case DbType.Date: return ((DateTime)value).ToSqlDate();
        case DbType.DateTime: return ((DateTime)value).ToSqlTimestamp(6);
        case DbType.Time: return ((DateTime)value).TimeOfDay.ToSqlTime();
        default: return value.ToString();
      }
    }

    public static string GetSqlQualifiedValue(this DbType dbType, object value, bool doIncludeQualifier) {
      var sqlValue = dbType.GetSqlValue(value);
      if (!doIncludeQualifier) return sqlValue;
      var qualifier = GetQualifier(dbType);
      return qualifier + sqlValue + qualifier;
    }

    public static bool HasQualifier(this DbType dbType) => dbType.IsDate() || dbType.IsString();

    public static bool IsDate(this DbType dbType) {
      switch (dbType) {
        case DbType.Date:
        case DbType.DateTime:
        case DbType.DateTime2:
        case DbType.DateTimeOffset:
        case DbType.Time:
          return true;

      }
      return false;
    }

    public static bool IsNumeric(this DbType dbType) {
      switch (dbType) {
        case DbType.Boolean:
        case DbType.Byte:
        case DbType.Currency:
        case DbType.Decimal:
        case DbType.Double:
        case DbType.Int16:
        case DbType.Int32:
        case DbType.Int64:
        case DbType.SByte:
        case DbType.Single:
        case DbType.UInt16:
        case DbType.UInt32:
        case DbType.UInt64:
        case DbType.VarNumeric:
          return true;

      }
      return false;
    }

    public static bool IsString(this DbType dbType) {
      switch (dbType) {
        case DbType.AnsiString:
        case DbType.AnsiStringFixedLength:
        case DbType.String:
        case DbType.StringFixedLength:
          return true;

      }
      return false;
    }

    public static Type AsType(this DbType @this) {
      switch (@this) {
        case DbType.Boolean:
          return typeof(bool?);
        case DbType.Byte:
          return typeof(byte?);
        case DbType.Binary:
          return typeof(byte[]);
        case DbType.StringFixedLength:
          return typeof(char?);
        case DbType.DateTime:
          return typeof(DateTime?);
        case DbType.DateTimeOffset:
          return typeof(DateTimeOffset?);
        case DbType.Decimal:
          return typeof(decimal?);
        case DbType.Double:
          return typeof(double?);
        case DbType.Guid:
          return typeof(Guid?);
        case DbType.Int32:
          return typeof(int?);
        case DbType.Int64:
          return typeof(long?);
        case DbType.Object:
          return typeof(object);
        case DbType.SByte:
          return typeof(sbyte?);
        case DbType.Int16:
          return typeof(short?);
        case DbType.Single:
          return typeof(float?);
        case DbType.String:
          return typeof(string);
        case DbType.Time:
          return typeof(TimeSpan?);
        case DbType.UInt16:
          return typeof(ushort?);
        case DbType.UInt32:
          return typeof(uint?);
        case DbType.UInt64:
          return typeof(ulong?);
        default:
          throw new NotImplementedException($"Not Mapped yet: {@this.ToString()}");
      }
    }

    public static string GetSqlValue(this DbType @this, object value, bool doIncludeQualifier) {
      if (value == null || Convert.IsDBNull((value))) {
        return "Null";
      }
      switch (@this) {
        case DbType.Date:
          value = Convert.ToDateTime(value).ToSqlDate();
          break;
        case DbType.DateTime:
          value = Convert.ToDateTime(value).ToSqlTimestamp(6);
          break;
        case DbType.Time:
          value = Convert.ToDateTime(value).ToSqlTime();
          break;
      }
      var qualifier = GetQualifier(@this);
      return Convert.ToString(doIncludeQualifier ? $"{qualifier}{(value)}{qualifier}" : value);
    }


  }
}