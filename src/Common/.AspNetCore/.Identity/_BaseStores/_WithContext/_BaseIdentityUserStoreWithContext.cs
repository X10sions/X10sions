using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityUserStoreWithContext<TContext, TUser, TKey>
    : _BaseIdentityStoreWithContext<TContext>
    , IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TUser, TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithConcurrency<TKey> {// _BaseIdentityUser_WithConcurrency<TKey> {

    public _BaseIdentityUserStoreWithContext(TContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber) { }
    #region IUserStore
    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default) => await this.xCreateAsync(user, cancellationToken);
    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default) => await this.xDeleteAsync(user, cancellationToken);
    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default) => await this.xUpdateAsync(user, cancellationToken);
    public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default) => await this.xFindByIdAsync(userId, cancellationToken);
    public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default) => await this.xFindByNameAsync(normalizedUserName, cancellationToken);
    public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetNormalizedUserNameAsync(cancellationToken);
    public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetUserIdAsync(cancellationToken);
    public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetUserNameAsync(cancellationToken);
    public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default) => await user.xSetNormalizedUserNameAsync(normalizedName, cancellationToken);
    public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default) => await user.xSetUserNameAsync(userName, cancellationToken);
    #endregion

    #region IQueryableUserStore
    public IQueryable<TUser> Users => Context.DbGetQueryable<TUser>();
    #endregion
  }
}