using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct CatalogName(string Value) : IValueObject<string> { }
