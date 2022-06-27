namespace Common.Structures {
  public class IntYear : IFormattable {
    public IntYear() { }

    public IntYear(DateTime d) : this(d.Year) { }
    public IntYear(int yyyy) : this() {
      YYYY = yyyy;
    }

    int yyyy = 1;
    public int YYYY { get => yyyy; set => yyyy = value.GetValueBetween(1, 9999); }

    public DateTime StartDate => new DateTime(YYYY, 1, 1);
    public DateTime EndDate => new DateTime(YYYY, 12, 31);

    public static implicit operator IntYear(decimal value) => new IntYear((int)value);
    public static implicit operator IntYear(int value) => new IntYear(value);
    public static implicit operator IntYear(DateTime value) => new IntYear(value.Year);

    #region IFormattable
    public override string ToString() => YYYY.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
    #endregion

  }
}
