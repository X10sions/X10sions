namespace Common.Data.Specification;

public interface ISingleResultSpecification {}
public interface ISingleResultSpecification<T> : ISpecification<T>, ISingleResultSpecification { }

