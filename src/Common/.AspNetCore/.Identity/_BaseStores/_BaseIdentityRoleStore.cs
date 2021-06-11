using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {
  public abstract class _BaseIdentityRoleStore<TRole, TKey> : _BaseIdentityStore<TRole, TKey>
    , IIdentityRoleStore<TRole, TKey>
    where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStore(IdentityErrorDescriber errorDescriber) : base(errorDescriber) { }

    public override Func<TRole, string> GetRoleOrUserDescription => x => x.Id.ToString();

    #region IQueryableRoleStore
    public abstract IQueryable<TRole> Roles { get; }
    #endregion

    #region IRoleStore
    protected override async Task<TRole> FindByIdAsync_Select(string roleId, CancellationToken cancellationToken = default) => (TRole)await Roles.xFindByIdAsync(roleId, cancellationToken);
    protected override async Task<TRole> FindByIdAndConcurrencyStampAsync(TRole role, CancellationToken cancellationToken = default) => (TRole)await Roles.FindByIdAndConcurrencyStampAsync(role, cancellationToken);
    protected override async Task<TRole> FindByNameAsync_Select(string normalizedRoleName, CancellationToken cancellationToken = default) => (TRole)await Roles.xFindByNameAsync(normalizedRoleName, cancellationToken);

    public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetNormalizedRoleNameAsync(cancellationToken);
    public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetRoleIdAsync(cancellationToken);
    public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default) => await role.xGetRoleNameAsync(cancellationToken);

    public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default) => await role.xSetNormalizedRoleNameAsync(normalizedName, cancellationToken);
    public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default) => await role.xSetRoleNameAsync(roleName, cancellationToken);

    protected override void UpdateAsync_SetConcurrencyStamp(TRole role) => role.ConcurrencyStamp = Guid.NewGuid().ToString();

    #endregion

  }


}