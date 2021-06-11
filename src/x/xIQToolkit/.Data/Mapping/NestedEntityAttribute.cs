using System;

namespace IQToolkit.Data.Mapping {
  /// <summary>
  /// Denotes the entity as a
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
  public class NestedEntityAttribute : MemberAttribute {
    /// <summary>
    /// The type that is constructed when the entity is returned as the result of a query.
    /// If not specified it is the same as the entity type, the type of the class or element type of the member the attribute is placed on.
    /// </summary>
    public Type RuntimeType { get; set; }
  }
}
