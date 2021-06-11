using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public abstract class _BaseIdentityUserStore<TUser, TKey> : _BaseIdentityStore<TUser, TKey>
    , IIdentityUserStore<TUser, TKey>
    , IUserEmailStore<TUser>
    , IUserPasswordStore<TUser>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithConcurrency<TKey>, IIdentityUserWithPassword<TKey>, IIdentityUserWithEmail<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStore(IdentityErrorDescriber errorDescriber) : base(errorDescriber) { }


    #region IQueryableUserStore
    public abstract IQueryable<TUser> Users { get; }
    #endregion

    #region IUserStore
    public override Func<TUser, string> GetRoleOrUserDescription => x => x.EmailAddress;

    protected override async Task<TUser> FindByIdAsync_Select(string userId, CancellationToken cancellationToken = default) => (TUser)await Users.xFindByIdAsync(userId, cancellationToken);
    protected override async Task<TUser> FindByIdAndConcurrencyStampAsync(TUser user, CancellationToken cancellationToken = default) => (TUser)await Users.FindByIdAndConcurrencyStampAsync(user, cancellationToken);
    protected override async Task<TUser> FindByNameAsync_Select(string normalizedUserName, CancellationToken cancellationToken = default) => (TUser)await Users.xFindByNameAsync(normalizedUserName, cancellationToken);

    public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetNormalizedUserNameAsync(cancellationToken);
    public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetUserIdAsync(cancellationToken);
    public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetUserNameAsync(cancellationToken);
    public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default) => await user.xSetNormalizedUserNameAsync(normalizedName, cancellationToken);
    public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default) => await user.xSetUserNameAsync(userName, cancellationToken);

    protected override void UpdateAsync_SetConcurrencyStamp(TUser user) => user.ConcurrencyStamp = Guid.NewGuid().ToString();

    #endregion

    #region IUserEmailStore
    public virtual async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) => (TUser)await Users.xFindByEmailAsync(normalizedEmail, cancellationToken);

    public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetEmailAsync(cancellationToken);
    public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetEmailConfirmedAsync(cancellationToken);
    public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetNormalizedEmailAsync(cancellationToken);
    public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken = default) => await user.xSetEmailAsync(email, cancellationToken);
    public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default) => await user.xSetEmailConfirmedAsync(confirmed, cancellationToken);
    public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default) => await user.xSetNormalizedEmailAsync(normalizedEmail, cancellationToken);
    #endregion

    #region IUserPasswordStore
    public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default) => await user.xSetPasswordHashAsync(passwordHash, cancellationToken);
    public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default) => await user.xGetPasswordHashAsync(cancellationToken);
    public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default) => await user.xHasPasswordAsync(cancellationToken);
    #endregion

  }
}