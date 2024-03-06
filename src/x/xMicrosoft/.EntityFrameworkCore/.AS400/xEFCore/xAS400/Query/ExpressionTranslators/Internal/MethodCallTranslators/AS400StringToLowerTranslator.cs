using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators {
  public class AS400StringToLowerTranslator : ParameterlessInstanceMethodCallTranslator {
    public AS400StringToLowerTranslator()
      : base(typeof(string), nameof(string.ToLower), "LOWER") {
    }
  }
}
