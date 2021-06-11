using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.AspNetCore.Identity.Providers.Raven {

  public interface IIdentityContext_RavenDb : IIdentityContext, IDocumentStore {
    //IDocumentStore DocumentStore { get; }
    IAsyncDocumentSession Session { get; }
  }

  public static class IRavenDbIdentityContextExtensions {

    public static async Task xDbDeleteAsync<T>(this IIdentityContext_RavenDb context, T data, CancellationToken cancellationToken = default) {
      context.Session.Delete(data);
      await context.Session.SaveChangesAsync(cancellationToken);
    }

    public static IQueryable<T> xDbGetQueryable<T>(this IIdentityContext_RavenDb context) where T : class => context.Session.Query<T>();

    public static async Task xDbInsertAsync<T>(this IIdentityContext_RavenDb context, T data, CancellationToken cancellationToken = default) {
      await context.Session.StoreAsync(data, cancellationToken);
      await context.Session.SaveChangesAsync(cancellationToken);
    }

    public static async Task xDbUpdateAsync<T>(this IIdentityContext_RavenDb context, T data, CancellationToken cancellationToken = default) {
      //var changeVector = context.Advanced.GetChangeVectorFor(data);
      //await context.StoreAsync(data , changeVector , cancellationToken);

      await context.Session.StoreAsync(data, cancellationToken);
      await context.Session.SaveChangesAsync(cancellationToken);
    }
  }

}
