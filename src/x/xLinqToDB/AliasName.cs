using Common.ValueObjects;

namespace LinqToDB;

public readonly record struct AliasName(string Value): IValueObject<string> {

  public  string CreateSql(string aliasForName) => $"Create Alias {Value} For {aliasForName})";
  public  string DropSql => $"Drop Alias {Value}";

}
