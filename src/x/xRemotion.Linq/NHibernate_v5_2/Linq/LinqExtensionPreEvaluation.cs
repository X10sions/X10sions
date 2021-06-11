namespace NHibernate_v5_2.Linq {
  /// <summary>
  /// Possible method call behaviors when the linq to NHibernate provider pre-evaluates
  /// expressions before translating them to SQL.
  /// </summary>
  public enum LinqExtensionPreEvaluation {
    /// <summary>
    /// The method call will not be evaluated even if its arguments do not depend on queried data.
    /// It will always be translated to the corresponding SQL statement.
    /// </summary>
    NoEvaluation,
    /// <summary>
    /// If the method call does not depend on queried data, the method call will be evaluated and replaced
    /// by the resulting (parameterized) constant expression in the resulting SQL query. A throwing
    /// method implementation will cause the query to throw.
    /// </summary>
    AllowPreEvaluation
  }
}
