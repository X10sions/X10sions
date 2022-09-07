using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeProject")]
  public class FakeProject {
    [Column] public int Id { get; set; }
    [Column] public string Title { get; set; } = string.Empty;
    public IEnumerable<FakeProjectItem> Items { get; set; } = null!;
  }


}
