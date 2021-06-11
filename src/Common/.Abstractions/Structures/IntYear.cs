using System;

namespace Common.Structures {
  public class IntYear : IFormattable {
    public IntYear() { }

    public IntYear(DateTime d) : this(d.Year) { }
    public IntYear(int yyyy) : this() {
      Value = yyyy;
    }

    int _value = 1;
    public int Value { get => _value; set => _value = value.GetValueBetween(1, 9999); }

    public int CYY => Value - 1900;

    public DateTime StartDate => new DateTime(Value, 1, 1);
    public DateTime EndDate => new DateTime(Value, 12, 31);

    public static implicit operator IntYear(decimal value) => new IntYear((int)value);
    public static implicit operator IntYear(int value) => new IntYear(value);
    public static implicit operator IntYear(DateTime value) => new IntYear(value.Year);

    #region IFormattable
    public override string ToString() => Value.ToString();
    public string ToString(string format, IFormatProvider formatProvider) => ToString(ToString(), formatProvider);
    #endregion

  }
}
