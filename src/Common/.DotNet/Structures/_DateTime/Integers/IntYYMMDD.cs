using Common.ValueObjects;

namespace Common.Structures;

//public readonly record struct IntYYMMDD(int Value) : IValueObject<int> {
//  public IntYYMMDD(IntYY yy, Month mm, Day dd) : this(new IntYYMM(yy, mm).Value * 100 + dd.Value) { }

//  public const int MinValue = 0;
//  public const int MaxValue = 999999;

//  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
//  public override string ToString() => Value.ToString("000000");

//  public static readonly IntYYMMDD Min = new(MinValue);
//  public static readonly IntYYMMDD Max = new(MaxValue);
//}
