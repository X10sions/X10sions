using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Dapper{
  public class _BaseIdentityUserStore_Dapper<TUser, TKey> : _BaseIdentityUserStore<TUser, TKey>
    , IQueryableUserStore<TUser>
    , IUserEmailStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStore_Dapper(IDbConnection db, string baseSqlFrom, string baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      this.db = db;
      BaseSqlFrom = baseSqlFrom;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly IDbConnection db;
    public string BaseSqlFrom { get; }
    public string BaseSqlWhere { get; }

    #region IQueryableUserStore
    public override IQueryable<TUser> Users => throw new NotImplementedException();
    #endregion

    #region IUserStore
    public override Func<TUser, string> GetRoleOrUserDescription => x => x.EmailAddress;

    protected override async Task<bool> CreateAsync_Insert(TUser user, CancellationToken cancellationToken = default) => await db.InsertAsync(user) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) => await db.DeleteAsync(user);

    protected override async Task<TUser> FindByIdAsync_Select(string userId, CancellationToken cancellationToken = default) {
      var param = new { Id = userId };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(param.Id)} = @{nameof(param.Id)}";
      return await db.QuerySingleOrDefaultAsync<TUser>(sql, param);
    }

    protected override async Task<TUser> FindByNameAsync_Select(string normalizedUserName, CancellationToken cancellationToken = default) {
      var param = new { NormalizedName = normalizedUserName.ToLower() };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(param.NormalizedName)} = @{nameof(param.NormalizedName)}";
      return await db.QuerySingleOrDefaultAsync<TUser>(sql, param);
    }

    protected override async Task<TUser> FindByIdAndConcurrencyStampAsync(TUser user, CancellationToken cancellationToken = default) {
      var param = new { user.Id, user.ConcurrencyStamp };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(DefaultRoleOrUser.Id)} = @{nameof(param.Id)} And {nameof(DefaultRoleOrUser.ConcurrencyStamp)} = @{nameof(param.ConcurrencyStamp)}";
      return await db.QuerySingleOrDefaultAsync<TUser>(sql, param);
    }

    protected override async Task<bool> UpdateAsync_Update(TUser user, CancellationToken cancellationToken = default) => await db.UpdateAsync(user);
    #endregion

    #region IUserEmailStore
    public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) {
      var param = new { NormalizedEmail = normalizedEmail.ToLower() };
      var sql = $"SELECT * From {BaseSqlFrom} Where {BaseSqlWhere} And {nameof(param.NormalizedEmail)} = @{nameof(param.NormalizedEmail)}";
      return await db.QuerySingleOrDefaultAsync<TUser>(sql, param);
    }
    #endregion

    #region IUserPasswordStore
    #endregion

  }
}
