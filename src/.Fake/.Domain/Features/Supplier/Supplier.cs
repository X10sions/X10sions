using Common.Domain.Entities;

namespace X10sions.Fake.Features.Supplier;

public class FakeSupplier : EntityBase<int> {
  public string SupplierName { get; set; }
  public List<Product.FakeProduct> Products { get; set; }
}
