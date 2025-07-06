namespace Common.ValueObjects;

public readonly record struct Currency(string Name, string Code, string Symbol) {
  public static Currency AustralianDollar => new Currency(nameof(AustralianDollar), "AUD", "$");
  public static Currency Euro => new Currency(nameof(Euro), "EUR", "€");
  public static Currency CanadianDollar => new Currency(nameof(CanadianDollar), "CAD", "$");
  public static Currency USDollar => new Currency(nameof(USDollar), "USD", "$");
};
