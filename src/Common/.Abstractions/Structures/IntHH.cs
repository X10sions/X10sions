namespace Common.Structures {
  public class IntHH : IFormattable {
    public IntHH(int hh) {
      HH = hh;
    }
    public IntHH(DateTime d) { HH = d.Hour; }

    #region Min & Max Values
    public const string Format = "hh";

    public static readonly int MinHH = 0;
    public static readonly int MaxHH = 23;

    public static readonly IntHH MinValue = new IntHH(0);
    public static readonly IntHH MaxValue = new IntHH(23);
    #endregion

    int hh;
    public int HH { get => hh; set => hh = value.GetValueBetween(MinHH, MaxHH); }
    #region IFormattable
    public override string ToString() => HH.ToString();
    public virtual string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

    public static implicit operator IntHH(decimal value) => new IntHH((int)value);
    public static implicit operator IntHH(int value) => new IntHH(value);
    public static implicit operator IntHH(DateTime value) => new IntHH(value);
  }
}
