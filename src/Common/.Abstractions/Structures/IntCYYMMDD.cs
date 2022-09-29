using System.Linq.Expressions;

namespace Common.Structures {
  public class IntCYYMMDD : IntCYYMM,
    IComparable,
    IComparable<IntCYYMMDD>,
    IComparable<DateTime>,
    IComparable<decimal>,
    IComparable<double>,
    IComparable<int>,
    IComparable<string>,
    IConvertible,
    IEquatable<IntCYYMMDD>,
    IEquatable<DateTime>,
    IEquatable<decimal>,
    IEquatable<double>,
    IEquatable<int>,
    IEquatable<string> {

    public IntCYYMMDD() : this(DateTime.Now) { }
    public IntCYYMMDD(int cyymmdd) : base(GetCYYMM(cyymmdd)) { DD = GetDD(cyymmdd); }
    public IntCYYMMDD(int yyyy, int mm, int dd) : base(new DateTime(yyyy, mm, dd)) { }
    public IntCYYMMDD(int c, int yy, int mm, int dd) : base(c, yy, mm) { DD = dd; }
    public IntCYYMMDD(DateTime d) : base(d) { DD = d.Day; }
    public IntCYYMMDD(string c, string yy, string mm, string dd) : this(c.As(0), yy.As(0), mm.As(0), dd.As(0)) { }
    public IntCYYMMDD(string cyymmdd) : this(cyymmdd.As(0)) { }
    public IntCYYMMDD(string c, string yymmdd) : this((c + yymmdd).As(0)) { }

    #region Min & Max Values
    public static readonly int MinDD = 0; // 9999-12-31
    public static readonly int MaxDD = 99; // 1901-01-01

    public static readonly int MinYYMMDD = 0; // 9999-12-31
    public static readonly int MaxYYMMDD = 999999; // 1901-01-01

    public static readonly int MinCYYMMDD = 0; // 9999-12-31
    public static readonly int MaxCYYMMDD = 9999999; // 1901-01-01

    public static new readonly IntCYYMMDD MinValue = new IntCYYMMDD(0); // 9999-12-31
    public static new readonly IntCYYMMDD MaxValue = new IntCYYMMDD(9999999); // 1901-01-01
    #endregion

    int dd;
    public int DD { get => dd; set => dd = value.GetValueBetween(MinDD, MaxDD); }

    public int CYYMMDD {
      get => CYYMM_DD_GetCYYMMDD(CYYMM, DD);
      set {
        value = value.GetValueBetween(MinCYYMMDD, MaxCYYMMDD);
        CYYMM = GetCYYMM(value);
        DD = GetDD(value);
      }
    }

    public int YYMMDD { get => GetYYMMDD(YY, MM, DD); set => CYYMMDD = C_YYMMDD_GetCYYMMDD(C, value.GetValueBetween(MinYYMMDD, MaxYYMMDD)); }

    public DateTime Date {
      get => new DateTime(YYYY, MM, DD).Date;
      set {
        YYYY = value.Year;
        MM = value.Month;
        DD = value.Day;
      }
    }

    public static int GetCYYMM(int cyymmdd) => cyymmdd / 100;
    public static int GetDD(int cyymmdd) => cyymmdd % 100;
    public static int CYYMM_DD_GetCYYMMDD(int cyymm, int dd) => cyymm * 100 + dd;
    public static int GetYYMMDD(int yy, int mm, int dd) => GetYYMM(yy, mm) * 100 + dd;
    public static int C_YYMMDD_GetCYYMMDD(int c, int yymmdd) => c * 1000000 + yymmdd;

    public static Expression<Func<DateTime, int>> CYYMMDD_Expression() => (DateTime d) => new IntCYYMMDD(d).CYYMMDD;

    public static implicit operator IntCYYMMDD(decimal value) => new IntCYYMMDD((int)value);
    public static implicit operator IntCYYMMDD(DateTime value) => new IntCYYMMDD(value);
    public static implicit operator IntCYYMMDD(double value) => new IntCYYMMDD((int)value);
    public static implicit operator IntCYYMMDD(int value) => new IntCYYMMDD(value);
    public static implicit operator IntCYYMMDD(string value) => new IntCYYMMDD(Convert.ToInt32(value));

    public static implicit operator DateTime(IntCYYMMDD value) => value.Date;
    public static implicit operator decimal(IntCYYMMDD value) => value.CYYMMDD;
    public static implicit operator double(IntCYYMMDD value) => value.CYYMMDD;
    public static implicit operator int(IntCYYMMDD value) => value.CYYMMDD;
    public static implicit operator string(IntCYYMMDD value) => value.ToString();

    #region IComparable
    public int CompareTo(object? obj) {
      //if(obj == null) {        return 1;      }
      switch (obj) {
        case null:
          return 1;
        case IntCYYMMDD icymd:
          return icymd.CompareTo(obj);
        case int i:
          return i.CompareTo(obj);
        case DateTime dt:
          return dt.CompareTo(obj);
        case decimal dec:
          return dec.CompareTo(obj);
        case double dbl:
          return dbl.CompareTo(obj);
        case string s:
          return s.CompareTo(obj);
      }
      throw new NotImplementedException();
    }
    public int CompareTo(IntCYYMMDD other) => other.CYYMMDD.CompareTo(CYYMMDD);
    public int CompareTo(DateTime other) => other.CompareTo(Date);
    public int CompareTo(decimal other) => other.CompareTo(CYYMMDD);
    public int CompareTo(double other) => other.CompareTo(CYYMMDD);
    public int CompareTo(int other) => other.CompareTo(CYYMMDD);
    public int CompareTo(string other) => string.Compare(other, ToString(), StringComparison.OrdinalIgnoreCase);
    #endregion

    #region IConvertible
    public bool ToBoolean(IFormatProvider provider) => throw new NotImplementedException();
    public byte ToByte(IFormatProvider provider) => Convert.ToByte(CYYMMDD, provider);
    public char ToChar(IFormatProvider provider) => throw new NotImplementedException();
    public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Date, provider);
    public decimal ToDecimal(IFormatProvider provider) => Convert.ToDecimal(CYYMMDD, provider);
    public double ToDouble(IFormatProvider provider) => Convert.ToDouble(CYYMMDD, provider);
    public float ToSingle(IFormatProvider provider) => Convert.ToSingle(CYYMMDD, provider);
    public int ToInt32(IFormatProvider provider) => Convert.ToInt32(CYYMMDD, provider);
    public long ToInt64(IFormatProvider provider) => Convert.ToInt64(CYYMMDD, provider);
    public object ToType(Type conversionType, IFormatProvider provider) {
      switch (conversionType) {
        case Type _ when conversionType == typeof(DateTime):
        case Type _ when conversionType == typeof(TimeSpan):
          return Convert.ChangeType(Date, conversionType, provider);
        default:
          return Convert.ChangeType(CYYMMDD, conversionType, provider);
      }
    }
    public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(CYYMMDD, provider);
    public short ToInt16(IFormatProvider provider) => Convert.ToInt16(CYYMMDD, provider);
    public string ToString(IFormatProvider provider) => Convert.ToString(ToString(), provider);
    public TypeCode GetTypeCode() => TypeCode.Int32;
    public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(CYYMMDD, provider);
    public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(CYYMMDD, provider);
    public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(CYYMMDD, provider);
    #endregion

