using System;

namespace Common.Structures {
  public class IntCYYMM : IntCYY {
    public IntCYYMM() { }
    public IntCYYMM(DateTime d) : this(d.Year, d.Month) { }
    public IntCYYMM(int cyymm) {
      CYYMM = cyymm;
    }
    public IntCYYMM(int c, int yy, int mm) {
      C = c;
      YY = yy;
      MM = mm;
    }
    public IntCYYMM(int year, int month) {
      Year = year;
      Month = month;
    }

    #region Min & Max Values
    public const int _MinMM = 0;
    public const int _MaxMM = 99;

    public const int _MinCYYMM = 0;
    public const int _MaxCYYMM = 99999;

    public const int _MinMonth = 1;
    public const int _MaxMonth = 12;


    public static readonly IntCYYMM _MinIntCYYMM = new IntCYYMM(_MinCYYMM);
    public static readonly IntCYYMM _MaxIntCYYMM = new IntCYYMM(_MaxCYYMM);
    #endregion

    int mm;
    public int MM { get => mm; set => mm = value.GetValueBetween(_MinMM, _MaxMM); }

    public int Month { get => GetMonth(MM); set => SetMonth(value); }
    public virtual void SetMonth(int month) => MM = GetMonth(month);
    public int GetMonth(int month) => month.GetValueBetween(_MinMonth, _MaxMonth);

    public int CYYMM {
      get => CYY * 100 + MM;
      set {
        value = value.GetValueBetween(_MinCYYMM, _MaxCYYMM);
        CYY = value / 100;
        MM = value % 100;
      }
    }

    public int CYYMM00 => CYYMM * 100;
    public int CYYMM01 => CYYMM00 + 1;
    public int CYYMM99 => CYYMM00 + 99;

    #region IFormattable
    public override string ToString() => CYYMM.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    public DateTime GetDate(int day) => new DateTime(Year, Month, day);

    public static implicit operator IntCYYMM(decimal value) => new IntCYYMM((int)value);
    public static implicit operator IntCYYMM(int value) => new IntCYYMM(value);
    public static implicit operator IntCYYMM(DateTime value) => new IntCYYMM(value);

    //public static int ToCYYMM(this DateTime d) => ToCYYMM(d.Year, d.Month);
    //public static int? ToCYYMM(this DateTime? d, DateTime? defaultIfNull = null) => (d ?? defaultIfNull).ToCYYMM();

  }
}
