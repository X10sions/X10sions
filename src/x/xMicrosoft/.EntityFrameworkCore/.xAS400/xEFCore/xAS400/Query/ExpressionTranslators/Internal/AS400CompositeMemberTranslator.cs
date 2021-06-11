using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using xEFCore.xAS400.Query.ExpressionTranslators.Internal.MemberTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal {
  public class AS400CompositeMemberTranslator : RelationalCompositeMemberTranslator {
    public AS400CompositeMemberTranslator([NotNull] RelationalCompositeMemberTranslatorDependencies dependencies)
           : base(dependencies) {
      AddTranslators(Translators);
    }

    public static readonly List<IMemberTranslator> Translators = new List<IMemberTranslator> {
        new AS400DateTimeDateTranslator(),
        new AS400DateTimeNowTranslator (),
        new AS400DateTimeTimeOfDayTranslator(),
        new AS400DateTimeUtcNowTranslator(),
        new AS400StringLengthTranslator()       
      };

  }
}