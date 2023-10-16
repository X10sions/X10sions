using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Supplier : EntityBase<int> {
  public string SupplierName { get; set; }
  public List<Product> Products { get; set; }
}
