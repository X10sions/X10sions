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

    public async static Task<Exception?> TryCommitAsync(this DataContextTransaction transaction, CancellationToken cancellationToken = default) {
      try {
        await  transaction.CommitTransactionAsync(cancellationToken);
        return null;
      } catch (Exception ex) {
        await transaction.RollbackTransactionAsync(cancellationToken);
        return ex;
      }
    }

  }
}
