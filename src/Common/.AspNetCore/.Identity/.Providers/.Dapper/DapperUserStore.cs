using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Dapper {

  public class DapperUserStore<TKey, TUser> : IUserStore<TUser>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUser<TKey> {
    protected readonly DapperUsersTable<TKey, TUser> _usersTable;

    public DapperUserStore(DapperUsersTable<TKey, TUser> usersTable) {
      _usersTable = usersTable;
    }

    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      return await _usersTable.CreateAsync(user);
    }

    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      return await _usersTable.DeleteAsync(user);
    }

    public void Dispose() { }

    public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      if (userId == null) throw new ArgumentNullException(nameof(userId));
      Guid idGuid;
      if (!Guid.TryParse(userId, out idGuid)) {
        throw new ArgumentException("Not a valid Guid id", nameof(userId));
      }
      return await _usersTable.FindByIdAsync(idGuid.ToString());
    }

    public async Task<TUser> FindByNameAsync(string userName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequested();
      if (userName == null) throw new ArgumentNullException(nameof(userName));
      return await _usersTable.FindByNameAsync(userName);
    }

    public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      return Task.FromResult(user.Name);
    }

    public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      user.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));
      return Task.FromResult<object>(null);
    }

    public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();

  }

  public class DapperUserWithEmailAndPasswordStore<TKey, TUser> : DapperUserStore<TKey, TUser>, IUserPasswordStore<TUser>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey> {

    public DapperUserWithEmailAndPasswordStore(DapperUsersTable<TKey, TUser> usersTable) : base(usersTable) { }

    public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken) {
      cancellationToken.ThrowIfCancellationRequested();
      if (user == null) throw new ArgumentNullException(nameof(user));
      user.PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
      return Task.FromResult<object>(null);
    }

  }
}
