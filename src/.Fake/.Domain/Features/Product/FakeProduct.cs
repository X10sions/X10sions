using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Product;
[Table("FakeProduct")]
  public class FakeProduct {
    [ServiceStack.DataAnnotations.AutoIncrement] public int Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
  }
