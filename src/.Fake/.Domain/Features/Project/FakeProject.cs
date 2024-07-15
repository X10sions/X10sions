using System.ComponentModel.DataAnnotations.Schema;
using X10sions.Fake.Features.Project.Item;

namespace X10sions.Fake.Features.Project;
[Table("FakeProject")]
public class FakeProject {
  [ServiceStack.DataAnnotations.AutoIncrement, Column] public int Id { get; set; }
  [Column] public string Title { get; set; } = string.Empty;
  public IEnumerable<FakeProjectItem> Items { get; set; } = null!;
}
