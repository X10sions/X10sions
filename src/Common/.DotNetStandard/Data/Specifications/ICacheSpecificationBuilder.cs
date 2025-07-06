namespace Common.Data.Specifications;

public interface ICacheSpecificationBuilder<T> : ISpecificationBuilder<T> where T : class {
  bool IsChainDiscarded { get; set; }
}

public class CacheSpecificationBuilder<T> : ICacheSpecificationBuilder<T> where T : class {
  public ISpecification<T> Specification { get; }
  public bool IsChainDiscarded { get; set; }

  public CacheSpecificationBuilder(ISpecification<T> specification) : this(specification, false) { }

  public CacheSpecificationBuilder(ISpecification<T> specification, bool isChainDiscarded) {
    Specification = specification;
    IsChainDiscarded = isChainDiscarded;
  }
}