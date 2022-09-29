using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakePriority")]
  public class FakePriority {
    [ServiceStack.DataAnnotations.AutoIncrement, LinqToDB.Mapping.Identity,Column] public int Id { get; set; }
    [Column] public char Code { get; set; }
    [Column] public string Description { get; set; } = string.Empty;

    public IEnumerable<FakeProjectItem> ProjectItems { get; set; } = null!;
  }
}
