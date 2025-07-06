namespace Common.ValueObjects;

public readonly record struct Money(decimal Amount, Currency Currency) {
  public override string ToString() => $"{Currency.Symbol}{Amount}";
}
