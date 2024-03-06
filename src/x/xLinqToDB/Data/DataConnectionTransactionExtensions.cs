namespace LinqToDB.Data {

  public static class DataConnectionTransactionExtensions {

    public static Exception? TryCommit(this DataConnectionTransaction transaction) {
      try {
        transaction.Commit();
        return null;
      } catch (Exception ex) {
        transaction.Rollback();
        return ex;
      }
    }

    public static Task<Exception?> TryCommitAsync(this DataConnectionTransaction transaction, CancellationToken cancellationToken = default) {
      try {
        transaction.CommitAsync(cancellationToken);
        return Task.FromResult<Exception?>(null);
      } catch (Exception ex) {
        transaction.RollbackAsync(cancellationToken);
        return Task.FromResult<Exception?>(ex);
      }
    }

  }
}