namespace Common.ValueObjects.Database;

public readonly record struct ProgramName(string Value, SchemaName Schema) : ISchemaObjectName { }
