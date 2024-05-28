using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct Year(int Value) : IValueObject<int>, IFormattable {
  public Year(DateTime d) : this(d.Year) { }
  public Year(IntCYY cyy) : this(cyy.Value + 1900) { }
  public Year(IntCYYMM cyymm) : this((cyymm.Value / 100) + 1900) { }
  public Year(IntCYYMMDD cyymmdd) : this((cyymmdd.Value / 10000) + 1900) { }

  public const int MinValue = 1;
  public const int MinValidValue = 1;
  public const int MaxValue = 9999;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("0000");
  public string ToString(string format, IFormatProvider formatProvider) => ToString().ToString(formatProvider);
  public DateTime StartDate => new DateTime(Value, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
  public DateTime EndDate => new DateTime(Value, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);

  public static readonly Year Min = new(MinValue);
  public static readonly Year MinValid = new(MinValidValue);
  public static readonly Year Max = new(MaxValue);

  public static implicit operator Year(decimal value) => new((int)value);
  public static implicit operator Year(int value) => new(value);
  public static implicit operator Year(DateTime value) => new(value.Year);
}