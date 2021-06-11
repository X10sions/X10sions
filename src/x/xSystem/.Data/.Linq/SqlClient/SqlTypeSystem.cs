using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Data.Linq.SqlClient {
  internal static class SqlTypeSystem {

    internal static TypeSystemProvider Create2000Provider() => new Sql2000Provider();

    internal static TypeSystemProvider Create2005Provider() => new Sql2005Provider();

    internal static TypeSystemProvider Create2008Provider() => new Sql2008Provider();

    internal static TypeSystemProvider CreateCEProvider() => new SqlCEProvider();

    // -1 is used to indicate a MAX size.  In ADO.NET, -1 is specified as the size
    // on a SqlParameter with SqlDbType = VarChar to indicate VarChar(MAX).
    internal const short LargeTypeSizeIndicator = -1;

    // class constructor will cause static initialization to be deferred
    [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Static constructors provide deferred execution, which is desired.")]
    static SqlTypeSystem() { }

    #region Factories
    private static ProviderType Create(SqlDbType type, int size) => new SqlType(type, size);

    private static ProviderType Create(SqlDbType type, int precision, int scale) {
      if (type != SqlDbType.Decimal && precision == 0 && scale == 0) {
        return Create(type);
      } else if (type == SqlDbType.Decimal && precision == defaultDecimalPrecision && scale == defaultDecimalScale) {
        return Create(type);
      }
      return new SqlType(type, precision, scale);
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
    private static ProviderType Create(SqlDbType type) {
      switch (type) {
        case SqlDbType.BigInt: return theBigInt;
        case SqlDbType.Bit: return theBit;
        case SqlDbType.Char: return theChar;
        case SqlDbType.DateTime: return theDateTime;
        case SqlDbType.Date: return theDate;
        case SqlDbType.Time: return theTime;
        case SqlDbType.DateTime2: return theDateTime2;
        case SqlDbType.DateTimeOffset: return theDateTimeOffset;
        case SqlDbType.Decimal: return theDefaultDecimal;
        case SqlDbType.Float: return theFloat;
        case SqlDbType.Int: return theInt;
        case SqlDbType.Money: return theMoney;
        case SqlDbType.Real: return theReal;
        case SqlDbType.UniqueIdentifier: return theUniqueIdentifier;
        case SqlDbType.SmallDateTime: return theSmallDateTime;
        case SqlDbType.SmallInt: return theSmallInt;
        case SqlDbType.SmallMoney: return theSmallMoney;
        case SqlDbType.Timestamp: return theTimestamp;
        case SqlDbType.TinyInt: return theTinyInt;
        case SqlDbType.Xml: return theXml;
        case SqlDbType.Text: return theText;
        case SqlDbType.NText: return theNText;
        case SqlDbType.Image: return theImage;
        default:
          return new SqlType(type);
      }
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
    private static Type GetClosestRuntimeType(SqlDbType sqlDbType) {
      switch (sqlDbType) {
        case SqlDbType.Int:
          return typeof(int);
        case SqlDbType.BigInt:
          return typeof(long);
        case SqlDbType.Bit:
          return typeof(bool);
        case SqlDbType.Date:
        case SqlDbType.SmallDateTime:
        case SqlDbType.DateTime:
        case SqlDbType.DateTime2:
          return typeof(DateTime);
        case SqlDbType.DateTimeOffset:
          return typeof(DateTimeOffset);
        case SqlDbType.Time:
          return typeof(TimeSpan);
        case SqlDbType.Float:
          return typeof(double);
        case SqlDbType.Real:
          return typeof(float);
        case SqlDbType.Binary:
        case SqlDbType.Image:
        case SqlDbType.Timestamp:
        case SqlDbType.VarBinary:
          return typeof(byte[]);
        case SqlDbType.Decimal:
        case SqlDbType.Money:
        case SqlDbType.SmallMoney:
          return typeof(decimal);
        case SqlDbType.Char:
        case SqlDbType.NChar:
        case SqlDbType.NText:
        case SqlDbType.NVarChar:
        case SqlDbType.Xml:
        case SqlDbType.VarChar:
        case SqlDbType.Text:
          return typeof(string);
        case SqlDbType.UniqueIdentifier:
          return typeof(Guid);
        case SqlDbType.SmallInt:
          return typeof(short);
        case SqlDbType.TinyInt:
          return typeof(byte);
        case SqlDbType.Udt:
          // Udt type is not handled.
          throw Error.UnexpectedTypeCode(SqlDbType.Udt);
        case SqlDbType.Variant:
        default:
          return typeof(object);
      }
    }
    #endregion

    #region Singleton SqlTypes
    /*
     * For simple types with no size, precision or scale, reuse a static set of SqlDataTypes.
     * This has two advantages: Fewer allocations of SqlType and faster comparison of
     * one type to another.
     */
    private static readonly SqlType theBigInt = new SqlType(SqlDbType.BigInt);
    private static readonly SqlType theBit = new SqlType(SqlDbType.Bit);
    private static readonly SqlType theChar = new SqlType(SqlDbType.Char);
    private static readonly SqlType theDateTime = new SqlType(SqlDbType.DateTime);
    private static readonly SqlType theDate = new SqlType(SqlDbType.Date);
    private static readonly SqlType theTime = new SqlType(SqlDbType.Time);
    private static readonly SqlType theDateTime2 = new SqlType(SqlDbType.DateTime2);
    private static readonly SqlType theDateTimeOffset = new SqlType(SqlDbType.DateTimeOffset);
    const int defaultDecimalPrecision = 29;
    const int defaultDecimalScale = 4;
    private static readonly SqlType theDefaultDecimal = new SqlType(SqlDbType.Decimal, defaultDecimalPrecision, defaultDecimalScale);
    private static readonly SqlType theFloat = new SqlType(SqlDbType.Float);
    private static readonly SqlType theInt = new SqlType(SqlDbType.Int);
    private static readonly SqlType theMoney = new SqlType(SqlDbType.Money, 19, 4);
    private static readonly SqlType theReal = new SqlType(SqlDbType.Real);
    private static readonly SqlType theUniqueIdentifier = new SqlType(SqlDbType.UniqueIdentifier);
    private static readonly SqlType theSmallDateTime = new SqlType(SqlDbType.SmallDateTime);
    private static readonly SqlType theSmallInt = new SqlType(SqlDbType.SmallInt);
    private static readonly SqlType theSmallMoney = new SqlType(SqlDbType.SmallMoney, 10, 4);
    private static readonly SqlType theTimestamp = new SqlType(SqlDbType.Timestamp);
    private static readonly SqlType theTinyInt = new SqlType(SqlDbType.TinyInt);
    private static readonly SqlType theXml = new SqlType(SqlDbType.Xml, LargeTypeSizeIndicator);
    private static readonly SqlType theText = new SqlType(SqlDbType.Text, LargeTypeSizeIndicator);
    private static readonly SqlType theNText = new SqlType(SqlDbType.NText, LargeTypeSizeIndicator);
    private static readonly SqlType theImage = new SqlType(SqlDbType.Image, LargeTypeSizeIndicator);
    #endregion

    public class SqlType : ProviderType {
      /// <summary>
      /// Helper method for building ToString functions.
      /// </summary>
      protected static string KeyValue<T>(string key, T value) {
        if (value != null) {
          return key + "=" + value.ToString() + " ";
        }
        return string.Empty;
      }

      /// <summary>
      /// Helper method for building ToString functions.
      /// </summary>
      protected static string SingleValue<T>(T value) {
        if (value != null) {
          return value.ToString() + " ";
        }
        return string.Empty;
      }
      public override string ToString() => SingleValue(GetClosestRuntimeType())
                + SingleValue(ToQueryString())
                + KeyValue("IsApplicationType", IsApplicationType)
                + KeyValue("IsUnicodeType", IsUnicodeType)
                + KeyValue("IsRuntimeOnlyType", IsRuntimeOnlyType)
                + KeyValue("SupportsComparison", SupportsComparison)
                + KeyValue("SupportsLength", SupportsLength)
                + KeyValue("IsLargeType", IsLargeType)
                + KeyValue("IsFixedSize", IsFixedSize)
                + KeyValue("IsOrderable", IsOrderable)
                + KeyValue("IsGroupable", IsGroupable)
                + KeyValue("IsNumeric", IsNumeric)
                + KeyValue("IsChar", IsChar)
                + KeyValue("IsString", IsString);
      protected SqlDbType sqlDbType;
      protected int? size;
      protected int precision;
      protected int scale;
      Type runtimeOnlyType;
      private int? applicationTypeIndex = null;

      #region Constructors
      internal SqlType(SqlDbType type) {
        sqlDbType = type;
      }

      internal SqlType(SqlDbType type, int? size) {
        sqlDbType = type;
        this.size = size;
      }

      internal SqlType(SqlDbType type, int precision, int scale) {
        System.Diagnostics.Debug.Assert(scale <= precision);
        sqlDbType = type;
        this.precision = precision;
        this.scale = scale;
      }

      internal SqlType(Type type) {
        runtimeOnlyType = type;
      }

      internal SqlType(int applicationTypeIndex) {
        this.applicationTypeIndex = applicationTypeIndex;
      }
      #endregion

      #region ProviderType Implementation
      internal override bool IsUnicodeType {
        get {
          switch (SqlDbType) {
            case SqlDbType.NChar:
            case SqlDbType.NText:
            case SqlDbType.NVarChar:
              return true;
            default:
              return false;
          }
        }
      }
      internal override ProviderType GetNonUnicodeEquivalent() {
        if (IsUnicodeType) {
          switch (SqlDbType) {
            case SqlDbType.NChar:
              return new SqlType(SqlDbType.Char, Size);
            case SqlDbType.NText:
              return new SqlType(SqlDbType.Text);
            case SqlDbType.NVarChar:
              return new SqlType(SqlDbType.VarChar, Size);
            default:
              // can't happen
              return this;
          }
        }
        return this;
      }
      internal override bool IsRuntimeOnlyType => runtimeOnlyType != null;

      internal override bool IsApplicationType => applicationTypeIndex != null;

      internal override bool IsApplicationTypeOf(int index) {
        if (IsApplicationType) {
          return applicationTypeIndex == index;
        }
        return false;
      }

      /// <summary>
      /// Determines if it is safe to suppress size specifications for
      /// the operand of a cast/convert.  For example, when casting to string,
      /// all these types have length less than the default sized used by SqlServer,
      /// so the length specification can be omitted without fear of truncation.
      /// </summary>
      internal override bool CanSuppressSizeForConversionToString {
        get {
          var defaultSize = 30;

          if (IsLargeType) {
            return false;
          } else if (!IsChar && !IsString && IsFixedSize && Size > 0 /*&& this.Size != LargeTypeSizeIndicator*/) { // commented out because LargeTypeSizeIndicator == -1
            return (Size < defaultSize);
          } else {

            switch (SqlDbType) {
              case SqlDbType.BigInt: // -2^63 (-9,223,372,036,854,775,808) to 2^63-1 (9,223,372,036,854,775,807)
              case SqlDbType.Bit: // 0 or 1
              case SqlDbType.Int: // -2^31 (-2,147,483,648) to 2^31-1 (2,147,483,647)
              case SqlDbType.SmallInt: // -2^15 (-32,768) to 2^15-1 (32,767)
              case SqlDbType.TinyInt: // 0 to 255
              case SqlDbType.Money: // -922,337,203,685,477.5808 to 922,337,203,685,477.5807
              case SqlDbType.SmallMoney: // -214,748.3648 to 214,748.3647
              case SqlDbType.Float: // -1.79E+308 to -2.23E-308, 0 and 2.23E-308 to 1.79E+308
              case SqlDbType.Real: // -3.40E+38 to -1.18E-38, 0 and 1.18E-38 to 3.40E+38
                return true;
              default:
                return false;
            };
          }

        }
      }

      internal override int ComparePrecedenceTo(ProviderType type) {
        var sqlProviderType = (SqlType)type;
        // Highest precedence is given to server-known types. Converting to a client-only type is
        // impossible when the conversion is present in a SQL query.
        var p0 = IsTypeKnownByProvider ? GetTypeCoercionPrecedence(SqlDbType) : int.MinValue;
        var p1 = sqlProviderType.IsTypeKnownByProvider ? GetTypeCoercionPrecedence(sqlProviderType.SqlDbType) : int.MinValue;
        return p0.CompareTo(p1);
      }

      /// <summary>
      /// Whether this is a legitimate server side type like NVARCHAR.
      /// </summary>
      private bool IsTypeKnownByProvider => !IsApplicationType && !IsRuntimeOnlyType;

      internal override bool IsSameTypeFamily(ProviderType type) {
        var sqlType = (SqlType)type;
        if (IsApplicationType) {
          return false;
        }
        if (sqlType.IsApplicationType) {
          return false;
        }
        return Category == sqlType.Category;
      }

      internal override bool SupportsComparison {
        get {
          switch (sqlDbType) {
            case SqlDbType.NText:
            case SqlDbType.Image:
            case SqlDbType.Xml:
            case SqlDbType.Text:
              return false;
            default:
              return true;
          }
        }
      }

      internal override bool SupportsLength {
        get {
          switch (sqlDbType) {
            case SqlDbType.NText:
            case SqlDbType.Image:
            case SqlDbType.Xml:
            case SqlDbType.Text:
              return false;
            default:
              return true;
          }
        }
      }

      /// <summary>
      /// Returns true if the given values will be equal to eachother on the server for this type.
      /// </summary>
      internal override bool AreValuesEqual(object o1, object o2) {
        if (o1 == null || o2 == null) {
          return false;
        }
        switch (sqlDbType) {
          case SqlDbType.Char:
          case SqlDbType.NChar:
          case SqlDbType.NVarChar:
          case SqlDbType.VarChar:
          case SqlDbType.Text:
            var s1 = o1 as string;
            if (s1 != null) {
              var s2 = o2 as string;
              if (s2 != null) {
                return s1.TrimEnd(' ').Equals(s2.TrimEnd(' '), StringComparison.Ordinal);
              }
            }
            break;
        }
        return o1.Equals(o2);
      }

      internal override bool IsLargeType {
        get {
          switch (sqlDbType) {
            case SqlDbType.NText:
            case SqlDbType.Image:
            case SqlDbType.Xml:
            case SqlDbType.Text:
              return true;
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
            case SqlDbType.VarBinary:
              return (size == LargeTypeSizeIndicator);
            default:
              return false;
          }
        }
      }

      internal override bool HasPrecisionAndScale {
        get {
          switch (SqlDbType) {
            case SqlDbType.Decimal:
            case SqlDbType.Float:
            case SqlDbType.Real:
            case SqlDbType.Money:      // precision and scale are fixed at 19,4
            case SqlDbType.SmallMoney: // precision and scale are fixed at 10,4
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
            case SqlDbType.Time:
              return true;
            default:
              return false;
          }
        }
      }

      internal override Type GetClosestRuntimeType() {
        if (runtimeOnlyType != null) {
          return runtimeOnlyType;
        } else {
          return SqlTypeSystem.GetClosestRuntimeType(sqlDbType);
        }
      }

      internal override string ToQueryString() => ToQueryString(QueryFormatOptions.None);

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override string ToQueryString(QueryFormatOptions formatFlags) {
        if (runtimeOnlyType != null) {
          return runtimeOnlyType.ToString();
        }
        var sb = new StringBuilder();

        switch (sqlDbType) {
          case SqlDbType.BigInt:
          case SqlDbType.Bit:
          case SqlDbType.Date:
          case SqlDbType.Time:
          case SqlDbType.DateTime:
          case SqlDbType.DateTime2:
          case SqlDbType.DateTimeOffset:
          case SqlDbType.Int:
          case SqlDbType.Money:
          case SqlDbType.SmallDateTime:
          case SqlDbType.SmallInt:
          case SqlDbType.SmallMoney:
          case SqlDbType.Timestamp:
          case SqlDbType.TinyInt:
          case SqlDbType.UniqueIdentifier:
          case SqlDbType.Xml:
          case SqlDbType.Image:
          case SqlDbType.NText:
          case SqlDbType.Text:
          case SqlDbType.Udt:
            sb.Append(sqlDbType.ToString());
            break;
          case SqlDbType.Variant:
            sb.Append("sql_variant");
            break;
          case SqlDbType.Binary:
          case SqlDbType.Char:
          case SqlDbType.NChar:
            sb.Append(sqlDbType);
            if ((formatFlags & QueryFormatOptions.SuppressSize) == 0) {
              sb.Append("(");
              sb.Append(size);
              sb.Append(")");
            }
            break;
          case SqlDbType.NVarChar:
          case SqlDbType.VarBinary:
          case SqlDbType.VarChar:
            sb.Append(sqlDbType);
            if ((size.HasValue && size != 0) && (formatFlags & QueryFormatOptions.SuppressSize) == 0) {
              sb.Append("(");
              if (size == LargeTypeSizeIndicator) {
                sb.Append("MAX");
              } else {
                sb.Append(size);
              }
              sb.Append(")");
            }
            break;
          case SqlDbType.Decimal:
          case SqlDbType.Float:
          case SqlDbType.Real:
            sb.Append(sqlDbType);
            if (precision != 0) {
              sb.Append("(");
              sb.Append(precision);
              if (scale != 0) {
                sb.Append(",");
                sb.Append(scale);
              }
              sb.Append(")");
            }
            break;
        }
        return sb.ToString();
      }

      internal override bool HasSizeOrIsLarge => size.HasValue || IsLargeType;

      internal override int? Size => size;

      internal int Precision => precision;

      internal int Scale => scale;

      internal override bool IsFixedSize {
        get {
          switch (sqlDbType) {
            case SqlDbType.NText:
            case SqlDbType.Text:
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
            case SqlDbType.Image:
            case SqlDbType.VarBinary:
            case SqlDbType.Xml:
              return false;
            default:
              return true;
          }
        }
      }

      internal SqlDbType SqlDbType => sqlDbType;

      internal override bool IsOrderable {
        get {
          if (IsRuntimeOnlyType) return false; // must be a SQL type

          switch (sqlDbType) {
            case SqlDbType.Image:
            case SqlDbType.Text:
            case SqlDbType.NText:
            case SqlDbType.Xml:
              return false;
            default:
              return true;
          }
        }
      }

      internal override bool IsGroupable {
        get {
          if (IsRuntimeOnlyType) return false; // must be a SQL type

          switch (sqlDbType) {
            case SqlDbType.Image:
            case SqlDbType.Text:
            case SqlDbType.NText:
            case SqlDbType.Xml:
              return false;
            default:
              return true;
          }
        }
      }

      internal override bool CanBeColumn => !IsApplicationType && !IsRuntimeOnlyType;

      internal override bool CanBeParameter => !IsApplicationType && !IsRuntimeOnlyType;

      internal override bool IsNumeric {
        get {
          if (IsApplicationType || IsRuntimeOnlyType) {
            return false;
          }
          switch (SqlDbType) {
            case SqlDbType.Bit:
            case SqlDbType.TinyInt:
            case SqlDbType.SmallInt:
            case SqlDbType.Int:
            case SqlDbType.BigInt:
            case SqlDbType.Decimal:
            case SqlDbType.Float:
            case SqlDbType.Real:
            case SqlDbType.Money:
            case SqlDbType.SmallMoney:
              return true;
            default:
              return false;
          };
        }
      }

      internal override bool IsChar {
        get {
          if (IsApplicationType || IsRuntimeOnlyType)
            return false;
          switch (SqlDbType) {
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
              return Size == 1;
            default:
              return false;
          }
        }
      }

      internal override bool IsString {
        get {
          if (IsApplicationType || IsRuntimeOnlyType)
            return false;
          switch (SqlDbType) {
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
              // -1 is used for large types to represent MAX
              return Size == 0 || Size > 1 || Size == LargeTypeSizeIndicator;
            case SqlDbType.Text:
            case SqlDbType.NText:
              return true;
            default:
              return false;
          }
        }
      }
      #endregion

      #region Equals + GetHashCode

      public override bool Equals(object obj) {
        if ((object)this == obj)
          return true;

        var that = obj as SqlType;
        if (that == null)
          return false;

        return
            runtimeOnlyType == that.runtimeOnlyType &&
            applicationTypeIndex == that.applicationTypeIndex &&
            sqlDbType == that.sqlDbType &&
            Size == that.Size &&
            precision == that.precision &&
            scale == that.scale;
      }

      public override int GetHashCode() {
        // Make up a hash function that will atleast not treat precision and scale
        // as interchangeable. This may need a smarter replacement if certain hash
        // buckets get too full.
        var hash = 0;
        if (runtimeOnlyType != null) {
          hash = runtimeOnlyType.GetHashCode();
        } else if (applicationTypeIndex != null) {
          hash = applicationTypeIndex.Value;
        }
        return hash ^ sqlDbType.GetHashCode() ^ (size ?? 0) ^ (precision) ^ (scale << 8);
      }
      #endregion

      #region Type Classification
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      private static int GetTypeCoercionPrecedence(SqlDbType type) {
        switch (type) {
          case SqlDbType.Binary: return 0;
          case SqlDbType.VarBinary: return 1;
          case SqlDbType.VarChar: return 2;
          case SqlDbType.Char: return 3;
          case SqlDbType.NChar: return 4;
          case SqlDbType.NVarChar: return 5;
          case SqlDbType.UniqueIdentifier: return 6;
          case SqlDbType.Timestamp: return 7;
          case SqlDbType.Image: return 8;
          case SqlDbType.Text: return 9;
          case SqlDbType.NText: return 10;
          case SqlDbType.Bit: return 11;
          case SqlDbType.TinyInt: return 12;
          case SqlDbType.SmallInt: return 13;
          case SqlDbType.Int: return 14;
          case SqlDbType.BigInt: return 15;
          case SqlDbType.SmallMoney: return 16;
          case SqlDbType.Money: return 17;
          case SqlDbType.Decimal: return 18;
          case SqlDbType.Real: return 19;
          case SqlDbType.Float: return 20;
          case SqlDbType.Date: return 21;
          case SqlDbType.Time: return 22;
          case SqlDbType.SmallDateTime: return 23;
          case SqlDbType.DateTime: return 24;
          case SqlDbType.DateTime2: return 25;
          case SqlDbType.DateTimeOffset: return 26;
          case SqlDbType.Xml: return 27;
          case SqlDbType.Variant: return 28;
          case SqlDbType.Udt: return 29;
          default:
            throw Error.UnexpectedTypeCode(type);
        }
      }

      internal enum TypeCategory {
        Numeric,
        Char,
        Text,
        Binary,
        Image,
        Xml,
        DateTime,
        UniqueIdentifier,
        Variant,
        Udt
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal TypeCategory Category {
        get {
          switch (SqlDbType) {
            case SqlDbType.Bit:
            case SqlDbType.TinyInt:
            case SqlDbType.SmallInt:
            case SqlDbType.Int:
            case SqlDbType.BigInt:
            case SqlDbType.Decimal:
            case SqlDbType.Float:
            case SqlDbType.Real:
            case SqlDbType.Money:
            case SqlDbType.SmallMoney:
              return TypeCategory.Numeric;
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.VarChar:
            case SqlDbType.NVarChar:
              return TypeCategory.Char;
            case SqlDbType.Text:
            case SqlDbType.NText:
              return TypeCategory.Text;
            case SqlDbType.Binary:
            case SqlDbType.VarBinary:
            case SqlDbType.Timestamp:
              return TypeCategory.Binary;
            case SqlDbType.Image:
              return TypeCategory.Image;
            case SqlDbType.Xml:
              return TypeCategory.Xml;
            case SqlDbType.Date:
            case SqlDbType.Time:
            case SqlDbType.DateTime:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
            case SqlDbType.SmallDateTime:
              return TypeCategory.DateTime;
            case SqlDbType.UniqueIdentifier:
              return TypeCategory.UniqueIdentifier;
            case SqlDbType.Variant:
              return TypeCategory.Variant;
            case SqlDbType.Udt:
              return TypeCategory.Udt;
            default:
              throw Error.UnexpectedTypeCode(this);
          }
        }
      }
      #endregion
    }

    abstract class ProviderBase : TypeSystemProvider {
      protected Dictionary<int, SqlType> applicationTypes = new Dictionary<int, SqlType>();

      #region Factories
      internal override ProviderType GetApplicationType(int index) {
        if (index < 0)
          throw Error.ArgumentOutOfRange("index");
        SqlType result = null;
        if (!applicationTypes.TryGetValue(index, out result)) {
          result = new SqlType(index);
          applicationTypes.Add(index, result);
        }
        return result;
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType Parse(string stype) {
        // parse type...
        string typeName = null;
        string param1 = null;
        string param2 = null;
        var paren = stype.IndexOf('(');
        var space = stype.IndexOf(' ');
        var end = (paren != -1 && space != -1) ? Math.Min(space, paren)
                : (paren != -1) ? paren
                : (space != -1) ? space
                : -1;
        if (end == -1) {
          typeName = stype;
          end = stype.Length;
        } else {
          typeName = stype.Substring(0, end);
        }
        var start = end;
        if (start < stype.Length && stype[start] == '(') {
          start++;
          end = stype.IndexOf(',', start);
          if (end > 0) {
            param1 = stype.Substring(start, end - start);
            start = end + 1;
            end = stype.IndexOf(')', start);
            param2 = stype.Substring(start, end - start);
          } else {
            end = stype.IndexOf(')', start);
            param1 = stype.Substring(start, end - start);
          }
          start = end++;
        }

        #region Special case type mappings
        if (string.Compare(typeName, "rowversion", StringComparison.OrdinalIgnoreCase) == 0) {
          typeName = "Timestamp";
        }

        if (string.Compare(typeName, "numeric", StringComparison.OrdinalIgnoreCase) == 0) {
          typeName = "Decimal";
        }

        if (string.Compare(typeName, "sql_variant", StringComparison.OrdinalIgnoreCase) == 0) {
          typeName = "Variant";
        }

        if (string.Compare(typeName, "filestream", StringComparison.OrdinalIgnoreCase) == 0) {
          typeName = "Binary";
        }
        #endregion

        // since we're going to parse the enum value below, we verify
        // here that it is defined.  For example, types like table, cursor
        // are not valid.
        if (!Enum.GetNames(typeof(SqlDbType)).Select(n => n.ToUpperInvariant()).Contains(typeName.ToUpperInvariant())) {
          throw Error.InvalidProviderType(typeName);
        }

        var p1 = 0;
        var p2 = 0;
        var dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), typeName, true);
        if (param1 != null) {
          if (string.Compare(param1.Trim(), "max", StringComparison.OrdinalIgnoreCase) == 0) {
            p1 = LargeTypeSizeIndicator;
          } else {
            p1 = int.Parse(param1, Globalization.CultureInfo.InvariantCulture);
            if (p1 == int.MaxValue)
              p1 = LargeTypeSizeIndicator;
          }
        }

        if (param2 != null) {
          if (string.Compare(param2.Trim(), "max", StringComparison.OrdinalIgnoreCase) == 0) {
            p2 = LargeTypeSizeIndicator;
          } else {
            p2 = int.Parse(param2, Globalization.CultureInfo.InvariantCulture);
            if (p2 == int.MaxValue)
              p2 = LargeTypeSizeIndicator;

          }
        }

        switch (dbType) {
          case SqlDbType.Binary:
          case SqlDbType.Char:
          case SqlDbType.NChar:
          case SqlDbType.NVarChar:
          case SqlDbType.VarBinary:
          case SqlDbType.VarChar:
            return Create(dbType, p1);
          case SqlDbType.Decimal:
          case SqlDbType.Real:
          case SqlDbType.Float:
            return Create(dbType, p1, p2);
          case SqlDbType.Timestamp:
          default:
            return Create(dbType);
        }
      }
      #endregion

      #region Implementation
      // Returns true if the type provider supports MAX types (e.g NVarChar(MAX))
      protected abstract bool SupportsMaxSize { get; }

      /// <summary>
      /// For types with size, determine the closest matching type for the information
      /// specified, promoting to the appropriate large type as needed.  If no size is
      /// specified, we use the max.
      /// </summary>
      protected ProviderType GetBestType(SqlDbType targetType, int? size) {
        // determine max size for the specified db type
        var maxSize = 0;
        switch (targetType) {
          case SqlDbType.NChar:
          case SqlDbType.NVarChar:
            maxSize = 4000;
            break;
          case SqlDbType.Char:
          case SqlDbType.VarChar:
          case SqlDbType.Binary:
          case SqlDbType.VarBinary:
            maxSize = 8000;
            break;
        };

        if (size.HasValue) {
          if (size.Value <= maxSize) {
            return Create(targetType, size.Value);
          } else {
            return GetBestLargeType(Create(targetType));
          }
        }

        // if the type provider supports MAX types, return one, otherwise use
        // the maximum size determined above
        return Create(targetType, SupportsMaxSize ? SqlTypeSystem.LargeTypeSizeIndicator : maxSize);
      }

      internal override void InitializeParameter(ProviderType type, DbParameter parameter, object value) {
        var sqlType = (SqlType)type;
        if (sqlType.IsRuntimeOnlyType) {
          throw Error.BadParameterType(sqlType.GetClosestRuntimeType());
        }
        var sParameter = parameter as System.Data.SqlClient.SqlParameter;
        if (sParameter != null) {
          sParameter.SqlDbType = sqlType.SqlDbType;
          if (sqlType.HasPrecisionAndScale) {
            sParameter.Precision = (byte)sqlType.Precision;
            sParameter.Scale = (byte)sqlType.Scale;
          }
        } else {
          var piSqlDbType = parameter.GetType().GetProperty("SqlDbType");
          if (piSqlDbType != null) {
            piSqlDbType.SetValue(parameter, sqlType.SqlDbType, null);
          }
          if (sqlType.HasPrecisionAndScale) {
            var piPrecision = parameter.GetType().GetProperty("Precision");
            if (piPrecision != null) {
              piPrecision.SetValue(parameter, Convert.ChangeType(sqlType.Precision, piPrecision.PropertyType, CultureInfo.InvariantCulture), null);
            }
            var piScale = parameter.GetType().GetProperty("Scale");
            if (piScale != null) {
              piScale.SetValue(parameter, Convert.ChangeType(sqlType.Scale, piScale.PropertyType, CultureInfo.InvariantCulture), null);
            }
          }
        }
        parameter.Value = GetParameterValue(sqlType, value);

        var determinedSize = DetermineParameterSize(sqlType, parameter);
        if (determinedSize.HasValue) {
          parameter.Size = determinedSize.Value;
        }
      }

      internal virtual int? DetermineParameterSize(SqlType declaredType, DbParameter parameter) {
        // Output parameters and input-parameters of a fixed-size should be specifically set if value fits.
        var isInputParameter = parameter.Direction == ParameterDirection.Input;
        if (!isInputParameter || declaredType.IsFixedSize) {
          if (declaredType.Size.HasValue && parameter.Size <= declaredType.Size || declaredType.IsLargeType) {
            return declaredType.Size.Value;
          }
        }

        // Preserve existing provider & server-driven behaviour for all other cases.
        return null;
      }

      protected int? GetLargestDeclarableSize(SqlType declaredType) {
        switch (declaredType.SqlDbType) {
          case SqlDbType.Image:
          case SqlDbType.Binary:
          case SqlDbType.VarChar:
            return 8000;
          case SqlDbType.NVarChar:
            return 4000;
          default:
            return null;
        }
      }

      [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
      protected object GetParameterValue(SqlType type, object value) {
        if (value == null) {
          return DBNull.Value;
        } else {
          var vType = value.GetType();
          var pType = type.GetClosestRuntimeType();
          if (pType == vType) {
            return value;
          } else {
            return DBConvert.ChangeType(value, pType);
          }
        }
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType PredictTypeForUnary(SqlNodeType unaryOp, ProviderType operandType) {
        switch (unaryOp) {
          case SqlNodeType.Not:
          case SqlNodeType.Not2V:
          case SqlNodeType.IsNull:
          case SqlNodeType.IsNotNull:
            return theBit;
          case SqlNodeType.Negate:
          case SqlNodeType.BitNot:
          case SqlNodeType.ValueOf:
          case SqlNodeType.Treat:
          case SqlNodeType.OuterJoinedValue:
            return operandType;
          case SqlNodeType.Count:
            return From(typeof(int));
          case SqlNodeType.LongCount:
            return From(typeof(long));
          case SqlNodeType.Min:
          case SqlNodeType.Max:
            return operandType;
          case SqlNodeType.Sum:
          case SqlNodeType.Avg:
          case SqlNodeType.Stddev:
            return MostPreciseTypeInFamily(operandType);
          case SqlNodeType.ClrLength:
            if (operandType.IsLargeType) {
              return From(typeof(long)); // SqlDbType.BigInt
            } else {
              return From(typeof(int)); // SqlDbType.Int
            }
          default:
            throw Error.UnexpectedNode(unaryOp);
        }
      }

      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType PredictTypeForBinary(SqlNodeType binaryOp, ProviderType leftType, ProviderType rightType) {
        SqlType highest;

        if (leftType.IsSameTypeFamily(From(typeof(string))) && rightType.IsSameTypeFamily(From(typeof(string)))) {
          highest = (SqlType)GetBestType(leftType, rightType);
        } else {
          var coercionPrecedence = leftType.ComparePrecedenceTo(rightType);
          highest = (SqlType)(coercionPrecedence > 0 ? leftType : rightType);
        }

        switch (binaryOp) {
          case SqlNodeType.Add:
          case SqlNodeType.Sub:
          case SqlNodeType.Mul:
          case SqlNodeType.Div:
          case SqlNodeType.BitAnd:
          case SqlNodeType.BitOr:
          case SqlNodeType.BitXor:
          case SqlNodeType.Mod:
          case SqlNodeType.Coalesce:
            return highest;
          case SqlNodeType.Concat:
            // When concatenating two types with size, the result type after
            // concatenation must have a size equal to the sum of the two sizes.
            if (highest.HasSizeOrIsLarge) {
              // get the best type, specifying null for size so we get
              // the maximum allowable size
              var concatType = GetBestType(highest.SqlDbType, null);

              if ((!leftType.IsLargeType && leftType.Size.HasValue) &&
                  (!rightType.IsLargeType && rightType.Size.HasValue)) {
                // If both types are not large types and have size, and the
                // size is less than the default size, return the shortened type.
                var concatSize = leftType.Size.Value + rightType.Size.Value;
                if ((concatSize < concatType.Size) || concatType.IsLargeType) {
                  return GetBestType(highest.SqlDbType, concatSize);
                }
              }

              return concatType;
            }
            return highest;
          case SqlNodeType.And:
          case SqlNodeType.Or:
          case SqlNodeType.LT:
          case SqlNodeType.LE:
          case SqlNodeType.GT:
          case SqlNodeType.GE:
          case SqlNodeType.EQ:
          case SqlNodeType.NE:
          case SqlNodeType.EQ2V:
          case SqlNodeType.NE2V:
            return theInt;
          default:
            throw Error.UnexpectedNode(binaryOp);
        }
      }

      internal override ProviderType MostPreciseTypeInFamily(ProviderType type) {
        var sqlType = (SqlType)type;
        switch (sqlType.SqlDbType) {
          case SqlDbType.TinyInt:
          case SqlDbType.SmallInt:
          case SqlDbType.Int:
            return From(typeof(int));
          case SqlDbType.SmallMoney:
          case SqlDbType.Money:
            return Create(SqlDbType.Money);
          case SqlDbType.Real:
          case SqlDbType.Float:
            return From(typeof(double));
          case SqlDbType.Date:
          case SqlDbType.Time:
          case SqlDbType.SmallDateTime:
          case SqlDbType.DateTime:
          case SqlDbType.DateTime2:
            return From(typeof(DateTime));
          case SqlDbType.DateTimeOffset:
            return From(typeof(DateTimeOffset));
          default:
            return type;
        }
      }

      [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unknown reason.")]
      private ProviderType[] GetArgumentTypes(SqlFunctionCall fc) {
        var array = new ProviderType[fc.Arguments.Count];
        for (int i = 0, n = array.Length; i < n; i++) {
          array[i] = fc.Arguments[i].SqlType;
        }
        return array;
      }

      /// <summary>
      /// Gives the return type of a SQL function which has as first argument something of this SqlType.
      /// For some functions (e.g. trigonometric functions) the return type is independent of the argument type;
      /// in those cases this method returns null and the type was already set correctly in the MethodCallConverter.
      /// </summary>
      /// <param name="functionCall">The SqlFunctionCall node.</param>
      /// <returns>null: Don't change type, otherwise: Set return type to this ProviderType</returns>
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType ReturnTypeOfFunction(SqlFunctionCall functionCall) {
        var argumentTypes = GetArgumentTypes(functionCall);

        var arg0 = (SqlType)argumentTypes[0];
        var arg1 = argumentTypes.Length > 1 ? (SqlType)argumentTypes[1] : null;

        switch (functionCall.Name) {
          case "LEN":
          case "DATALENGTH":
            switch (arg0.SqlDbType) {
              case SqlDbType.NVarChar:
              case SqlDbType.VarChar:
              case SqlDbType.VarBinary:
                if (arg0.IsLargeType) {
                  return Create(SqlDbType.BigInt);
                } else {
                  return Create(SqlDbType.Int);
                }
              default:
                return Create(SqlDbType.Int);
            }
          case "ABS":
          case "SIGN":
          case "ROUND":
          case "CEILING":
          case "FLOOR":
          case "POWER":
            switch (arg0.SqlDbType) {
              case SqlDbType.TinyInt:
              case SqlDbType.Int:
              case SqlDbType.SmallInt:
                return Create(SqlDbType.Int);
              case SqlDbType.Float:
              case SqlDbType.Real:
                return Create(SqlDbType.Float);
              default:
                return arg0;
            }
          case "PATINDEX":
          case "CHARINDEX":
            if (arg1.IsLargeType)
              return Create(SqlDbType.BigInt);
            return Create(SqlDbType.Int);
          case "SUBSTRING":
            if (functionCall.Arguments[2].NodeType == SqlNodeType.Value) {
              var val = (SqlValue)functionCall.Arguments[2];

              if (val.Value is int) {
                switch (arg0.SqlDbType) {
                  case SqlDbType.NVarChar:
                  case SqlDbType.NChar:
                  case SqlDbType.VarChar:
                  case SqlDbType.Char:
                    return Create(arg0.SqlDbType, (int)val.Value);
                  default:
                    return null;
                }
              }
            }
            switch (arg0.SqlDbType) {
              case SqlDbType.NVarChar:
              case SqlDbType.NChar:
                return Create(SqlDbType.NVarChar);
              case SqlDbType.VarChar:
              case SqlDbType.Char:
                return Create(SqlDbType.VarChar);
              default:
                return null;
            }
          case "STUFF":
            // if the stuff call is an insertion  and is strictly additive
            // (no deletion of characters) the return type is the same as 
            // a concatenation
            if (functionCall.Arguments.Count == 4) {
              var delLength = functionCall.Arguments[2] as SqlValue;
              if (delLength != null && (int)delLength.Value == 0) {
                return PredictTypeForBinary(SqlNodeType.Concat,
                    functionCall.Arguments[0].SqlType, functionCall.Arguments[3].SqlType);
              }
            }
            return null;
          case "LOWER":
          case "UPPER":
          case "RTRIM":
          case "LTRIM":
          case "INSERT":
          case "REPLACE":
          case "LEFT":
          case "RIGHT":
          case "REVERSE":
            return arg0;
          default:
            return null;
        }
      }

      internal override ProviderType ChangeTypeFamilyTo(ProviderType type, ProviderType toType) {
        // if this is already the same family, do nothing
        if (type.IsSameTypeFamily(toType))
          return type;

        // otherwise as a default return toType
        // but look for special cases we have to convert carefully
        if (type.IsApplicationType || toType.IsApplicationType)
          return toType;
        var sqlToType = (SqlType)toType;
        var sqlThisType = (SqlType)type;

        if (sqlToType.Category == SqlType.TypeCategory.Numeric && sqlThisType.Category == SqlType.TypeCategory.Char) {
          switch (sqlThisType.SqlDbType) {
            case SqlDbType.NChar:
              return Create(SqlDbType.Int);
            case SqlDbType.Char:
              return Create(SqlDbType.SmallInt);
            default:
              return toType;
          }
        } else {
          return toType;
        }
      }

      [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Microsoft: Cast is dependent on node type and casts do not happen unecessarily in a single code path.")]
      internal override ProviderType GetBestType(ProviderType typeA, ProviderType typeB) {
        // first determine the type precedence
        var bestType = (SqlType)(typeA.ComparePrecedenceTo(typeB) > 0 ? typeA : typeB);

        // if one of the types is a not a server type, return
        // that type
        if (typeA.IsApplicationType || typeA.IsRuntimeOnlyType) {
          return typeA;
        }
        if (typeB.IsApplicationType || typeB.IsRuntimeOnlyType) {
          return typeB;
        }

        var sqlTypeA = (SqlType)typeA;
        var sqlTypeB = (SqlType)typeB;
        if (sqlTypeA.HasPrecisionAndScale && sqlTypeB.HasPrecisionAndScale && bestType.SqlDbType == SqlDbType.Decimal) {
          var p0 = sqlTypeA.Precision;
          var s0 = sqlTypeA.Scale;
          var p1 = sqlTypeB.Precision;
          var s1 = sqlTypeB.Scale;
          // precision and scale may be zero if this is an unsized type.
          if (p0 == 0 && s0 == 0 && p1 == 0 && s1 == 0) {
            return Create(bestType.SqlDbType);
          } else if (p0 == 0 && s0 == 0) {
            return Create(bestType.SqlDbType, p1, s1);
          } else if (p1 == 0 && s1 == 0) {
            return Create(bestType.SqlDbType, p0, s0);
          }
          // determine best scale/precision
          var bestLeft = Math.Max(p0 - s0, p1 - s1);
          var bestRight = Math.Max(s0, s1);
          var precision = Math.Min(bestLeft + bestRight, 38);
          return Create(bestType.SqlDbType, precision, /*scale*/bestRight);
        } else {
          // determine the best size
          int? bestSize = null;

          if (sqlTypeA.Size.HasValue && sqlTypeB.Size.HasValue) {
            bestSize = (sqlTypeB.Size > sqlTypeA.Size) ? sqlTypeB.Size : sqlTypeA.Size;
          }

          if (sqlTypeB.Size.HasValue && sqlTypeB.Size.Value == LargeTypeSizeIndicator
              || sqlTypeA.Size.HasValue && sqlTypeA.Size.Value == LargeTypeSizeIndicator) {
            // the large type size trumps all
            bestSize = LargeTypeSizeIndicator;
          }

          bestType = new SqlType(bestType.SqlDbType, bestSize);
        }

        return bestType;
      }

      internal override ProviderType From(object o) {
        var clrType = (o != null) ? o.GetType() : typeof(object);
        if (clrType == typeof(string)) {
          var str = (string)o;
          return From(clrType, str.Length);
        } else if (clrType == typeof(bool)) {
          return From(typeof(int));
        } else if (clrType.IsArray) {
          var arr = (Array)o;
          return From(clrType, arr.Length);
        } else if (clrType == typeof(decimal)) {
          var d = (decimal)o;
          // The CLR stores the scale of a decimal value in bits
          // 16 to 23 (i.e., mask 0x00FF0000) of the fourth int. 
          var scale = (decimal.GetBits(d)[3] & 0x00FF0000) >> 16;
          return From(clrType, scale);
        } else {
          return From(clrType);
        }
      }

      internal override ProviderType From(Type type) => From(type, null);

      internal override ProviderType From(Type type, int? size) => From(type, size);

      #endregion
    }

    class Sql2005Provider : ProviderBase {
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType From(Type type, int? size) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
          type = type.GetGenericArguments()[0];
        var tc = System.Type.GetTypeCode(type);
        switch (tc) {
          case TypeCode.Boolean:
            return Create(SqlDbType.Bit);
          case TypeCode.Byte:
            return Create(SqlDbType.TinyInt);
          case TypeCode.SByte:
          case TypeCode.Int16:
            return Create(SqlDbType.SmallInt);
          case TypeCode.Int32:
          case TypeCode.UInt16:
            return Create(SqlDbType.Int);
          case TypeCode.Int64:
          case TypeCode.UInt32:
            return Create(SqlDbType.BigInt);
          case TypeCode.UInt64:
            return Create(SqlDbType.Decimal, 20, 0);
          case TypeCode.Decimal:
            return Create(SqlDbType.Decimal, 29, size ?? 4);
          case TypeCode.Double:
            return Create(SqlDbType.Float);
          case TypeCode.Single:
            return Create(SqlDbType.Real);
          case TypeCode.Char:
            return Create(SqlDbType.NChar, 1);
          case TypeCode.String:
            return GetBestType(SqlDbType.NVarChar, size);
          case TypeCode.DateTime:
            return Create(SqlDbType.DateTime);
          case TypeCode.Object: {
              if (type == typeof(Guid))
                return Create(SqlDbType.UniqueIdentifier);
              if (type == typeof(byte[]) || type == typeof(Binary))
                return GetBestType(SqlDbType.VarBinary, size);
              if (type == typeof(char[]))
                return GetBestType(SqlDbType.NVarChar, size);
              if (type == typeof(TimeSpan))
                return Create(SqlDbType.BigInt);
              if (type == typeof(System.Xml.Linq.XDocument) ||
                  type == typeof(System.Xml.Linq.XElement))
                return theXml;
              // else UDT?
              return new SqlType(type);
            }
          default:
            throw Error.UnexpectedTypeCode(tc);
        }
      }

      internal override ProviderType GetBestLargeType(ProviderType type) {
        var sqlType = (SqlType)type;
        switch (sqlType.SqlDbType) {
          case SqlDbType.NText:
          case SqlDbType.NChar:
          case SqlDbType.NVarChar:
            return Create(SqlDbType.NVarChar, LargeTypeSizeIndicator);
          case SqlDbType.Text:
          case SqlDbType.Char:
          case SqlDbType.VarChar:
            return Create(SqlDbType.VarChar, LargeTypeSizeIndicator);
          case SqlDbType.Image:
          case SqlDbType.Binary:
          case SqlDbType.VarBinary:
            return Create(SqlDbType.VarBinary, LargeTypeSizeIndicator);
        }
        return type;
      }

      internal override int? DetermineParameterSize(SqlType declaredType, DbParameter parameter) {
        var parameterSize = base.DetermineParameterSize(declaredType, parameter);
        if (parameterSize.HasValue) {
          return parameterSize;
        }

        // Engine team have recommended we use 4k/8k block sizes rather than -1 providing the
        // data fits within that size.
        var largestDeclarableSize = GetLargestDeclarableSize(declaredType);
        if (largestDeclarableSize.HasValue && largestDeclarableSize.Value >= parameter.Size) {
          return largestDeclarableSize.Value;
        }

        // Providers that support the maximum size large type indicator should fall to that.
        return SqlTypeSystem.LargeTypeSizeIndicator;
      }


      protected override bool SupportsMaxSize => true;

    }

    class Sql2008Provider : Sql2005Provider {
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType From(Type type, int? size) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
          type = type.GetGenericArguments()[0];

        // Retain mappings for DateTime and TimeSpan; add one for the new DateTimeOffset type.
        //
        if (System.Type.GetTypeCode(type) == TypeCode.Object &&
            type == typeof(DateTimeOffset)) {
          return Create(SqlDbType.DateTimeOffset);
        }

        return base.From(type, size);
      }
    }

    class Sql2000Provider : ProviderBase {
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These issues are related to our use of if-then and case statements for node types, which adds to the complexity count however when reviewed they are easy to navigate and understand.")]
      internal override ProviderType From(Type type, int? size) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
          type = type.GetGenericArguments()[0];
        var tc = System.Type.GetTypeCode(type);
        switch (tc) {
          case TypeCode.Boolean:
            return Create(SqlDbType.Bit);
          case TypeCode.Byte:
            return Create(SqlDbType.TinyInt);
          case TypeCode.SByte:
          case TypeCode.Int16:
            return Create(SqlDbType.SmallInt);
          case TypeCode.Int32:
          case TypeCode.UInt16:
            return Create(SqlDbType.Int);
          case TypeCode.Int64:
          case TypeCode.UInt32:
            return Create(SqlDbType.BigInt);
          case TypeCode.UInt64:
            return Create(SqlDbType.Decimal, 20, 0);
          case TypeCode.Decimal:
            return Create(SqlDbType.Decimal, 29, size ?? 4);
          case TypeCode.Double:
            return Create(SqlDbType.Float);
          case TypeCode.Single:
            return Create(SqlDbType.Real);
          case TypeCode.Char:
            return Create(SqlDbType.NChar, 1);
          case TypeCode.String:
            return GetBestType(SqlDbType.NVarChar, size);
          case TypeCode.DateTime:
            return Create(SqlDbType.DateTime);
          case TypeCode.Object: {
              if (type == typeof(Guid))
                return Create(SqlDbType.UniqueIdentifier);
              if (type == typeof(byte[]) || type == typeof(Binary))
                return GetBestType(SqlDbType.VarBinary, size);
              if (type == typeof(char[]))
                return GetBestType(SqlDbType.NVarChar, size);
              if (type == typeof(TimeSpan))
                return Create(SqlDbType.BigInt);
              if (type == typeof(System.Xml.Linq.XDocument) ||
                  type == typeof(System.Xml.Linq.XElement))
                return theNText;
              // else UDT?
              return new SqlType(type);
            }
          default:
            throw Error.UnexpectedTypeCode(tc);
        }
      }

      internal override ProviderType GetBestLargeType(ProviderType type) {
        var sqlType = (SqlType)type;
        switch (sqlType.SqlDbType) {
          case SqlDbType.NChar:
          case SqlDbType.NVarChar:
            return Create(SqlDbType.NText);
          case SqlDbType.Char:
          case SqlDbType.VarChar:
            return Create(SqlDbType.Text);
          case SqlDbType.Binary:
          case SqlDbType.VarBinary:
            return Create(SqlDbType.Image);
        }
        return type;
      }

      protected override bool SupportsMaxSize => false;

    }

    class SqlCEProvider : Sql2000Provider {
    }
  }
}