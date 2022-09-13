using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeIdName")]
  public class FakeIdName {
    [Column(Order = 1),Key, LinqToDB.Mapping.Identity, LinqToDB.Mapping.PrimaryKey] public int Id { get; set; }
    [Column] public string Name { get; set; }
  }

}
