using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class LinqToDbRoleStore<TKey> :
    IQueryableRoleStore<IdentityRoleNav<TKey>>,
    IRoleClaimStore<IdentityRoleNav<TKey>>
    where TKey : IEquatable<TKey> {
    public LinqToDbRoleStore(IIdentityDataConnection<TKey> dataConnection, IdentityErrorDescriber describer = null) {
      DataConnection = dataConnection ?? throw new ArgumentNullException(nameof(dataConnection));
      ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }

    private bool _disposed;

    public IIdentityDataConnection<TKey> DataConnection { get; private set; }

    public IdentityErrorDescriber ErrorDescriber { get; set; }

    public virtual async Task<IdentityResult> CreateAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      await DataConnection.InsertAsync(role);
      return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> UpdateAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      role.ConcurrencyStamp = Guid.NewGuid().ToString();
      await DataConnection.UpdateAsync(role);
      return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> DeleteAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      await DataConnection.DeleteAsync(role);
      return IdentityResult.Success;
    }

    public virtual Task<string?> GetRoleIdAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      return Task.FromResult(role.Id.ConvertIdToString());
    }

    public virtual Task<string> GetRoleNameAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      return Task.FromResult(role.Name);
    }

    public virtual Task SetRoleNameAsync(IdentityRoleNav<TKey> role, string roleName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      role.Name = roleName;
      return Task.CompletedTask;
    }

    public virtual Task<IdentityRoleNav<TKey>> FindByIdAsync(string id, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      var roleId = id.ConvertIdFromString<TKey>();
      return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
    }

    public virtual Task<IdentityRoleNav<TKey>> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
    }

    public virtual Task<string> GetNormalizedRoleNameAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      return Task.FromResult(role.NormalizedName);
    }

    public virtual Task SetNormalizedRoleNameAsync(IdentityRoleNav<TKey> role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken)) {
      cancellationToken.ThrowIfCancellationRequested();
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      role.NormalizedName = normalizedName;
      return Task.CompletedTask;
    }

    protected void ThrowIfDisposed() {
      if (_disposed) {
        throw new ObjectDisposedException(GetType().Name);
      }
    }

    public void Dispose() => _disposed = true;

    public virtual async Task<IList<Claim>> GetClaimsAsync(IdentityRoleNav<TKey> role, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
    }

    public virtual Task AddClaimAsync(IdentityRoleNav<TKey> role, Claim claim, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      RoleClaims.DataContext.Insert(CreateRoleClaim(role, claim));
      return Task.FromResult(false);
    }

    public virtual async Task RemoveClaimAsync(IdentityRoleNav<TKey> role, Claim claim, CancellationToken cancellationToken = default) {
      ThrowIfDisposed();
      if (role == null) {
        throw new ArgumentNullException(nameof(role));
      }
      if (claim == null) {
        throw new ArgumentNullException(nameof(claim));
      }
      var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
      foreach (var c in claims) {
        RoleClaims.DataContext.Delete(c);
      }
    }

    public virtual IQueryable<IdentityRoleNav<TKey>> Roles => DataConnection.GetTable<IdentityRoleNav<TKey>>();
    private ITable<IdentityRoleClaimNav<TKey>> RoleClaims => DataConnection.GetTable<IdentityRoleClaimNav<TKey>>();

    protected virtual IdentityRoleClaim<TKey> CreateRoleClaim(IdentityRoleNav<TKey> role, Claim claim) => new IdentityRoleClaimNav<TKey> {
      RoleId = role.Id,
      ClaimType = claim.Type,
      ClaimValue = claim.Value
    }; 

  }
}