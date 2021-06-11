using System.Linq.Expressions;

namespace DynamicQueryNet.Utility {
  public interface ICompare {
    Expression Equal<T>(CompareInput input);
    Expression NotEqual<T>(CompareInput input);
    Expression GreaterThan<T>(CompareInput input);
    Expression GreaterThanOrEqual<T>(CompareInput input);
    Expression LessThan<T>(CompareInput input);
    Expression LessThanOrEqual<T>(CompareInput input);
    Expression Contains<T>(CompareInput input);
  }
}
