using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using System.Collections.Generic;
using xEFCore.xAS400.Query.ExpressionTranslators.Internal.MethodCallTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal {
  public class AS400CompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator {

    public AS400CompositeMethodCallTranslator(
      [NotNull] RelationalCompositeMethodCallTranslatorDependencies dependencies)
      : base(dependencies) {
      AddTranslators(Translators);
    }

    public static readonly List<IMethodCallTranslator> Translators = new List<IMethodCallTranslator>{
      //new AS400ContainsOptimizedTranslator(),
      //new AS400ConvertTranslator(),
      //new AS400DateAddTranslator(),
      //new AS400EndsWithOptimizedTranslator(),
      //new AS400MathTranslator(),
      //new Db2MathAbsTranslator(),
      //new Db2MathCeilingTranslator(),
      //new Db2MathFloorTranslator(),
      //new Db2MathPowerTranslator(),
      //new Db2MathRoundTranslator(),
      //new Db2MathTruncateTranslator(),
      //new AS400NewGuidTranslator(),
      //new Db2NewGuidTranslator(),
      //new AS400ObjectToStringTranslator(),
      //new AS400StartsWithOptimizedTranslator(),
      //new AS400StringIsNullOrWhiteSpaceTranslator(),
      //new Db2StringIsNullOrWhiteSpaceTranslator(),
      //new AS400StringReplaceTranslator(),
      //new Db2StringReplaceTranslator(),
      new AS400StringSubstringTranslator(),
      new AS400StringToLowerTranslator(),
      new AS400StringToUpperTranslator(),
      new AS400StringTrimEndTranslator(),
      new AS400StringTrimStartTranslator(),
      new AS400StringTrimTranslator(),
      //new Db2SumTranslator()
    };

  }
}