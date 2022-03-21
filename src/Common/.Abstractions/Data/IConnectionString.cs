namespace Common.Data;
  public interface IConnectionString {
  // https://github.com/serenity-is/Serenity/blob/master/src/Serenity.Net.Data/Connections/IConnectionString.cs

  //ISqlDialect Dialect { get; }
  string? Key { get; }
  string Value { get; }
  string? ProviderName { get; }
}