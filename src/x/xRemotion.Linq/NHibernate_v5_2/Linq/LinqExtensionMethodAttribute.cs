namespace NHibernate_v5_2.Linq {
  public class LinqExtensionMethodAttribute : LinqExtensionMethodAttributeBase {
    /// <summary>
    /// Default constructor. The method call will be translated by the linq provider to
    /// a function call having the same name than the method.
    /// </summary>
    public LinqExtensionMethodAttribute()
      : base(LinqExtensionPreEvaluation.NoEvaluation) { }

    /// <summary>
    /// Constructor specifying a SQL function name.
    /// </summary>
    /// <param name="name">The name of the SQL function.</param>
    public LinqExtensionMethodAttribute(string name)
      : this(name, LinqExtensionPreEvaluation.NoEvaluation) { }

    /// <summary>
    /// Constructor allowing to specify a <see cref="LinqExtensionPreEvaluation"/> for the method.
    /// </summary>
    /// <param name="preEvaluation">Should the method call be pre-evaluated when not depending on
    /// queried data? Default is <see cref="LinqExtensionPreEvaluation.NoEvaluation"/>.</param>
    public LinqExtensionMethodAttribute(LinqExtensionPreEvaluation preEvaluation)
      : base(preEvaluation) { }

    /// <summary>
    /// Constructor for specifying a SQL function name and a <see cref="LinqExtensionPreEvaluation"/>.
    /// </summary>
    /// <param name="name">The name of the SQL function.</param>
    /// <param name="preEvaluation">Should the method call be pre-evaluated when not depending on
    /// queried data? Default is <see cref="LinqExtensionPreEvaluation.NoEvaluation"/>.</param>
    public LinqExtensionMethodAttribute(string name, LinqExtensionPreEvaluation preEvaluation)
      : base(preEvaluation) {
      Name = name;
    }

    /// <summary>
    /// The name of the SQL function.
    /// </summary>
    public string Name { get; }
  }
}
