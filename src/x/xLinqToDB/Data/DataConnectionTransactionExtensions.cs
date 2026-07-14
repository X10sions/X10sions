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

    public static async Task<Exception?> TryCommitAsync(this DataConnectionTransaction transaction, CancellationToken cancellationToken = default) {
      try {
        await transaction.CommitAsync(cancellationToken);
        return null;
      } catch (Exception ex) {
        await transaction.RollbackAsync(cancellationToken);
        return ex;
      }
    }

  }
}