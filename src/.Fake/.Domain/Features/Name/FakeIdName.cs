using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Name;
[Table("FakeIdName")]
public class FakeIdName {
  [Column(Order = 1), Key] public int Id { get; set; }
  [Column] public string Name { get; set; }
}

