using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public class _BaseIdentityRoleStore_RavenDb<TRole, TKey> : _BaseIdentityRoleStore<TRole, TKey>
      where TRole : class, IIdentityRole<TKey>, IIdentityRoleWithConcurrency<TKey>
      where TKey : IEquatable<TKey> {


    // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-Odbc/

    public _BaseIdentityRoleStore_RavenDb(IDocumentStore context, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      Context = context ?? throw new ArgumentNullException(nameof(context));
      _session = new Lazy<IAsyncDocumentSession>(() => {
        var session = Context.OpenAsyncSession();
        session.Advanced.UseOptimisticConcurrency = true;
        return session;
      }, true);
    }

    public IDocumentStore Context { get; }
    private readonly Lazy<IAsyncDocumentSession> _session;
    public IAsyncDocumentSession Session => _session.Value;

    public async Task<bool> SaveChanges(CancellationToken cancellationToken = default) {
      try {
        await Session.SaveChangesAsync(cancellationToken);
        return await Task.FromResult(true);
      } catch {
        return await Task.FromResult(false);
      }
    }

    #region IDisposable
    protected override void Dispose(bool disposing) {
      if (!IsDisposed && disposing) {
        Session.Dispose();
      }
      base.Dispose(disposing);
    }
    #endregion

    #region IQueryableRoleStore
    public override IQueryable<TRole> Roles => Session.Query<TRole>();
    #endregion

    #region IRoleStore

    protected override async Task<bool> CreateAsync_Insert(TRole role, CancellationToken cancellationToken = default) {
      await Session.StoreAsync(role, cancellationToken);
      return await SaveChanges(cancellationToken);
    }

    protected override async Task<bool> DeleteAsync_Delete(TRole role, CancellationToken cancellationToken = default) {
      Session.Delete(role.Id);
      return await SaveChanges(cancellationToken);
    }

    protected override async Task<bool> UpdateAsync_Update(TRole role, CancellationToken cancellationToken = default) {
      var stored = await Session.LoadAsync<TRole>(role.Id.ToString(), cancellationToken);
      var etag = Session.Advanced.GetChangeVectorFor(stored);
      await Session.StoreAsync(role, etag, cancellationToken);
      return await SaveChanges(cancellationToken);
    }

    #endregion


  }
}
