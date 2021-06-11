namespace System.Data.Linq {
  internal sealed class ChangeConflictSession {
    private DataContext refreshContext;
    internal DataContext Context { get; }
    internal DataContext RefreshContext {
      get {
        if (refreshContext == null) {
          refreshContext = Context.CreateRefreshContext();
        }
        return refreshContext;
      }
    }

    internal ChangeConflictSession(DataContext context) {
      Context = context;
    }
  }

}