namespace Common.Structures {
  public class IntCYYMM : IntCYY {
    public IntCYYMM(int c, int yy, int mm) : base(c, yy) { MM = mm; }
    public IntCYYMM(DateTime d) : base(d) { MM = d.Month; }
    public IntCYYMM(int cyymm) : base(GetCYY(cyymm)) { MM = GetMM(cyymm); }
    public IntCYYMM(string c, string yy, string mm) : this(c.As(0), yy.As(0), mm.As(0)) { }

    #region Min & Max Values
    public static readonly int MinMM = 0;
    public static readonly int MaxMM = 99;

    public static readonly int MinCYYMM = 0;
    public static readonly int MaxCYYMM = 99999;

    public static new readonly IntCYYMM MinValue = new IntCYYMM(0);
    public static new readonly IntCYYMM MaxValue = new IntCYYMM(99999);
    #endregion

    int mm;
    public int MM { get => mm; set => mm = value.GetValueBetween(MinMM, MaxMM); }

    public int CYYMM {
      get => GetCYYMM(CYY, MM);
      set {
        value = value.GetValueBetween(MinCYYMM, MaxCYYMM);
        CYY = GetCYY(value);
        MM = GetMM(value);
      }
    }

    public static new int GetC(int cyymm) => GetCYY(cyymm)/ 100;
    public static new int GetCYY(int cyymm) => cyymm / 100;
    public static int GetCYYMM(int cyy, int mm) => cyy * 100 + mm;
    public static int GetMM(int cyymm) => cyymm % 100;
    public static int GetYYMM(int yy, int mm) => yy * 100 + mm;

    public int CYYMM00 => CYYMM * 100;
    public int CYYMM01 => CYYMM00 + 1;
    public int CYYMM99 => CYYMM00 + 99;

    #region IFormattable
    public override string ToString() => CYYMM.ToString();
    public override string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

    public DateTime GetDate(int day) => new DateTime(YYYY, MM, day);

    public static implicit operator IntCYYMM(decimal value) => new IntCYYMM((int)value);
    public static implicit operator IntCYYMM(int value) => new IntCYYMM(value);
    public static implicit operator IntCYYMM(DateTime value) => new IntCYYMM(value);

  }
}
