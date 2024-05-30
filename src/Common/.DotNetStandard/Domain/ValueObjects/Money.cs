namespace Common.Domain.ValueObjects;

public record struct Money(decimal Amount, Currency Currency) { }
