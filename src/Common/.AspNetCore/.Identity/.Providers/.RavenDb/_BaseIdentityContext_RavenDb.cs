using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public class _BaseIdentityContext_RavenDb : DocumentStore, IIdentityContext_RavenDb {

    #region IDisposable
    public override void Dispose() {
      base.Dispose();
      Session.Dispose();
    }
    #endregion

    public IAsyncDocumentSession Session => this.OpenAsyncSessionWithOptimisticConcurrency().Value;

    public async Task DbDeleteAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbDeleteAsync(data, cancellationToken);
    public async Task DbInsertAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbInsertAsync(data, cancellationToken);
    public async Task DbUpdateAsync<T>(T data, CancellationToken cancellationToken = default) where T : class => await this.xDbUpdateAsync(data, cancellationToken);
    public IQueryable<T> DbGetQueryable<T>() where T : class => this.xDbGetQueryable<T>();

    //public async Task DbSaveChangesAsync(CancellationToken cancellationToken = default) => await Session.SaveChangesAsync(cancellationToken);
  }

}
