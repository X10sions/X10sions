using Common.Models;

namespace Common.ValueObjects.Database;

public interface ISchemaObjectName : IValueObject<string> {
  public SchemaName Schema { get; }
}

public static class Extensions {

  public static string QualifiedName(this ISchemaObjectName schemaObject, string separator) => schemaObject.Schema.QualifiedPrefix(separator) + schemaObject.Value;
  public static string SqlQualifiedName(this ISchemaObjectName schemaObject) => schemaObject.Schema.SqlQualifiedPrefix + schemaObject.Value;
  public static string SystemQualifiedName(this ISchemaObjectName schemaObject) => schemaObject.Schema.SystemQualifiedPrefix + schemaObject.Value;

}