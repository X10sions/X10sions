using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  public class InheritanceParent2 : InheritanceParentBase {
    [Column] public string? Name { get; set; }
  }
}
