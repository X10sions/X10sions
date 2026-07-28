namespace Common.ValueObjects.Database;

public readonly record struct AliasName(string Value, SchemaName Schema) : ISchemaObjectName { }
