using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Data.Models {
  [Table("FakeProduct")]
  public class FakeProduct {
    [ServiceStack.DataAnnotations.AutoIncrement, LinqToDB.Mapping.Identity] public int Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
  }


}
