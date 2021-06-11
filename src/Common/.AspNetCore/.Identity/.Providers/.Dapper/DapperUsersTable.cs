using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Dapper {

  public class DapperUsersTable<TKey, TUser> : IDapperTable<TKey>
  where TKey : IEquatable<TKey>
  where TUser : class, IIdentityUser<TKey> {

    public IDbConnection Connection { get; }
    public string TableName { get; } = "dbo.CustomUser";

    public DapperUsersTable(IDbConnection connection, string tableName) {
      Connection = connection;
      TableName = tableName;
    }

    public DapperUsersTable(IDbConnection connection, IConfiguration configuration) : this(connection, configuration["DapperUsersTableName"]) { }

    public virtual async Task<IdentityResult> CreateAsync(TUser user) {
      var sql = $@"INSERT INTO {TableName} VALUES (
@{nameof(user.Id)},
@{nameof(user.Name)})";
      var rows = await Connection.ExecuteAsync(sql, new { user.Id, user.Name });
      if (rows > 0) {
        return IdentityResult.Success;
      }
      return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Name}." });
    }

    public async Task<IdentityResult> DeleteAsync(TUser user) {
      var sql = $"DELETE FROM {TableName} WHERE Id = @{nameof(user.Id)}";
      var rows = await Connection.ExecuteAsync(sql, new { user.Id });
      if (rows > 0) {
        return IdentityResult.Success;
      }
      return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Name}." });
    }

    public async Task<TUser> FindByIdAsync(string userId) {
      var sql = $@"SELECT * FROM {TableName} WHERE Id = @Id;";
      return await Connection.QuerySingleOrDefaultAsync<TUser>(sql, new {
        Id = userId
      });
    }

    public async Task<TUser> FindByNameAsync(string userName) {
      var sql = $"SELECT * FROM {TableName} WHERE UserName = @UserName;";
      return await Connection.QuerySingleOrDefaultAsync<TUser>(sql, new {
        UserName = userName
      });
    }

  }

  public class DapperUsersWithEmailAndPasswordTable<TKey, TUser> : DapperUsersTable<TKey, TUser>
   where TKey : IEquatable<TKey>
   where TUser : class, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey> {

    public DapperUsersWithEmailAndPasswordTable(IDbConnection connection, string tableName) : base(connection, tableName) { }

    public DapperUsersWithEmailAndPasswordTable(IDbConnection connection, IConfiguration configuration) : base(connection, configuration) { }

    public override async Task<IdentityResult> CreateAsync(TUser user) {
      var sql = $@"INSERT INTO {TableName} VALUES (
@{nameof(user.Id)},
@{nameof(user.EmailAddress)},
@{nameof(user.IsEmailConfirmed)},
@{nameof(user.PasswordHash)},
@{nameof(user.Name)})";
      var rows = await Connection.ExecuteAsync(sql, new { user.Id, user.EmailAddress, user.IsEmailConfirmed, user.PasswordHash, user.Name });
      if (rows > 0) {
        return IdentityResult.Success;
      }
      return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Name}." });
    }


  }

}
