using Common.ValueObjects;

namespace Common.Structures;
public readonly record struct Hour(int Value) : IValueObject<int> {
  public Hour(DateTime d) : this(d.Hour) { }
  public Hour(TimeOnly t) : this(t.Hour) { }
  //public Hour(TimeSpan   ts) : this(ts.Hours) { }
  public Hour(decimal value) : this((int)value) { }
  //public Hour(IntHH hh) : this(hh.Value) { }
  //public Hour(IntHHMM hhmm) : this(hhmm.HH) { }
  //public Hour(IntHHMMSS hhmmss) : this(hhmmss.Value / 10000) { }
  public Hour(string value) : this(int.Parse(value)) { }

  public const int MinValue = 0;
  public const int MaxValue = 23;
  public const string Format = "hh";

  public int Value { get; init; } = Value.Clamp(MinValue, MaxValue);
  public override string ToString() => Value.ToString("00");

  public static readonly Hour Min = new(MinValue);
  public static readonly Hour Max = new(MaxValue);

  public static implicit operator Hour(decimal value) => new(value);
  public static implicit operator Hour(int value) => new(value);
  public static implicit operator Hour(DateTime value) => new(value);
}

public static class HourExtensions {
  public static TimeOnly ToTimeOnly(this Hour hour, Minute minute, Second second) => new TimeOnly(hour.Value, minute.Value, second.Value);
}
