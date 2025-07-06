namespace Common.Data.Specifications;

public interface IInMemoryEvaluator {
  IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
}

public interface IInMemorySpecificationEvaluator {
  IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification);
  IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification);
}

public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator {
  // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
  public static InMemorySpecificationEvaluator Default { get; } = new InMemorySpecificationEvaluator();

  private readonly List<IInMemoryEvaluator> evaluators = new List<IInMemoryEvaluator>();

  public InMemorySpecificationEvaluator() {
    evaluators.AddRange(new IInMemoryEvaluator[] {
      WhereEvaluator.Instance,
      SearchEvaluator.Instance,
      OrderEvaluator.Instance,
      PaginationEvaluator.Instance
    });
  }
  public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators) {
    this.evaluators.AddRange(evaluators);
  }

  public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification) {
    _ = specification.Selector ?? throw new Exception("SelectorNotFoundException");
    var baseQuery = Evaluate(source, (ISpecification<T>)specification);
    var resultQuery = baseQuery.Select(specification.Selector.Compile());
    return specification.PostProcessingAction == null        ? resultQuery        : specification.PostProcessingAction(resultQuery);
  }

  public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification) {
    foreach (var evaluator in evaluators) {
      source = evaluator.Evaluate(source, specification);
    }
    return specification.PostProcessingAction == null        ? source        : specification.PostProcessingAction(source);
  }
}

