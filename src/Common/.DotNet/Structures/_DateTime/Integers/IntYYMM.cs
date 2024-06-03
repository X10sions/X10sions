using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntYYMM(int Value) : IValueObject<int> {
  public IntYYMM(IntYY yy, Month mm) : this(yy.Value * 100 + mm.Value) { }

  public const int MinValue = 0;
  public const int MaxValue = 9999;

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);

  public override string ToString() => Value.ToString("0000");

  public static readonly IntYYMM Min = new(MinValue);
  public static readonly IntYYMM Max = new(MaxValue);
}
