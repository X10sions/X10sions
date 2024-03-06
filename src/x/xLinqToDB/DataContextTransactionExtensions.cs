namespace LinqToDB {
  public static class DataContextTransactionExtensions {

    public static Exception? TryCommit(this DataContextTransaction transaction) {
      try {
        transaction.CommitTransaction();
        return null;
      } catch (Exception ex) {
        transaction.RollbackTransaction();
        return ex;
      }
    }

    public static Task<Exception?> TryCommitAsync(this DataContextTransaction transaction, CancellationToken cancellationToken = default) {
      try {
        transaction.CommitTransactionAsync(cancellationToken);
        return Task.FromResult<Exception?>(null);
      } catch (Exception ex) {
        transaction.RollbackTransactionAsync(cancellationToken);
        return Task.FromResult<Exception?>(ex);
      }
    }

  }
}
