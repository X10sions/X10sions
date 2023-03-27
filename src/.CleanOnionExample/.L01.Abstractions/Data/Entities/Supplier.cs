using Common.Data;

namespace CleanOnionExample.Data.Entities;

public class Supplier : BaseEntity<int> {
  public string SupplierName { get; set; }
  public List<Product> Products { get; set; }
}
