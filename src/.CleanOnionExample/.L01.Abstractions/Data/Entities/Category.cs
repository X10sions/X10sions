using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Category : EntityBase<int> {
  public string CategoryName { get; set; }
  public string Description { get; set; }
  public List<Product> Products { get; set; }
}
