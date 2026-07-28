using Common.Models;

namespace Common.ValueObjects.Database;

public readonly record struct ProcedureType(string Value) : IValueObject<string> { }
