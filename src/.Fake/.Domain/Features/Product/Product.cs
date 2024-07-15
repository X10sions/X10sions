using Common.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Fake.Features.Product;

public class Product : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Barcode { get; set; }
  public byte[] Image { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  [Column(TypeName = "money")] public decimal UnitPrice { get; set; }
  public int BrandId { get; set; }
  public virtual Brand.Brand Brand { get; set; }
}
