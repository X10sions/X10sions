namespace Common.Structures {
  public class IntHHMM : IntHH {
    public IntHHMM(int hh, int mm) : base(hh) {
      MM = mm;
    }
    public IntHHMM(DateTime d) : base(d) { MM = d.Minute; }
    public IntHHMM(int hhmm) : base(GetHH(hhmm)) { MM = GetMM(hhmm); }
    public IntHHMM(string hh, string mm) : this(hh.As(0), mm.As(0)) { }

    #region Min & Max Values
    public new const string Format = "hhmm";

    public static readonly int MinMM= 0;
    public static readonly int MaxMM= 59;

    public static readonly int MinHHMM = 0;
    public static readonly int MaxHHMM = 2359;

    public static new readonly IntHHMM MinValue = new IntHHMM(0);
    public static new readonly IntHHMM MaxValue = new IntHHMM(2359);
    #endregion

    int mm;
    public int MM { get => mm; set => mm = value.GetValueBetween(MinMM, MaxMM); }
    public int HHMM {
      get => GetHHMM(HH, MM);
      set {
        value = value.GetValueBetween(MinHHMM, MaxHHMM);
        HH = GetHH(value);
        MM = GetMM(value);
      }
    }

    public static int GetHH(int hhmm) => hhmm / 100;
    public static int GetHHMM(int hh, int mm) => hh * 100 + mm;
    public static int GetMM(int hhmm) => hhmm % 100;

    #region IFormattable
    public override string ToString() => HHMM.ToString();
    public override string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

    public DateTime GetDate(int ss, int millisecond = 0) => new DateTime(1, 1, 1, HH, MM, ss, millisecond);

    public static implicit operator IntHHMM(decimal value) => new IntHHMM((int)value);
    public static implicit operator IntHHMM(int value) => new IntHHMM(value);
    public static implicit operator IntHHMM(DateTime value) => new IntHHMM(value);
  }
}
