using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public class _BaseIdentityUserStoreWithContext_Microsoft<TUser, TRole, TKey, TUserClaim, TUserLogin, TUserRole, TUserToken>
    : _BaseIdentityUserStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>
    , IIdentityUserAuthenticationTokenStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey, TUserToken>, IUserAuthenticationTokenStore<TUser>
    , IIdentityUserClaimStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey, TUserClaim>, IUserClaimStore<TUser>
    , IIdentityUserLoginStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey, TUserLogin>, IUserLoginStore<TUser>
    , IIdentityUserRoleStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey, TUserRole, TRole>, IUserRoleStore<TUser>

    , IIdentityUserWithEmailStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserEmailStore<TUser>
    , IIdentityUserWithLockoutStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserLockoutStore<TUser>
    , IIdentityUserWithPasswordStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserPasswordStore<TUser>
    , IIdentityUserWithPhoneNumberStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserPhoneNumberStore<TUser>
    , IIdentityUserWithSecurityStampStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserSecurityStampStore<TUser>
    , IIdentityUserWithTwoFactorStoreWithContext<IIdentityContext_Microsoft<TKey>, TUser, TKey>, IUserTwoFactorStore<TUser>
    where TUser : _BaseIdentityUser_Microsoft<TKey>
    where TRole : _BaseIdentityRole_Microsoft<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : class, IIdentityUserClaim<TKey>, new()
    where TUserLogin : class, IIdentityUserLogin<TKey>, new()
    where TUserRole : class, IIdentityUserRole<TKey>, new()
    where TUserToken : class, IIdentityUserToken<TKey>, new() {

    // https://github.com/aspnet/AspNetCore/blob/bfec2c14be1e65f7dd361a43950d4c848ad0cd35/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public _BaseIdentityUserStoreWithContext_Microsoft(IIdentityContext_Microsoft<TKey> context, IdentityErrorDescriber errorDescriber)
      : base(context, errorDescriber) { }

    #region IIdentityUserAuthenticationTokenStore
    // IUserAuthenticationTokenStore
    public async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken) => await this.xGetTokenAsync(user, loginProvider, name, cancellationToken);
    public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken) => await this.xRemoveTokenAsync(user, loginProvider, name, cancellationToken);
    public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken) => await this.xSetTokenAsync(user, loginProvider, name, value, cancellationToken);
    #endregion

    #region IIdentityUserClaimStore
    //IUserClaimStore
    public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) => await this.xAddClaimsAsync(user, claims, cancellationToken);
    public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default) => await this.xGetClaimsAsync(user, cancellationToken);
    public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default) => await this.xGetUsersForClaimAsync(claim, cancellationToken);
    public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default) => await this.xRemoveClaimsAsync(user, claims, cancellationToken);
    public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default) => await this.xReplaceClaimAsync(user, claim, newClaim, cancellationToken);
    #endregion

    #region IIdentityUserLoginStore
    //IUserLoginStore
    public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default) => await this.xAddLoginAsync(user, login, cancellationToken);
    public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken) => await this.xFindByLoginAsync(loginProvider, providerKey, cancellationToken);
    public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default) => await this.xGetLoginsAsync(user, cancellationToken);
    public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default) => await this.xRemoveLoginAsync(user, loginProvider, providerKey, cancellationToken);
    #endregion

    #region IIdentityUserRoleStore
    //IUserRoleStore
    public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken) => await this.xAddToRoleAsync(user, roleName, cancellationToken);
    public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken) => await this.xGetRolesAsync(user, cancellationToken);
    public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) => await this.xGetUsersInRoleAsync(roleName, cancellationToken);
    public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken) => await this.xIsInRoleAsync(user, roleName, cancellationToken);
    public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken) => await this.xRemoveFromRoleAsync(user, roleName, cancellationToken);
    #endregion

    #region IIdentityUserWithEmailStore
    //IUserEmailStore
    public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default) => await this.xFindByEmailAsync(normalizedEmail, cancellationToken);
    public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken) => await user.xGetEmailAsync(cancellationToken);
    public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken) => await user.xGetEmailConfirmedAsync(cancellationToken);
    public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken) => await user.xGetNormalizedEmailAsync(cancellationToken);
    public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken) => await user.xSetEmailAsync(email, cancellationToken);
    public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken) => await user.xSetEmailConfirmedAsync(confirmed, cancellationToken);
    public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken) => await user.xSetNormalizedEmailAsync(normalizedEmail, cancellationToken);
    #endregion

    #region IIdentityUserWithLockoutStore
    //IUserLockoutStore
    public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken) => await this.xGetAccessFailedCountAsync(user, cancellationToken);
    public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken) => await this.xGetLockoutEnabledAsync(user, cancellationToken);
    public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken) => await this.xGetLockoutEndDateAsync(user, cancellationToken);
    public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken) => await this.xIncrementAccessFailedCountAsync(user, cancellationToken);
    public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken) => await this.xResetAccessFailedCountAsync(user, cancellationToken);
    public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken) => await this.xSetLockoutEnabledAsync(user, enabled, cancellationToken);
    public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken) => await this.xSetLockoutEndDateAsync(user, lockoutEnd, cancellationToken);
    #endregion

    #region IIdentityUserWithPasswordStore
    //IUserPasswordStore
    public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken) => await user.xGetPasswordHashAsync(cancellationToken);
    public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken) => await user.xHasPasswordAsync(cancellationToken);
    public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken) => await user.xSetPasswordHashAsync(passwordHash, cancellationToken);
    #endregion

    #region IIdentityUserWithPhoneNumberStore
    //IUserPhoneNumberStore
    public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken) => await this.xGetPhoneNumberAsync(user, cancellationToken);
    public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken) => await this.xGetPhoneNumberConfirmedAsync(user, cancellationToken);
    public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken) => await this.xSetPhoneNumberAsync(user, phoneNumber, cancellationToken);
    public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken) => await this.xSetPhoneNumberConfirmedAsync(user, confirmed, cancellationToken);
    #endregion

    #region IIdentityUserWithSecurityStampStore
    //IUserSecurityStampStore
    public async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken) => await this.xGetSecurityStampAsync(user, cancellationToken);
    public async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken) => await this.xSetSecurityStampAsync(user, stamp, cancellationToken);
    #endregion

    #region IIdentityUserWithTwoFactorStore
    //IUserTwoFactorStore
    public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken) => await this.xGetTwoFactorEnabledAsync(user, cancellationToken);
    public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken) => await this.xSetTwoFactorEnabledAsync(user, enabled, cancellationToken);
    #endregion

  }
}
