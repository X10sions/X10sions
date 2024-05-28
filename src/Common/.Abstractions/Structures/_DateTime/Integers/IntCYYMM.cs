using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct IntCYYMM(int Value) : IValueObject<int>, IFormattable {
  public IntCYYMM() : this(DateTime.Now) { }
  public IntCYYMM(IntC c, IntYY yy, Month mm) : this(c.Value * 10000 + yy.Value * 100 + mm.Value) { }
  public IntCYYMM(DateTime d) : this(new IntCYY(d), new Month(d.Month)) { }
  public IntCYYMM(string c, string yy, string mm) : this(new IntC(c), new IntYY(yy), new Month(mm)) { }
  public IntCYYMM(IntCYY cyy, Month mm) : this(cyy.Value * 100 + mm.Value) { }
  public IntCYYMM(IntCYYMMDD cyymmdd) : this(cyymmdd.Value / 100) { }

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);

  #region Min & Max Values
  public const int MinValue= 0;
  public const int MaxValue = 99999;
  public const int MaxValidValue = 99912;

  public static readonly IntCYYMM Min = new(MinValue);
  public static readonly IntCYYMM Max = new(MaxValue);
  public static readonly IntCYYMM MaxValid= new(MaxValidValue);
  #endregion

  public Month MM => new(this);
  public IntC C => new(this);
  public IntCYY CYY => new(this);
  public Year YYYY => new(this);

  public IntCYYMMDD CYYMM00 => new(Value * 100);
  public IntCYYMMDD CYYMM01 => new(CYYMM00.Value + 1);
  public IntCYYMMDD CYYMM99 => new(CYYMM00.Value + 99);

  #region IFormattable
  public override string ToString() => Value.ToString("00000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  #endregion

  public DateTime GetDate(int day) => new DateTime(YYYY.Value, MM.Value, day);

  public static implicit operator IntCYYMM(decimal value) => new IntCYYMM((int)value);
  public static implicit operator IntCYYMM(int value) => new IntCYYMM(value);
  public static implicit operator IntCYYMM(DateTime value) => new IntCYYMM(value);

}
