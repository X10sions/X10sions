using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Raven {

  public class _BaseIdentityUserStore_RavenDb<TUser, TKey> : _BaseIdentityUserStore<TUser, TKey>
    where TUser : class, IIdentityUser<TKey>, IIdentityUserWithEmail<TKey>, IIdentityUserWithPassword<TKey>
    where TKey : IEquatable<TKey> {

    // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-Odbc/

    public _BaseIdentityUserStore_RavenDb(IDocumentStore context, Expression<Func<TUser, bool>> baseSqlWhere, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      Context = context ?? throw new ArgumentNullException(nameof(context));
      _session = new Lazy<IAsyncDocumentSession>(() => {
        var session = Context.OpenAsyncSession();
        session.Advanced.UseOptimisticConcurrency = true;
        return session;
      }, true);
      BaseSqlWhere = baseSqlWhere;
    }

    public IDocumentStore Context { get; }
    public Expression<Func<TUser, bool>> BaseSqlWhere { get; }
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

    #region IQueryableUserStore
    public override IQueryable<TUser> Users => Session.Query<TUser>().Where(BaseSqlWhere);
    #endregion

    #region IUserStore

    protected override async Task<bool> CreateAsync_Insert(TUser user, CancellationToken cancellationToken = default) {
      await Session.StoreAsync(user, cancellationToken);
      return await SaveChanges(cancellationToken);
    }

    protected override async Task<bool> DeleteAsync_Delete(TUser user, CancellationToken cancellationToken = default) {
      Session.Delete(user.Id);
      return await SaveChanges(cancellationToken);
    }

    protected override async Task<bool> UpdateAsync_Update(TUser user, CancellationToken cancellationToken = default) {
      var stored = await Session.LoadAsync<TUser>(user.Id.ToString(), cancellationToken);
      var etag = Session.Advanced.GetChangeVectorFor(stored);
      await Session.StoreAsync(user, etag, cancellationToken);
      return await SaveChanges(cancellationToken);
    }

    #endregion

  }
}
