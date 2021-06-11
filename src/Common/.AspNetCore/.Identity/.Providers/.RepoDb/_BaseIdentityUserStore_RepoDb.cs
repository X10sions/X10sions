using Microsoft.AspNetCore.Identity;
using RepoDb;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.RepoDb {
  public class _BaseIdentityUserStore_RepoDb<TUser, TKey> : _BaseIdentityUserStore<TUser, TKey>
    , IQueryableUserStore<TUser>
    , IUserEmailStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStore_RepoDb(IDbConnection dbConnection, Expression<Func<TUser, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      db = dbConnection;
      BaseSqlWhere = baseSqlWhere;
    }

    private readonly IDbConnection db;
    public Expression<Func<TUser, bool>> BaseSqlWhere { get; }

    #region IQueryableUserStore
    public override IQueryable<TUser> Users => throw new NotImplementedException();
    #endregion

    #region IUserStore
    protected override async Task<bool> CreateAsync_Insert(TUser user, CancellationToken cancellationToken = default) => await db.InsertAsync<TUser, TKey>(user) != null;
    //protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) => await db.DeleteAsync(user) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) => await db.DeleteAsync<TUser>(x => x.Id.Equals(user.Id)) > 0;

    protected override async Task<TUser> FindByIdAsync_Select(string userId, CancellationToken cancellationToken = default) => (await db.QueryAsync<TUser>(x => x.Id.Equals(userId))).FirstOrDefault();
    protected override async Task<TUser> FindByNameAsync_Select(string normalizedUserName, CancellationToken cancellationToken = default) => (await db.QueryAsync<TUser>(x => x.NormalizedName.Equals(normalizedUserName))).FirstOrDefault();
    protected override async Task<TUser> FindByIdAndConcurrencyStampAsync(TUser user, CancellationToken cancellationToken = default) => (await db.QueryAsync<TUser>(x => x.HasIdAndConcurrencyStamp(user))).FirstOrDefault();
    protected override async Task<bool> UpdateAsync_Update(TUser user, CancellationToken cancellationToken = default) => await db.UpdateAsync(user) > 0;
    #endregion

    #region IUserEmailStore
    #endregion

    #region IUserPasswordStore
    #endregion

  }
}
