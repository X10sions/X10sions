using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityRoleStoreWithContext<TContext, TRole, TKey>
    : _BaseIdentityStoreWithContext<TContext>, IIdentityRoleStoreWithContext<TContext, TRole, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUserAndRoles<TRole, TKey, TUserRole>
    where TKey : IEquatable<TKey>
    //where TUserRole : class, IIdentityUserRole<TKey>
    where TRole : class, IIdentityRoleWithConcurrency<TKey> {

    public _BaseIdentityRoleStoreWithContext(TContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber) { }

    public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default) => await this.xCreateAsync(role, cancellationToken);
    public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default) => await this.xDeleteAsync(role, cancellationToken);
    public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default) => await this.xUpdateAsync(role, cancellationToken);

    public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken = default) => await this.xFindByIdAsync(roleId, cancellationToken);
    public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default) => await this.xFindByNameAsync(normalizedRoleName, cancellationToken);

    public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetNormalizedRoleNameAsync(cancellationToken);
    public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetRoleIdAsync(cancellationToken);
    public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetRoleNameAsync(cancellationToken);
    public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default) => await role.xSetNormalizedRoleNameAsync(normalizedName, cancellationToken);
    public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default) => await role.xSetRoleNameAsync(roleName, cancellationToken);
    #region IQueryableRoleStore
    public IQueryable<TRole> Roles => Context.DbGetQueryable<TRole>();
    #endregion
  }
}