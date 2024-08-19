namespace Common.Data.Specifications;

public interface ISingleResultSpecification {}
public interface ISingleResultSpecification<T> : ISpecification<T>, ISingleResultSpecification { }

