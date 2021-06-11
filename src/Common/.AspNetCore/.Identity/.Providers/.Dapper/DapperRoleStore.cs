using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Dapper {
  public class DapperRoleStore<TKey> :
    IRoleStore<IIdentityRole<TKey>>
    where TKey : IEquatable<TKey> {
    public Task<IdentityResult> CreateAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IdentityResult> DeleteAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();

    public void Dispose() { }

    public Task<IIdentityRole<TKey>> FindByIdAsync(string roleId, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IIdentityRole<TKey>> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<string> GetNormalizedRoleNameAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<string> GetRoleIdAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<string> GetRoleNameAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task SetNormalizedRoleNameAsync(IIdentityRole<TKey> role, string normalizedName, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task SetRoleNameAsync(IIdentityRole<TKey> role, string roleName, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IdentityResult> UpdateAsync(IIdentityRole<TKey> role, CancellationToken cancellationToken) => throw new NotImplementedException();
  }
}
