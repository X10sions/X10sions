using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public interface IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin>
    : IUserLoginStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithLogins<TKey>
    where TUserLogin : class, IIdentityUserLogin<TKey> {
    //IQueryable<TUserLogin> UserLogins { get; set; }
    //IIdentityDatabaseTable<TUserLogin> UserLoginsSet { get; set; }
  }

  public static class IIdentityUserLoginStoreWithContextExtensions {

    #region UserStore: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/UserStore.cs

    public static async Task xAddLoginAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>, new()
      where TUser : class, IIdentityUserWithLogins<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      if (login == null) {
        throw new ArgumentNullException(nameof(login));
      }
      await store.Context.DbInsertAsync(user.xCreateUserLogin<TKey, TUserLogin>(login));
      await Task.FromResult(false);
    }

    public static async Task xRemoveLoginAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      where TUser : class, IIdentityUserWithLogins<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      var entry = await store.xFindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
      if (entry != null) {
        await store.Context.DbDeleteAsync(entry);
      }
    }

    #endregion

    public static async Task<TUser> xFindByLoginAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      where TUser : class, IIdentityUserWithLogins<TKey> {
      cancellationToken.ThrowIfCancellationRequested();
      var userLogin = await store.xFindUserLoginAsync(loginProvider, providerKey, cancellationToken);
      if (userLogin != null) {
        return (TUser)await store.Context.DbGetQueryable<TUser>().xFindByIdAsync(userLogin.UserId, cancellationToken);
      }
      return null;
    }

    public static async Task<TUserLogin> xFindUserLoginAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      where TUser : class, IIdentityUserWithLogins<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return await store.Context.DbGetQueryable<TUserLogin>().xSingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
    }

    public static async Task<TUserLogin> xFindUserLoginAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, string loginProvider, string providerKey, CancellationToken cancellationToken)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      where TUser : class, IIdentityUserWithLogins<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return await store.Context.DbGetQueryable<TUserLogin>().xSingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
    }

    public static async Task<IList<UserLoginInfo>> xGetLoginsAsync<TContext, TUser, TKey, TUserLogin>(this IIdentityUserLoginStoreWithContext<TContext, TUser, TKey, TUserLogin> store, TUser user, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserLogins<TKey>
      where TKey : IEquatable<TKey>
      where TUserLogin : class, IIdentityUserLogin<TKey>
      where TUser : class, IIdentityUserWithLogins<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrUserNull(user, cancellationToken);
      return await (from l in store.Context.DbGetQueryable<TUserLogin>() where l.UserId.Equals(user.Id) select new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).xToListAsync(cancellationToken);
    }



  }
}