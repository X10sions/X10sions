using Common.Domain.Entities;

namespace X10sions.Fake.Features.Supplier;

public class Supplier : EntityBase<int> {
  public string SupplierName { get; set; }
  public List<Product.Product> Products { get; set; }
}
