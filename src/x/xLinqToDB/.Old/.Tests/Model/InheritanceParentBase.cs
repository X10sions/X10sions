using LinqToDB.Mapping;

namespace LinqToDB.Tests.Model {
  [Table("InheritanceParent")]
  [InheritanceMapping(Code = null, Type = typeof(InheritanceParentBase), IsDefault = true)]
  [InheritanceMapping(Code = 1, Type = typeof(InheritanceParent1))]
  [InheritanceMapping(Code = 2, Type = typeof(InheritanceParent2))]
  public class InheritanceParentBase : TInheritance {
    [PrimaryKey] public int InheritanceParentId { get; set; }
    [Column(IsDiscriminator = true)] public int? TypeDiscriminator { get; set; }

    public override bool Equals(object? obj) {
      var other = obj as InheritanceParentBase;
      if (other == null)
        return false;

      if (ReferenceEquals(this, other))
        return true;

      return
        InheritanceParentId == other.InheritanceParentId &&
        TypeDiscriminator == other.TypeDiscriminator &&
        GetType() == other.GetType();
    }

    public override int GetHashCode() => InheritanceParentId;
  }
}
