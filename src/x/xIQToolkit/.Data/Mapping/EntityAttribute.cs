using System;

namespace IQToolkit.Data.Mapping {
  /// <summary>
  /// Describes information about an entity class.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class EntityAttribute : MappingAttribute {
    /// <summary>
    /// The ID associated with the entity mapping.
    /// If not specified, the entity id will be the entity type's simple name.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The type that is constructed when the entity is returned as the result of a query.
    /// If not specified it is the same as the entity type, the type the attribute is placed on.
    /// </summary>
    public Type RuntimeType { get; set; }
  }
}
