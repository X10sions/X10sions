namespace Common.Data.Specification;

public interface ISpecificationBuilder<T> {
  ISpecification<T> Specification { get; }
}

public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T> {
  new ISpecification<T, TResult> Specification { get; }
}

public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult> {
  public new ISpecification<T, TResult> Specification { get; }

  public SpecificationBuilder(Specification<T, TResult> specification) : base(specification) {
    Specification = specification;
  }
}

public class SpecificationBuilder<T> : ISpecificationBuilder<T> {
  public ISpecification<T> Specification { get; }

  public SpecificationBuilder(Specification<T> specification) {
    Specification = specification;
  }
}