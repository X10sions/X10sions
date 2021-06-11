using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleStoreWithContext<TContext, TRole, TKey>
    : IQueryableRoleStore<TRole>
    , IIdentityStoreWithContext<TContext>//<IIdentityContext_WithUserAndRoles<TKey>>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
    where TKey : IEquatable<TKey>
    where TRole : class, IIdentityRoleWithConcurrency<TKey> {
    //IQueryable<TRole> RoleQueryable { get; }
    //IIdentityDatabaseTable<TRole> RoleDatabaseTable{ get; set; }
  }

  public static class IIdentityRoleStoreWithContextExtensions {

    #region RoleStore: https://github.com/aspnet/AspNetCore/blob/release/2.2/src/Identity/EntityFrameworkCore/src/RoleStore.cs

    public static async Task<IdentityResult> xCreateAsync<TContext, TRole, TKey>(this IIdentityRoleStoreWithContext<TContext, TRole, TKey> store, TRole role, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      await store.Context.DbInsertAsync(role, cancellationToken);
      return IdentityResult.Success;
    }

    public static async Task<IdentityResult> xDeleteAsync<TContext, TRole, TKey>(this IIdentityRoleStoreWithContext<TContext, TRole, TKey> store, TRole role, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      var stored = await store.Context.DbGetQueryable<TRole>().FindByIdAndConcurrencyStampAsync(role, cancellationToken);
      if (stored == null) {
        return IdentityResult.Failed(store.ErrorDescriber.ConcurrencyFailure());
      }
      await store.Context.DbDeleteAsync(role, cancellationToken);
      return IdentityResult.Success;
    }

    public static async Task<TRole> xFindByIdAsync<TContext, TRole, TKey>(this IIdentityRoleStoreWithContext<TContext, TRole, TKey> store, string id, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return (TRole)await store.Context.DbGetQueryable<TRole>().xFindByIdAsync(id, cancellationToken);
    }

    public static async Task<TRole> xFindByNameAsync<TContext, TRole, TKey>(this IIdentityRoleStoreWithContext<TContext, TRole, TKey> store, string normalizedName, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposed(cancellationToken);
      return (TRole)await store.Context.DbGetQueryable<TRole>().xFindByNameAsync(normalizedName, cancellationToken);
    }

    public static async Task<IdentityResult> xUpdateAsync<TContext, TRole, TKey>(this IIdentityRoleStoreWithContext<TContext, TRole, TKey> store, TRole role, CancellationToken cancellationToken = default)
      where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TKey>
      where TKey : IEquatable<TKey>
      where TRole : class, IIdentityRoleWithConcurrency<TKey> {
      store.ThrowIfCancelledRequestOrDisposedOrRoleNull(role, cancellationToken);
      var stored = await store.Context.DbGetQueryable<TRole>().FindByIdAndConcurrencyStampAsync(role, cancellationToken);
      if (stored == null) {
        return IdentityResult.Failed(store.ErrorDescriber.ConcurrencyFailure());
      }
      role.ConcurrencyStamp = Guid.NewGuid().ToString();
      await store.Context.DbUpdateAsync(role, cancellationToken);
      return IdentityResult.Success;
    }

    #endregion

  }
}