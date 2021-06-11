namespace IQToolkit.Data.Mapping {
  /// <summary>
  /// A base class for member mapping information.
  /// </summary>
  public abstract class MemberAttribute : MappingAttribute {
    /// <summary>
    /// The member for the mapping.
    /// If not specified it is inferred to be the member the attribute is placed on.
    /// </summary>
    public string Member { get; set; }
  }
}
