using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntCYY(int Value) : IValueObject<int>, IFormattable {
  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public IntCYY() : this(DateTime.Now) { }
  public IntCYY(DateTime d) : this(d.Year) { }
  public IntCYY(Year yyyy) : this(yyyy.Value - 1900) { }
  public IntCYY(decimal value) : this((int)value) { }
  public IntCYY(IntC c, IntYY yy) : this((c.Value * 100) + yy.Value) { }
  public IntCYY(IntCYYMM cyymm) : this(cyymm.Value / 100) { }
  public IntCYY(IntCYYMMDD cyymmdd) : this(cyymmdd.CYYMM) { }

  public IntCYY(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 999;

  public static readonly IntCYY Min = new(MinValue);
  public static readonly IntCYY Max = new(MaxValue);

  public IntC C => new(this);
  public IntYY YY => new(this);
  public Year YYYY => new(this);
  public DateTime YearStartDateTime => new(YYYY.Value, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
  public DateTime YearEndDateTime => new(YYYY.Value, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified);
  public DateTime GetDate(int month, int day) => new DateTime(YYYY.Value, month, day, 0, 0, 0, DateTimeKind.Unspecified);

  #region IFormattable
  public override string ToString() => Value.ToString("000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  #endregion

  public static implicit operator IntCYY(decimal value) => new(value);
  public static implicit operator IntCYY(int value) => new(value);
  public static implicit operator IntCYY(DateTime value) => new(value);
}

public static class Extensions {
  public static IntCYY ToIntCYY(this decimal value) => new(value);
  public static IntCYY ToIntCYY(this int value) => new(value);
  public static IntCYY ToIntCYY(this DateTime value) => new(value);
}
