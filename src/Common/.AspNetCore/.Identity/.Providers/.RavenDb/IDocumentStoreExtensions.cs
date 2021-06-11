using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public static class IDocumentStoreExtensions {

    public static Lazy<IAsyncDocumentSession> OpenAsyncSessionWithOptimisticConcurrency(this IDocumentStore documentStore) => new Lazy<IAsyncDocumentSession>(() => {
      var session = documentStore.OpenAsyncSession();
      session.Advanced.UseOptimisticConcurrency = true;
      return session;
    }, true);

  }


}