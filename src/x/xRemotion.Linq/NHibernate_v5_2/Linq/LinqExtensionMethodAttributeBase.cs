using System;

namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// Base class for Linq extension attributes.
  /// </summary>
  public abstract class LinqExtensionMethodAttributeBase : Attribute {
    /// <summary>
    /// Should the method call be pre-evaluated when not depending on queried data? If it can,
    /// it would then be evaluated and replaced by the resulting (parameterized) constant expression
    /// in the resulting SQL query.
    /// </summary>
    public LinqExtensionPreEvaluation PreEvaluation { get; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="preEvaluation">Should the method call be pre-evaluated when not depending on queried data?</param>
    protected LinqExtensionMethodAttributeBase(LinqExtensionPreEvaluation preEvaluation) {
      PreEvaluation = preEvaluation;
    }
  }
}
