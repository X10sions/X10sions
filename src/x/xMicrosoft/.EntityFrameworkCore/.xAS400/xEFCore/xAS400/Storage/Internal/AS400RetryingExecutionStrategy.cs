using IBM.Data.DB2.iSeries; // System.Data.SqlClient;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400RetryingExecutionStrategy : ExecutionStrategy {

    readonly ICollection<int> _additionalErrorNumbers;

    public AS400RetryingExecutionStrategy(
        [NotNull] DbContext context)
        : this(context, DefaultMaxRetryCount) {
    }

    public AS400RetryingExecutionStrategy(
        [NotNull] ExecutionStrategyDependencies dependencies)
        : this(dependencies, DefaultMaxRetryCount) {
    }

    public AS400RetryingExecutionStrategy([NotNull] DbContext context, int maxRetryCount)
    : this(context, maxRetryCount, DefaultMaxDelay, errorNumbersToAdd: null) {
    }

    public AS400RetryingExecutionStrategy(
        [NotNull] ExecutionStrategyDependencies dependencies,
        int maxRetryCount)
        : this(dependencies, maxRetryCount, DefaultMaxDelay, errorNumbersToAdd: null) {
    }

    public AS400RetryingExecutionStrategy(
        [NotNull] DbContext context,
        int maxRetryCount,
        TimeSpan maxRetryDelay,
        [CanBeNull] ICollection<int> errorNumbersToAdd)
        : base(context,
            maxRetryCount,
            maxRetryDelay) {
      _additionalErrorNumbers = errorNumbersToAdd;
    }

    public AS400RetryingExecutionStrategy(
        [NotNull] ExecutionStrategyDependencies dependencies,
        int maxRetryCount,
        TimeSpan maxRetryDelay,
        [CanBeNull] ICollection<int> errorNumbersToAdd)
        : base(dependencies, maxRetryCount, maxRetryDelay) {
      _additionalErrorNumbers = errorNumbersToAdd;
    }

    protected override bool ShouldRetryOn(Exception exception) {
      if (_additionalErrorNumbers != null) {
        var sqlException = exception as iDB2Exception;
        if (sqlException != null) {
          foreach (iDB2Error err in sqlException.Errors) {
            if (_additionalErrorNumbers.Contains(err.MessageCode)) {
              return true;
            }
          }
        }
      }
      return AS400TransientExceptionDetector.ShouldRetryOn(exception);
    }

    protected override TimeSpan? GetNextDelay(Exception lastException) {
      var baseDelay = base.GetNextDelay(lastException);
      if (baseDelay == null) {
        return null;
      }
      if (CallOnWrappedException(lastException, IsMemoryOptimizedError)) {
        return TimeSpan.FromMilliseconds(baseDelay.Value.TotalSeconds);
      }
      return baseDelay;
    }

    bool IsMemoryOptimizedError(Exception exception) {
      var storeException = exception as iDB2Exception;
      if (storeException != null) {
        foreach (iDB2Error err in storeException.Errors) {
          //TODO: Determine IsMemoryOptimizedError iDB2Exception Error MessageCodes
          throw new Exception($"{err.MessageCode}: {err.Message}");
          switch (err.MessageCode) {
            //case 41301: //Dependency failure: a dependency was taken on another transaction that later failed to commit.
            //case 41302: //Attempted to update a row that was updated in a different transaction since the start of the present transaction.
            //case 41305: //Repeatable read validation failure. A row read from a memory-optimized table this transaction has been updated by another transaction that has committed before the commit of this transaction.
            //case 41325: //Serializable validation failure. A new row was inserted into a range that was scanned earlier by the present transaction. We call this a phantom row.
            //case 41839: //Transaction exceeded the maximum number of commit dependencies.
            //  return true;
          }
        }
      }
      return false;
    }

  }
}