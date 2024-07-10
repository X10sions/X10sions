using Common.ValueObjects;

namespace Common.Structures;

//public readonly record struct IntMM(int Value) : IValueObject<int> {
//  public int Value { get; } = Value.Clamp(MinValue, MaxValue);
//  public Minute Minute => new(Value);
//  public override string ToString() => Value.ToString("00");
//  public const string Format = "mm";
//  public const int MinValue = 0;
//  public const int MaxValue = 99;

//  public static readonly IntMM Min = new(MinValue);
//  public static readonly IntMM Max = new(MaxValue);

//}
