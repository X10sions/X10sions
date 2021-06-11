using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public interface IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken>
    : IUserAuthenticationTokenStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithTokens<TKey>
    where TUserToken : class, IIdentityUserToken<TKey> {
    //IQueryable<TUserToken> UserTokenQueryable { get; set; }
    //IIdentityDatabaseTable<TUserToken> UserTokensDatabaseTable { get; set; }
  }

  public static class IIdentityUserAuthenticationTokenStoreWithContextExtensions {

    #region https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/UserStore.cs

    public static async Task xAddUserTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUserToken token)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey> {
      await store.Context.DbInsertAsync(token);
      await Task.CompletedTask;
    }

    public static async Task<TUserToken> xFindTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string loginProvider, string name, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>
      => (TUserToken)await store.Context.DbGetQueryable<TUserToken>().xFindAsync(user, loginProvider, name, cancellationToken);

    public static async Task xRemoveUserTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUserToken token)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey> {
      await store.Context.DbDeleteAsync(token);
      await Task.CompletedTask;
    }

    #endregion

    #region UserStoreBase: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/Extensions.Stores/src/UserStoreBase.cs

    public static async Task<int> xCountCodesAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var mergedCodes = await store.xGetTokenAsync(user, IdentityConstants.InternalLoginProvider, IdentityConstants.RecoveryCodeTokenName, cancellationToken) ?? "";
      if (mergedCodes.Length > 0) {
        return mergedCodes.Split(';').Length;
      }
      return 0;
    }

    public static async Task<string> xGetAuthenticatorKeyAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>
      => await store.xGetTokenAsync(user, IdentityConstants.InternalLoginProvider, IdentityConstants.AuthenticatorKeyTokenName, cancellationToken);

    public static async Task<string> xGetTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string loginProvider, string name, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var entry = await store.xFindTokenAsync(user, loginProvider, name, cancellationToken);
      return entry?.Value;
    }

    public static async Task<bool> xRedeemCodeAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string code, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>, new() {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (code == null) {
        throw new ArgumentNullException(nameof(code));
      }
      var mergedCodes = await store.xGetTokenAsync(user, IdentityConstants.InternalLoginProvider, IdentityConstants.RecoveryCodeTokenName, cancellationToken) ?? "";
      var splitCodes = mergedCodes.Split(';');
      if (splitCodes.Contains(code)) {
        var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
        await store.xReplaceCodesAsync(user, updatedCodes, cancellationToken);
        return true;
      }
      return false;
    }

    public static async Task xRemoveTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string loginProvider, string name, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var entry = await store.xFindTokenAsync(user, loginProvider, name, cancellationToken);
      if (entry != null) {
        await store.xRemoveUserTokenAsync(entry);
      }
    }

    public static async Task xReplaceCodesAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>, new() {
      var mergedCodes = string.Join(";", recoveryCodes);
      await store.xSetTokenAsync(user, IdentityConstants.InternalLoginProvider, IdentityConstants.RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    public static async Task xSetAuthenticatorKeyAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string key, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>, new()
      => await store.xSetTokenAsync(user, IdentityConstants.InternalLoginProvider, IdentityConstants.AuthenticatorKeyTokenName, key, cancellationToken);

    public static async Task xSetTokenAsync<TContext, TUser, TKey, TUserToken>(this IIdentityUserAuthenticationTokenStoreWithContext<TContext, TUser, TKey, TUserToken> store, TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserTokens<TKey>
      where TKey : IEquatable<TKey>
      where TUser : class, IIdentityUserWithTokens<TKey>
      where TUserToken : class, IIdentityUserToken<TKey>, new() {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var token = await store.xFindTokenAsync(user, loginProvider, name, cancellationToken);
      if (token == null) {
        await store.xAddUserTokenAsync(user.xCreateUserToken<TKey, TUserToken>(loginProvider, name, value));
      } else {
        token.Value = value;
      }
    }

    #endregion

  }

}