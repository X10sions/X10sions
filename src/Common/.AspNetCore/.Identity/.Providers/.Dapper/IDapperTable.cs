using System;
using System.Data;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public interface IDapperTable<TKey>
    where TKey : IEquatable<TKey> {
    IDbConnection Connection { get; }
    string TableName { get; }
  }
}
