namespace Common.Structures {
  public class IntCYY : IFormattable {
    public IntCYY(int c, int yy) {
      C = c;
      YY = yy;
    }
    public IntCYY(DateTime d) : this(GetCYY(d.Year)) { }
    public IntCYY(int cyy) : this(GetC(cyy), GetYY(cyy)) { }

    #region Min & Max Values
    public static readonly int MinC = 0;
    public static readonly int MaxC = 9;

    public static readonly int MinYY = 0;
    public static readonly int  MaxYY = 99;

    public static readonly int MinCYY = 0;
    public static readonly int MaxCYY = 999;

    public static readonly int MinYYYY = 1;
    public static readonly int MaxYYYY = 9999;

    //public const IntCYY MinValue = new IntCYY(0);
    //public const IntCYY MaxValue = new IntCYY(999);
   
    public static readonly IntCYY? MinValue = new IntCYY(0);
    public static readonly IntCYY? MaxValue = new IntCYY(999);
    #endregion

    int c ;
    int yy ;

    /// <summary>
    /// Centuries after 1900
    /// </summary>
    public int C { get => c; set => c = value.GetValueBetween(MinC, MaxC); }
    public int YY { get => yy; set => yy = value.GetValueBetween(MinYY, MaxYY); }
    public int CYY {
      get => GetCYY(C, YY);
      set {
        value = value.GetValueBetween(MinCYY, MaxCYY);
        C = GetC(value);
        YY = GetYY(value);
      }
    }
    public int YYYY { get => GetYYYY(CYY); set => CYY = GetCYY(value.GetValueBetween(MinYYYY, MaxYYYY)); }

    public static int GetC(int cyy) => cyy / 100;
    public static int GetCYY(int c, int yy) => (c * 100) + yy;
    public static int GetCYY(int yyyy) => yyyy - 1900;
    public static int GetYY(int cyy) => cyy % 100;
    public static int GetYYYY(int cyy) => cyy + 1900;

    public DateTime YearStartDateTime => new DateTime(YYYY, 1, 1);
    public DateTime YearEndDateTime => new DateTime(YYYY, 12, 31, 23, 59, 59, 999);

    public DateTime GetDate(int month, int day) => new DateTime(YYYY, month, day);

    #region IFormattable
    public override string ToString() => CYY.ToString();
    public virtual string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

    public static implicit operator IntCYY(decimal value) => new IntCYY((int)value);
    public static implicit operator IntCYY(int value) => new IntCYY(value);
    public static implicit operator IntCYY(DateTime value) => new IntCYY(value);

  }
}
