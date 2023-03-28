namespace Common.Data.Specification;

public interface IValidator {
  bool IsValid<T>(T entity, ISpecification<T> specification);
}

public class SearchValidator : IValidator {
  private SearchValidator() { }
  public static SearchValidator Instance { get; } = new SearchValidator();
  public bool IsValid<T>(T entity, ISpecification<T> specification) {
    foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup)) {
      if (!searchGroup.Any(c => c.SelectorFunc(entity).IsLikeSql(c.SearchTerm))) return false;
    }
    return true;
  }
}

public class WhereValidator : IValidator {
  private WhereValidator() { }
  public static WhereValidator Instance { get; } = new WhereValidator();
  public bool IsValid<T>(T entity, ISpecification<T> specification) {
    foreach (var info in specification.WhereExpressions) {
      if (!info.FilterFunc(entity)) return false;
    }
    return true;
  }
}

