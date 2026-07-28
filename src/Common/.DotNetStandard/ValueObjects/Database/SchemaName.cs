using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct SchemaName(string Value) : IValueObject<string> {
  public const string SqlNamingSeparator = ".";
  public const string SystemNamingSeparator = "/";
  public string QualifiedPrefix(string separator) => string.IsNullOrWhiteSpace(Value) ? string.Empty : $"{Value}{separator}";

  public string SqlQualifiedPrefix => QualifiedPrefix(SqlNamingSeparator);
  public string SystemQualifiedPrefix => QualifiedPrefix(SystemNamingSeparator);
  public static SchemaName Empty { get; } = new(string.Empty);
}