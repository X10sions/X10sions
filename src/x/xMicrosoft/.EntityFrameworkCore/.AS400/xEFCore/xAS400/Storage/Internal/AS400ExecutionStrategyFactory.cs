using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Storage.Internal {
  public class AS400ExecutionStrategyFactory : RelationalExecutionStrategyFactory {
    public AS400ExecutionStrategyFactory(
           [NotNull] ExecutionStrategyDependencies dependencies)
           : base(dependencies) {
    }

    protected override IExecutionStrategy CreateDefaultStrategy(ExecutionStrategyDependencies dependencies)
        => new AS400ExecutionStrategy(dependencies);
  }
}
