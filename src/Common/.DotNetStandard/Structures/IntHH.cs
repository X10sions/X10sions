using Common.ValueObjects;

namespace Common.Structures;

public readonly record struct IntHH(int Value) : IValueObject<int> {
  //  public IntHH(DateTime dt) : this(dt.Hour) { }
  //  public IntHH(TimeOnly t) : this(t.Hour) { }
  //  public IntHH(TimeSpan t) : this(t.Hours) { }
  //  //public IntHH(IntHHMM hhmm) : this(hhmm.Value / 100) { }


  //  public Hour Hour => new(Value);

    public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);

  //  public override string ToString() => Value.ToString("00");

  //  public const string TimeFormat = "HH";
  public const int MinValue = 0;
  public const int MaxValue = 99;

  //  public static readonly IntHH Min = new(MinValue);
  //  public static readonly IntHH Max = new(MaxValue);

  //  public static implicit operator IntHH(decimal value) => new((int)value);
  //  public static implicit operator IntHH(int value) => new(value);
  //  public static implicit operator IntHH(DateTime value) => new(value);
  //  public static implicit operator IntHH(TimeOnly value) => new(value);
  //  public static implicit operator IntHH(double value) => new((int)value);
  //  public static implicit operator IntHH(string value) => new(value.As(0));
}
