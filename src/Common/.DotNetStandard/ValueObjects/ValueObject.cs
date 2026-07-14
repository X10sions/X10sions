namespace Common.ValueObjects;

public readonly record struct ValueObject<T>(T Value) : IValueObject<T> {
  public static ValueObject<int> For(int value) => new ValueObject<int>(value);
  public static ValueObject<string> For(string value) => new ValueObject<string>(value);
  public static ValueObject<string> ForNotNullOrWhiteSpace(string value) => string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Value must be non-empty", nameof(value)) : new ValueObject<string>(value.Trim());
  public static ValueObject<string> ForFixedLengthString(string value, int maxLength) => value.Length > maxLength ? throw new ArgumentException("Value is too long", nameof(value)) : new ValueObject<string>(value.Trim());

  public override string ToString() => $"{Value}";

  //public static implicit operator string(ValueObject<T> vo) => vo.ToString();
}