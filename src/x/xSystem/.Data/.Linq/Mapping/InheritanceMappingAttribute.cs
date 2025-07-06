using System.Diagnostics.CodeAnalysis;

namespace System.Data.Linq.Mapping;

/// <summary>
/// Class attribute used to describe an inheritance hierarchy to be mapped.
/// For example, 
/// 
///     [Table(Name = "People")]
///     [InheritanceMapping(Code = "P", Type = typeof(Person), IsDefault=true)]
///     [InheritanceMapping(Code = "C", Type = typeof(Customer))]
///     [InheritanceMapping(Code = "E", Type = typeof(Employee))]
///     class Person { ... }
///     
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class InheritanceMappingAttribute : Attribute {
  private object code;
  private Type type;
  private bool isDefault;

  /// <summary>
  /// Discriminator value in store column for this type.
  /// </summary>
  public object Code {
    get { return this.code; }
    set { this.code = value; }
  }
  /// <summary>
  /// Type to instantiate when Key is matched.
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "The contexts in which this is available are fairly specific.")]
  public Type Type {
    get { return this.type; }
    set { this.type = value; }
  }

  /// <summary>
  /// If discriminator value in store column is unrecognized then instantiate this type.
  /// </summary>
  public bool IsDefault {
    get { return this.isDefault; }
    set { this.isDefault = value; }
  }

}
