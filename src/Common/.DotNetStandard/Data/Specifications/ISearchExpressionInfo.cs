using System.Linq.Expressions;

namespace Common.Data.Specifications;

public interface ISearchExpressionInfo<T> {
   Expression<Func<T, string>> Selector { get; }
   string SearchTerm { get; }
   int SearchGroup { get; }
   Func<T, string> SelectorFunc { get; }
}

public class SearchExpressionInfo<T> : ISearchExpressionInfo<T> {
  private readonly Lazy<Func<T, string>> selectorFunc;

  public SearchExpressionInfo(Expression<Func<T, string>> selector, string searchTerm, int searchGroup = 1) {
    _ = selector ?? throw new ArgumentNullException(nameof(selector));
    if (string.IsNullOrEmpty(searchTerm)) throw new ArgumentException(nameof(searchTerm));
    Selector = selector;
    SearchTerm = searchTerm;
    SearchGroup = searchGroup;
    selectorFunc = new Lazy<Func<T, string>>(Selector.Compile);
  }

  public Expression<Func<T, string>> Selector { get; }
  public string SearchTerm { get; }
  public int SearchGroup { get; }
  public Func<T, string> SelectorFunc => selectorFunc.Value;
}