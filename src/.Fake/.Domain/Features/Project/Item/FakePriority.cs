using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Project.Item;
[Table("FakePriority")]
public class FakePriority {
  [ServiceStack.DataAnnotations.AutoIncrement, Column] public int Id { get; set; }
  [Column] public char Code { get; set; }
  [Column] public string Description { get; set; } = string.Empty;

  public IEnumerable<FakeProjectItem> ProjectItems { get; set; } = null!;
}
