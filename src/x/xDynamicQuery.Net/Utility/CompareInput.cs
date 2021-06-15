using System.Linq.Expressions;

namespace DynamicQueryNet.Utility {
  public class CompareInput {
    public MemberExpression PropertyExpr { get; set; }
    public ConstantExpression Value { get; set; }
  }
}