    #region IEquatable
    public bool Equals(IntCYYMMDD other) => other.CYYMMDD.Equals(CYYMMDD);
    public bool Equals(DateTime other) => other.Equals(Date);
    public bool Equals(decimal other) => other.Equals(CYYMMDD);
    public bool Equals(double other) => other.Equals(CYYMMDD);
    public bool Equals(int other) => other.Equals(CYYMMDD);
    public override bool Equals(object obj) {
      switch (obj) {
        case IntCYYMMDD other:
          return other.CYYMMDD.Equals(CYYMMDD);
        case DateTime other:
          return other.Equals(Date);
        case decimal other:
          return other.Equals(CYYMMDD);
        case double other:
          return other.Equals(CYYMMDD);
        case int other:
          return other.Equals(CYYMMDD);
        case string other:
          return other.Equals(ToString());
      }
      throw new NotImplementedException();
    }
    public bool Equals(string other) => other.Equals(ToString());
    #endregion

    #region IFormattable
    public override string ToString() => CYYMMDD.ToString();
    public override string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion
  }

  public static class IntCYYMMDDExtensions {

    //public static int ToCYYMMDD(this DateTime d) => ((d.Year - 1900) * 10000) + (d.Month * 100) + d.Day;

    //[Computed] public static int? ToCYYMMDD(this DateTime? d, int? defaultIfNull = null) => d.HasValue ? d.Value.ToCYYMMDD() : defaultIfNull;
    //[Computed] public static int ToCYY(DateTime d) => ToCYY(d.Year);
    //[Computed] public static int ToCYY(int year) => (year - 1900);
    //[Computed] public static int ToCYYMM(DateTime d) => ToCYYMM(d.Year, d.Month);
    //[Computed] public static int ToCYYMM(int year, int month) => ((ToCYY(year) * 100) + month);
    //[Computed] public static int ToCYYMM00(DateTime d) => (ToCYYMM(d) * 100);
    //[Computed] public static int ToCYYMM01(DateTime d) => ((ToCYYMM(d) * 100) + 1);
    //[Computed] public static int ToCYYMM99(DateTime d) => ((ToCYYMM(d) * 100) + 99);
    //[Computed] public static int ToCYYMMDD(DateTime d) => ToCYYMMDD(d.Year, d.Month, d.Day);
    //[Computed] public static int ToCYYMMDD(int year, int month, int day) => (ToCYYMM(year, month) * 100) + day;

    //public static int IntCYYMMDD_GetCYYMM(this int cyymmdd) => (cyymmdd / 100).GetValueBetween(0, 99999);
    //public static int IntCYYMMDD_GetCYY(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYYMM() / 100;
    //public static int IntCYYMMDD_GetC(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYY() / 100;
    //public static int IntCYYMMDD_GetYY(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYY() % 100;
    //public static int IntCYYMMDD_GetMM(this int cyymmdd) => cyymmdd.IntCYYMMDD_GetCYYMM() % 100;
    //public static int IntCYYMMDD_GetDD(this int cyymmdd) => cyymmdd % 100.GetValueBetween(0, 99);
  }

}
