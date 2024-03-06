using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace xEFCore.xAS400.Query.Internal {
  public class AS400QueryCompilationContextFactory : RelationalQueryCompilationContextFactory {
    public AS400QueryCompilationContextFactory(
           [NotNull] QueryCompilationContextDependencies dependencies,
           [NotNull] RelationalQueryCompilationContextDependencies relationalDependencies)
           : base(dependencies, relationalDependencies) {
    }
    public override QueryCompilationContext Create(bool async)
       => async
           ? new AS400QueryCompilationContext(
               Dependencies,
               new AsyncLinqOperatorProvider(),
               new AsyncQueryMethodProvider(),
               TrackQueryResults)
           : new AS400QueryCompilationContext(
               Dependencies,
               new LinqOperatorProvider(),
               new QueryMethodProvider(),
               TrackQueryResults);
  }
}
