namespace Common.Data;

//public interface IHaveDbConnectionStringList {
//  List<IDbConnectionString> DbConnectionStrings { get; }
//}
//public interface IHaveDbConnectionStrings {
//  IDictionary<string, IDbConnectionString> DbConnectionStrings { get; }
//}

public interface IDbConnectionString {
  // https://github.com/serenity-is/Serenity/blob/master/src/Serenity.Net.Data/Connections/IConnectionString.cs

  //ISqlDialect Dialect { get; }
  string? Key { get; }
  string? ProviderName { get; }
  string Value { get; }
}

