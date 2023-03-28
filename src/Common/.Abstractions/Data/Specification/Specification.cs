using System.Linq.Expressions;

namespace Common.Data.Specification;

public abstract class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult> {
  protected new virtual ISpecificationBuilder<T, TResult> Query { get; }

  protected Specification() : this(InMemorySpecificationEvaluator.Default) { }

  protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator) : base(inMemorySpecificationEvaluator) {
    Query = new SpecificationBuilder<T, TResult>(this);
  }

  public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities) => Evaluator.Evaluate(entities, this);

  public Expression<Func<T, TResult>>? Selector { get; set; }
  public new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; set; } = null;
}
public abstract class Specification<T> : ISpecification<T> {
  protected IInMemorySpecificationEvaluator Evaluator { get; }
  protected ISpecificationValidator Validator { get; }
  protected virtual ISpecificationBuilder<T> Query { get; }

  protected Specification() : this(InMemorySpecificationEvaluator.Default, SpecificationValidator.Default) { }

  protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator)
      : this(inMemorySpecificationEvaluator, SpecificationValidator.Default) { }

  protected Specification(ISpecificationValidator specificationValidator)
      : this(InMemorySpecificationEvaluator.Default, specificationValidator) { }

  protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator, ISpecificationValidator specificationValidator) {
    Evaluator = inMemorySpecificationEvaluator;
    Validator = specificationValidator;
    Query = new SpecificationBuilder<T>(this);
  }

  public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities) => Evaluator.Evaluate(entities, this);
  public virtual bool IsSatisfiedBy(T entity) => Validator.IsValid(entity, this);
  public IEnumerable<IWhereExpressionInfo<T>> WhereExpressions { get; } = new List<WhereExpressionInfo<T>>();
  public IEnumerable<IOrderExpressionInfo<T>> OrderExpressions { get; } = new List<OrderExpressionInfo<T>>();
  public IEnumerable<IIncludeExpressionInfo> IncludeExpressions { get; } = new List<IncludeExpressionInfo>();
  public IEnumerable<string> IncludeStrings { get; } = new List<string>();
  public IEnumerable<ISearchExpressionInfo<T>> SearchCriterias { get; } = new List<SearchExpressionInfo<T>>();
  public int? Take { get; set; } = null;
  public int? Skip { get; set; } = null;
  public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; set; } = null;
  public string? CacheKey { get; set; }
  public bool CacheEnabled { get; set; }
  public bool AsNoTracking { get; set; } = false;
  public bool AsSplitQuery { get; set; } = false;
  public bool AsNoTrackingWithIdentityResolution { get; set; } = false;
  public bool IgnoreQueryFilters { get; set; } = false;
}