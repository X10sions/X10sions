namespace Common.Structures {

  public class IntHHMMSS : IntHHMM {
    public IntHHMMSS() : this(DateTime.Now) { }
    public IntHHMMSS(int hh, int mm, int ss) : base(hh, mm) {
      SS = ss;
    }
    public IntHHMMSS(DateTime d) : base(d) {
      SS = d.Second;
      Millisecond = d.Millisecond;
    }

    public IntHHMMSS(int hhmmss) : base(GetHHMM(hhmmss)) { SS = GetSS(hhmmss); }
    public IntHHMMSS(string hhmmss) : this(hhmmss.As(0)) { }

    #region Min & Max Values
    public new const string Format = "hhmmss";
    
    public static  readonly int MinSS = 0;// 00:00:00
    public static  readonly int MaxSS = 59; // 23:59:00

    public static  readonly int MinHHMMSS = 0;// 00:00:00
    public static  readonly int MaxHHMMSS = 235959; // 23:59:00

    public static new readonly IntHHMMSS MinValue = new IntHHMMSS(0);// 00:00:00
    public static new readonly IntHHMMSS MaxValue = new IntHHMMSS(235959); // 23:59:00
    #endregion

    int ss;
    public int SS { get => ss; set => ss = value.GetValueBetween(MinSS, MaxSS); }
    public int Millisecond { get; set; }

    public int HHMMSS {
      get => GetHHMMSS(HH, MM, SS);
      set {
        HH = GetHH(value);
        MM = GetMM(value);
        SS = GetSS(value);
      }
    }

    public DateTime Date {
      get => new DateTime(1, 1, 1, HH, MM, SS, Millisecond);
      set {
        HH = value.Hour;
        MM = value.Minute;
        SS = value.Second;
        Millisecond = value.Millisecond;
      }
    }

    public static new int GetHH(int hhmmss) => GetHHMM(hhmmss) / 100;
    public static int GetHHMM(int hhmmss) => hhmmss / 100;
    public static int GetHHMMSS(int hh, int mm, int ss) => GetHHMM(hh, mm) * 100 + ss;
    public static new int GetMM(int hhmmss) => GetHHMM(hhmmss) % 100;
    public static int GetSS(int hhmmss) => hhmmss % 100;

    public TimeSpan AsTimeSpan() => new TimeSpan(0, HH, MM, SS, Millisecond);

    public static implicit operator IntHHMMSS(decimal value) => new IntHHMMSS((int)value);
    public static implicit operator IntHHMMSS(int value) => new IntHHMMSS(value);
    public static implicit operator IntHHMMSS(DateTime value) => new IntHHMMSS(value);
    public static implicit operator IntHHMMSS(double value) => new IntHHMMSS((int)value);
    public static implicit operator IntHHMMSS(string value) => new IntHHMMSS(value.As(0));

    public static implicit operator DateTime(IntHHMMSS value) => value.Date;
    public static implicit operator decimal(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator double(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator int(IntHHMMSS value) => value.HHMMSS;
    public static implicit operator string(IntHHMMSS value) => value.ToString();

    #region IFormattable
    public override string ToString() => HHMMSS.ToString();
    public override string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion
  }
}
