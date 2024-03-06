using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400ExecutionStrategy : IExecutionStrategy {
     ExecutionStrategyDependencies Dependencies { get; }

    public AS400ExecutionStrategy([NotNull] ExecutionStrategyDependencies dependencies) {
      Dependencies = dependencies;
    }

    public virtual bool RetriesOnFailure => false;

    public virtual TResult Execute<TState, TResult>(
        TState state,
        Func<DbContext, TState, TResult> operation,
        Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded) {
      try {
        return operation(Dependencies.CurrentDbContext.Context, state);
      } catch (Exception ex) {
        if (ExecutionStrategy.CallOnWrappedException(ex, AS400TransientExceptionDetector.ShouldRetryOn)) {
          throw new InvalidOperationException(EFCoreStrings.TransientExceptionDetected, ex);
        }
        throw;
      }
    }

    public virtual async Task<TResult> ExecuteAsync<TState, TResult>(
        TState state,
        Func<DbContext, TState, CancellationToken, Task<TResult>> operation,
        Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded,
        CancellationToken cancellationToken) {
      try {
        return await operation(Dependencies.CurrentDbContext.Context, state, cancellationToken);
      } catch (Exception ex) {
        if (ExecutionStrategy.CallOnWrappedException(ex, AS400TransientExceptionDetector.ShouldRetryOn)) {
          throw new InvalidOperationException(EFCoreStrings.TransientExceptionDetected, ex);
        }
        throw;
      }
    }

  }
}
