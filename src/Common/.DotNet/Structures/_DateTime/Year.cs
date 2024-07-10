using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Year(int Value) : IValueObject<int>, IFormattable {
  public Year(DateOnly d) : this(d.Year) { }
  public Year(DateTime d) : this(d.Year) { }
  //public Year(IntCYY cyy) : this(cyy.YYYY) { }
  //public Year(IntCYYMM cyymm) : this(cyymm.YYYY) { }
  //public Year(IntCYYMMDD cyymmdd) : this(cyymmdd.YYYY) { }

  public const int MinValue = 1;
  public const int MinValidValue = 1;
  public const int MaxValue = 9999;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("0000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  public DateTime StartDate => new DateTime(Value, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
  public DateTime EndDate => new DateTime(Value, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);
  public bool IsLeapYear => DateTime.IsLeapYear(Value);
  public int DaysInYear => IsLeapYear ? 366 : 365;
  public int DaysInMonth(Month month) => DateTime.DaysInMonth(Value, month.Value);

  public static ReadOnlySpan<byte> DaysInMonth365 => [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
  public static ReadOnlySpan<byte> DaysInMonth366 => [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

  public static readonly Year Min = new(MinValue);
  public static readonly Year MinValid = new(MinValidValue);
  public static readonly Year Max = new(MaxValue);

  public static implicit operator Year(decimal value) => new((int)value);
  public static implicit operator Year(int value) => new(value);
  public static implicit operator Year(DateTime value) => new(value.Year);
}
public static class YearExtensions {
  public static Year GetYear(this DateTime dateTime) => new(dateTime);
  public static Year GetYear(this DateOnly dateOnly) => new(dateOnly);
}