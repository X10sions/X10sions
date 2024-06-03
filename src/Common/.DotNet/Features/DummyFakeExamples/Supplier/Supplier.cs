using Common.Data.Entities;

namespace Common.Features.DummyFakeExamples.Supplier;

public class Supplier : EntityBase<int> {
  public string SupplierName { get; set; }
  public List<Product.Product> Products { get; set; }
}
