namespace Common.AspNetCore.Identity {
  public interface IIdentityStoreWithContext<TContext> : IIdentityStore, IDisposableDisposed
    where TContext : class, IIdentityContext {
    //bool AutoSaveChanges { get; set; }
    TContext Context { get; }
  }

  public static class IIdentityStoreWithContextExtensions {
    //public static Task SaveChanges(this IIdentityStoreWithContext store, CancellationToken cancellationToken)
    //  => store.AutoSaveChanges ? store.Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
  }

}