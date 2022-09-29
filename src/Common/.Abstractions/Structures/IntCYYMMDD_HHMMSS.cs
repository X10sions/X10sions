namespace Common.Structures {
  public class IntCYYMMDD_HHMMSS : IFormattable {
    public IntCYYMMDD_HHMMSS() : this(new IntCYYMMDD(), new IntHHMMSS()) { }
    public IntCYYMMDD_HHMMSS(IntCYYMMDD cyymmdd, IntHHMMSS hhmmss) {
      IntCYYMMDD = cyymmdd;
      IntHHMMSS = hhmmss;
    }
    public IntCYYMMDD_HHMMSS(DateTime d) : this(new IntCYYMMDD(d), new IntHHMMSS(d)) { }
    public IntCYYMMDD_HHMMSS(decimal cyymmdd_hhmmss) : this(new IntCYYMMDD(GetCYYMMDD(cyymmdd_hhmmss)), new IntHHMMSS(GetHHMMSS(cyymmdd_hhmmss))) { }
    public IntCYYMMDD_HHMMSS(int cyymmdd, int hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
    public IntCYYMMDD_HHMMSS(int year, int month, int day, int hour, int minute, int second, int millisecond) : this(new DateTime(year, month, day, hour, minute, second, millisecond)) { }
    public IntCYYMMDD_HHMMSS(string cyymmdd, string hhmmss) : this(new IntCYYMMDD(cyymmdd), new IntHHMMSS(hhmmss)) { }
    public IntCYYMMDD_HHMMSS(string c, string yymmdd, string hhmmss) : this(new IntCYYMMDD(c, yymmdd), new IntHHMMSS(hhmmss)) { }

    public static readonly IntCYYMMDD_HHMMSS MaxValue = new IntCYYMMDD_HHMMSS(9991231, 235959); // 9999-12-31 23:59:59
    public static readonly IntCYYMMDD_HHMMSS inValue = new IntCYYMMDD_HHMMSS(10101, 0); // 1901-01-01 00:00:00

    public DateTime Date {
      get => new DateTime(IntCYYMMDD.YYYY, IntCYYMMDD.MM, IntCYYMMDD.DD, IntHHMMSS.HH, IntHHMMSS.MM, IntHHMMSS.SS, IntHHMMSS.Millisecond);
      set {
        IntCYYMMDD = new IntCYYMMDD(value.Year, value.Month, value.Day);
        IntHHMMSS = new IntHHMMSS(value.Hour, value.Minute, value.Second);
      }
    }

    public decimal CYYMMDD_HHMMSS {
      get => new decimal(IntCYYMMDD.CYYMMDD + (double)IntHHMMSS.HHMMSS / 1000000);
      set {
        IntCYYMMDD = GetCYYMMDD(value);
        IntHHMMSS = GetHHMMSS(value);
      }
    }
    public IntCYYMMDD IntCYYMMDD { get; set; }
    public int CYYMMDD { get => IntCYYMMDD.CYYMMDD; set => IntCYYMMDD = value; }
    public int C { get => IntCYYMMDD.C; set => IntCYYMMDD.C = value; }
    public int YYMMDD { get => IntCYYMMDD.YYMMDD; set => IntCYYMMDD.YYMMDD = value; }
    public IntHHMMSS IntHHMMSS { get; set; }
    public int HHMMSS { get => IntHHMMSS.HHMMSS; set => IntHHMMSS = value; }
    public static implicit operator IntCYYMMDD_HHMMSS(decimal value) => new IntCYYMMDD_HHMMSS(value);
    public static implicit operator IntCYYMMDD_HHMMSS(int value) => new IntCYYMMDD_HHMMSS(value);
    public static implicit operator IntCYYMMDD_HHMMSS(DateTime value) => new IntCYYMMDD_HHMMSS(value);

    #region IFormattable
    public override string ToString() => CYYMMDD_HHMMSS.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

    public static int GetHHMMSS(decimal cyymmdd_hhmmss) => (int)(cyymmdd_hhmmss % 1 * 1000000);
    public static int GetCYYMMDD(decimal cyymmdd_hhmmss) => (int)cyymmdd_hhmmss;

  }
}