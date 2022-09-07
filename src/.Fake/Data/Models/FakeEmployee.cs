using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeEmployee")]
  public class FakeEmployee {
    public int Id { get; set; }
    public string Name { get; set; }
  }


}
