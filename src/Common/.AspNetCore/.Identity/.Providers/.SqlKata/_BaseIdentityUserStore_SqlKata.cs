using Microsoft.AspNetCore.Identity;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.SqlKata {

  public class _BaseIdentityUserStore_SqlKata<TUser, TKey> : _BaseIdentityUserStore<TUser, TKey>
    , IQueryableUserStore<TUser>
    , IUserEmailStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStore_SqlKata(Query userTableQuery, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      UserTableQuery = userTableQuery;
    }

    public _BaseIdentityUserStore_SqlKata(QueryFactory queryFactory, string userTableName, IdentityErrorDescriber errorDescriber) : this(queryFactory.Query(userTableName), errorDescriber) { }

    public override IQueryable<TUser> Users => throw new NotImplementedException();
    public Query UserTableQuery { get; }

    public virtual object InsertUser(TUser user) => UpdateUser(user).MergeToExpandoObject(new { user.Id });

    public virtual object UpdateUser(TUser user) => new {
      user.ConcurrencyStamp,
      user.EmailAddress,
      user.IsEmailConfirmed,
      user.Name,
      user.NormalizedEmailAddress,
      user.NormalizedName,
      user.PasswordHash
    };

    #region IUserStore
    public override Func<TUser, string> GetRoleOrUserDescription => x => x.EmailAddress;

    protected override async Task<bool> CreateAsync_Insert(TUser user, CancellationToken cancellationToken = default) => await UserTableQuery.InsertAsync(InsertUser(user)) > 0;
    protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) => await UserTableQuery.Where(nameof(user.Id), user.Id).DeleteAsync() > 0;
    protected override async Task<TUser> FindByIdAsync_Select(string userId, CancellationToken cancellationToken = default) => await UserTableQuery.Where(nameof(DefaultRoleOrUser.Id), userId).FirstOrDefaultAsync<TUser>();
    protected override async Task<TUser> FindByNameAsync_Select(string normalizedUserName, CancellationToken cancellationToken = default) => await UserTableQuery.Where(nameof(DefaultRoleOrUser.NormalizedName), normalizedUserName.ToLower()).FirstOrDefaultAsync<TUser>();
    protected override async Task<TUser> FindByIdAndConcurrencyStampAsync(TUser user, CancellationToken cancellationToken = default) => await UserTableQuery.Where(nameof(user.Id), user.Id).Where(nameof(user.ConcurrencyStamp), user.ConcurrencyStamp).FirstOrDefaultAsync<TUser>();
    protected override async Task<bool> UpdateAsync_Update(TUser user, CancellationToken cancellationToken = default) => await UserTableQuery.Where(nameof(user.Id), user.Id).UpdateAsync(UpdateUser(user)) > 0;
    #endregion

    #region IUserEmailStore
    public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) => await UserTableQuery.Where(nameof(DefaultRoleOrUser.NormalizedEmailAddress), normalizedEmail.ToLower()).FirstOrDefaultAsync<TUser>();

    #endregion

    #region IUserPasswordStore
    #endregion

  }

}
