﻿namespace System.Data.Common;
public static class DbTransactionExtensions {
  public static Task CommitAsync(this DbTransaction trans) => trans.CommitAsync();

  public static async Task CommitAsync(this DbTransaction trans, CancellationToken cancellationToken) {
    if (trans.Connection == null) {
      throw new InvalidOperationException("Already committed or rolled back.");
    }
    if (DbConnectionExtensions.CurrentTransactionAsync == trans) {
      using (var cmd = trans.Connection.CreateCommand()) {
        cmd.CommandText = "commit";
        cmd.Transaction = trans;
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        DbConnectionExtensions.CurrentTransactionAsync = null;
      }
    } else if (DbConnectionExtensions.CurrentTransactionAsync != null) {
      throw new InvalidOperationException("This is not the active transaction.");
    } else if (DbConnectionExtensions.CurrentTransactionAsync == null) {
      throw new InvalidOperationException("There is no active transaction.");
    }
  }

  public static Task RollbackAsync(this DbTransaction trans) => trans.RollbackAsync();

  public static async Task RollbackAsync(this DbTransaction trans, CancellationToken cancellationToken) {
    if (trans.Connection == null) {
      throw new InvalidOperationException("Already committed or rolled back.");
    }
    if (DbConnectionExtensions.CurrentTransactionAsync == trans) {
      using (var cmd = trans.Connection.CreateCommand()) {
        cmd.CommandText = "rollback";
        cmd.Transaction = trans;
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        DbConnectionExtensions.CurrentTransactionAsync = null;
      }
    } else if (DbConnectionExtensions.CurrentTransactionAsync != null) {
      throw new InvalidOperationException("This is not the active transaction.");
    } else if (DbConnectionExtensions.CurrentTransactionAsync == null) {
      throw new InvalidOperationException("There is no active transaction.");
    }
  }

}