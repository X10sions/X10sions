using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  public class InheritanceChild2 : InheritanceChildBase {
    [Column] public string? Name { get; set; }
  }
}