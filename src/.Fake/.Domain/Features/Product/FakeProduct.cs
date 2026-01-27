using Common.Domain.Entities;
using Common.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Product;

[Table("FakeProduct")]
public class FakeProduct : EntityAuditableBase<int> {
  [ServiceStack.DataAnnotations.AutoIncrement] public int Id { get; set; }
  public string Name { get; set; }
  [Column(TypeName = "money")] public decimal UnitPrice { get; set; }
  public string Barcode { get; set; }
  public byte[] Image { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  public int BrandId { get; set; }
  public virtual Brand.FakeBrand Brand { get; set; }

  public static class Errors {
    public static readonly Error NotFound = new("Product Not Found.");
  }

}
