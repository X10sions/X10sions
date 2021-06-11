using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity {

  public abstract class _BaseIdentityStore : IIdentityStore {
    public _BaseIdentityStore(IdentityErrorDescriber errorDescriber) {
      ErrorDescriber = errorDescriber;
    }
    public IdentityErrorDescriber ErrorDescriber { get; }

    #region IDisposable Support
    protected bool IsDisposed { get; private set; }
    protected virtual void Dispose(bool disposing) {
      if (!IsDisposed) {
        if (disposing) {
          // TODO: dispose managed state (managed objects).
        }
        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.
        IsDisposed = true;
      }
    }

    ~_BaseIdentityStore() {
      Dispose(false);
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion

  }

  public abstract class _BaseIdentityStore<T, TKey> : _BaseIdentityStore
    where T : IId<TKey>
    where TKey : IEquatable<TKey> {
    public _BaseIdentityStore(IdentityErrorDescriber errorDescriber) : base(errorDescriber) { }

    public abstract Func<T, string> GetRoleOrUserDescription { get; }
    protected abstract Task<bool> CreateAsync_Insert(T roleOrUser, CancellationToken cancellationToken = default);
    protected abstract Task<bool> DeleteAsync_Delete(T roleOrUser, CancellationToken cancellationToken = default);
    protected abstract Task<T> FindByIdAsync_Select(string roleOrUserId, CancellationToken cancellationToken = default);
    protected abstract Task<T> FindByNameAsync_Select(string roleOrUserNormalizedName, CancellationToken cancellationToken = default);
    protected abstract Task<T> FindByIdAndConcurrencyStampAsync(T roleOrUser, CancellationToken cancellationToken = default);
    protected abstract void UpdateAsync_SetConcurrencyStamp(T roleOrUser);
    protected abstract Task<bool> UpdateAsync_Update(T roleOrUser, CancellationToken cancellationToken = default);

    string parameterName => typeof(IIdentityUser<>).IsAssignableFrom(typeof(T)) ? "role" : "user";
    protected virtual T DefaultRoleOrUser => default;


    #region IDisposable Support
    protected bool IsDisposed { get; private set; }
    protected virtual void Dispose(bool disposing) {
      if (!IsDisposed) {
        if (disposing) {
          // TODO: dispose managed state (managed objects).
        }
        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.
        IsDisposed = true;
      }
    }

    ~_BaseIdentityStore() {
      Dispose(false);
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion

    #region IRoleStore, IUserStore

    public async Task<IdentityResult> CreateAsync(T roleOrUser, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(roleOrUser, parameterName);
      return await CreateAsync_Insert(roleOrUser, cancellationToken) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = $"Could not insert {parameterName} {GetRoleOrUserDescription(roleOrUser)}." });
    }

    public async Task<IdentityResult> DeleteAsync(T roleOrUser, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(roleOrUser, parameterName);
      return await DeleteAsync_Delete(roleOrUser, cancellationToken) ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = $"Could not delete {parameterName} {GetRoleOrUserDescription(roleOrUser)}." });
    }

    public async Task<T> FindByIdAsync(string roleorUserId, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(roleorUserId, nameof(roleorUserId));
      return await FindByIdAsync_Select(roleorUserId, cancellationToken);
    }

    public async Task<T> FindByNameAsync(string roleOrUserNormalizedName, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(roleOrUserNormalizedName, nameof(roleOrUserNormalizedName));
      return await FindByNameAsync_Select(roleOrUserNormalizedName, cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(T roleorUser, CancellationToken cancellationToken = default) {
      cancellationToken.ThrowIfCancellationRequestedOrNull(roleorUser, parameterName);
      var dbUser = await FindByIdAndConcurrencyStampAsync(roleorUser, cancellationToken);
      if (dbUser == null) {
        return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
      }
      UpdateAsync_SetConcurrencyStamp(dbUser);
      await UpdateAsync_Update(dbUser, cancellationToken);
      return IdentityResult.Success;
    }

    #endregion

  }
}