using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntCYY(int Value) : IValueObject<int>, IFormattable {
  public IntCYY() : this(System.DateTime.Now) { }
  public IntCYY(DateTime d) : this(new Year(d.Year)) { }
  public IntCYY(Year yyyy) : this(yyyy.Value - 1900) { }
  public IntCYY(decimal value) : this((int)value) { }
  //public IntCYY(IntC c, IntYY yy) : this((c.Value * 100) + yy.Value) { }
  public IntCYY(IntCYYMM cyymm) : this(cyymm.CYY) { }
  public IntCYY(IntCYYMMDD cyymmdd) : this(cyymmdd.CYYMM) { }

  public IntCYY(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 999;

  public static readonly IntCYY Min = new(MinValue);
  public static readonly IntCYY Max = new(MaxValue);

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public int C => Value / 100;
  public IntC IntC => new(this);
  //public IntYY IntYY => new(this);
  public Year Year => new(this);
  public int YY => Value % 100;
  public int YYYY => Value + 1900;

  public DateOnly DateOnly(int month, int day) => new DateOnly(YYYY, month, day);
  public DateTime DateTime(int month, int day, int hour = Hour.MinValue, int minute = Minute.MinValue, int second = Second.MinValue, int millisecond = Millisecond.MinValue) => new DateTime(YYYY, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
  public DateTime YearStartDateTime(int hour = Hour.MinValue, int minute = Minute.MinValue, int second = Second.MinValue, int millisecond = Millisecond.MinValue) => new(YYYY, 1, 1, hour, minute, second, millisecond, DateTimeKind.Unspecified);
  public DateTime YearEndDateTime(int hour = Hour.MaxValue, int minute = Minute.MaxValue, int second = Second.MaxValue, int millisecond = Millisecond.MaxValue) => new(YYYY, 12, 31, hour, minute, second, millisecond, DateTimeKind.Unspecified);

  #region IFormattable
  public override string ToString() => Value.ToString("000");
  public string ToString(string? format, IFormatProvider? formatProvider) => ToString().ToString(formatProvider);
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
