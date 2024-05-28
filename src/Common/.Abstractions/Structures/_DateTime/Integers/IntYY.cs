using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntYY(int Value) : IValueObject<int> {
  public IntYY(IntCYY cyy) : this(cyy.Value % 100) { }
  public IntYY(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 99;

  public int Value { get; init; } = Value.AssertBetween(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly IntYY Min = new(MinValue);
  public static readonly IntYY Max = new(MaxValue);
}
