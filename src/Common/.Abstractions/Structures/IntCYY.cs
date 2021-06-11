using System;

namespace Common.Structures {
  public class IntCYY : IFormattable {
    public IntCYY() { }
    public IntCYY(DateTime d) {
      Year = d.Year;
    }
    public IntCYY(int cyy) {
      CYY = cyy;
    }
    public IntCYY(int c, int yy) {
      C = c;
      YY = yy;
    }

    //public IntCYY(IntYear intYear) : this() {
    //  Year = intYear.Value;
    //}

    #region Min & Max Values
    public const int _MinC = 0;
    public const int _MaxC = 9;

    public const int _MinYY = 0;
    public const int _MaxYY = 99;

    public const int _MinCYY = 0;
    public const int _MaxCYY = 999;

    public const int _MinYear = 1900;
    public const int _MaxYear = 2899;

    public static readonly IntCYY _MinIntCYY = new IntCYYMM(_MinCYY);
    public static readonly IntCYY _MaxIntCYY = new IntCYYMM(_MaxCYY);
    #endregion

    int c;
    int yy;

    /// <summary>
    /// Centuries after 1900
    /// </summary>
    public int C { get => c; set => c = value.GetValueBetween(_MinC, _MaxC); }
    public int YY { get => yy; set => yy = value.GetValueBetween(_MinYY, _MaxYY); }

    public int CYY {
      get => (C * 100) + YY;
      set {
        value = value.GetValueBetween(_MinCYY, _MaxCYY);
        C = value / 100;
        YY = value % 100;
      }
    }

    public int Year { get => GetYear(CYY + 1900); set => SetYear(value); }
    public virtual void SetYear(int year) => CYY = GetYear(year) - 1900;
    public int GetYear(int year) => year.GetValueBetween(_MinYear, _MaxYear);


    public DateTime YearStartDateTime => new DateTime(Year, 1, 1);
    public DateTime YearEndDateTime => new DateTime(Year, 12, 31, 23, 59, 59, 999);

    public DateTime GetDate(int month, int day) => new DateTime(Year, month, day);

    //public int GetCYYMM(int month) => CYY * 100 + month;
    //public int GetCYYMMDD(int month, int day) => GetCYYMM(month) * 100 + day;

    #region IFormattable
    public override string ToString() => CYY.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

    public static implicit operator IntCYY(decimal value) => new IntCYY((int)value);
    public static implicit operator IntCYY(int value) => new IntCYY(value);
    public static implicit operator IntCYY(DateTime value) => new IntCYY(value);

  }

  //public static class IntCYYExtensions {

  //  public static int CYYToYear(this int c, int yy) => CYYToYear(c * 100 + yy);
  //  public static int CYYToYear(this int cyy) => cyy + 1900;
  //  public static (int C, int YY) GetCYYParts(this int cyy) => (cyy / 100, cyy % 100);
  //  public static int YeartoCYY(this int yyyy) => yyyy - 1900;

  //}

}
