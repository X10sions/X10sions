
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeIdName")]
  public class FakeIdName {
    public int Id { get; set; }
    public string Name { get; set; }
  }


}
