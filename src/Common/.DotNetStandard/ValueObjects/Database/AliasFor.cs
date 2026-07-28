using System.Data;

namespace Common.ValueObjects.Database;

public readonly record struct AliasFor(AliasName Alias, ISchemaObjectName For) {

  public string CreateSql(string separator) => $"Create Alias {Alias.QualifiedName(separator)} For {For.QualifiedName(separator)})";
  public string DropSql(string separator) => $"Drop Alias {Alias.QualifiedName(separator)}";

  public int Create(IDbConnection dbConnection, string separator) => dbConnection.ExecuteNonQuery(CreateSql(separator));
  public int Drop(IDbConnection dbConnection, string separator) => dbConnection.ExecuteNonQuery(DropSql(separator));
}
