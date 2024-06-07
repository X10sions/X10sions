using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHH(int Value)  : IValueObject<int> {
  public int Value { get; } = Value.Clamp(MinValue, MaxValue);
  public Hour Hour => new(Value);
  public override string ToString() => Value.ToString("00");
  public const string Format = "HH";
  public const int MinValue = 0;
  public const int MaxValue = 99;

  public static readonly IntHH Min = new(MinValue);
  public static readonly IntHH Max = new(MaxValue);
}
