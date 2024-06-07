using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntSS(int Value) : IValueObject<int> {
  public int Value { get; } = Value.Clamp(MinValue, MaxValue);
  public Second Second => new(Value);
  public override string ToString() => Value.ToString("00");
  public const string Format = "ss";
  public const int MinValue = 0;
  public const int MaxValue = 99;

  public static readonly IntSS Min = new(MinValue);
  public static readonly IntSS Max = new(MaxValue);

}