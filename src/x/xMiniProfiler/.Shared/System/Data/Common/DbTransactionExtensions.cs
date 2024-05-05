using StackExchange.Profiling.Data;

namespace System.Data.Common;
  public static class DbTransactionExtensions {
    public static DbTransaction UnwrapTransaction(this DbTransaction transaction) => transaction is ProfiledDbTransaction t ? t.WrappedTransaction : transaction;
  }