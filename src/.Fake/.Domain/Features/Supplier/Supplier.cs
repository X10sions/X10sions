using Common.Domain.Entities;
using X10sions.Fake.Features.Product;

namespace Common.Features.DummyFakeExamples.Supplier;

public class Supplier : EntityBase<int> {
  public string SupplierName { get; set; }
  public List<Product> Products { get; set; }
}
