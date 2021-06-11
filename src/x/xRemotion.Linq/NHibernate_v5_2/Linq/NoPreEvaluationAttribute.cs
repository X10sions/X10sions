namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// Can flag a method as not being callable by the runtime, when used in Linq queries.
  /// If the method is supported by the linq-to-nhibernate provider, it will always be converted
  /// to the corresponding SQL statement.
  /// Otherwise the linq-to-nhibernate provider evaluates method calls when they do not depend on
  /// the queried data.
  /// </summary>
  public class NoPreEvaluationAttribute : LinqExtensionMethodAttributeBase {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public NoPreEvaluationAttribute()
      : base(LinqExtensionPreEvaluation.NoEvaluation) { }
  }
}
