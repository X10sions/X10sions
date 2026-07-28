using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct ProcedureName(string Value) : IValueObject<string> { }
