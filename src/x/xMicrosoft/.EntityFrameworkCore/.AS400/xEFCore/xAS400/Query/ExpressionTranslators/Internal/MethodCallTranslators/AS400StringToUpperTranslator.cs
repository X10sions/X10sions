using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators {
  public class AS400StringToUpperTranslator : ParameterlessInstanceMethodCallTranslator {
    public AS400StringToUpperTranslator()
      : base(typeof(string), nameof(string.ToUpper), "UPPER") {
    }
  }
}
