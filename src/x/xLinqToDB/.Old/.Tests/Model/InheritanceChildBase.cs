using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  [Table("InheritanceChild")]
  [InheritanceMapping(Code = null, Type = typeof(InheritanceChildBase), IsDefault = true)]
  [InheritanceMapping(Code = 1, Type = typeof(InheritanceChild1))]
  [InheritanceMapping(Code = 2, Type = typeof(InheritanceChild2))]
  public class InheritanceChildBase : TInheritance {
    [PrimaryKey] public int InheritanceChildId { get; set; }
    [Column(IsDiscriminator = true)] public int? TypeDiscriminator { get; set; }
    [Column] public int InheritanceParentId { get; set; }

    [Association(ThisKey = "InheritanceParentId", OtherKey = "InheritanceParentId")]
    public InheritanceParentBase Parent { get; set; } = null!;

    public override bool Equals(object? obj) {
      var other = obj as InheritanceChildBase;
      if (other == null)
        return false;

      if (ReferenceEquals(this, other))
        return true;

      return InheritanceChildId == other.InheritanceChildId
        && TypeDiscriminator == other.TypeDiscriminator
        && GetType() == other.GetType();
    }

    public override int GetHashCode() => InheritanceChildId;
  }
}
