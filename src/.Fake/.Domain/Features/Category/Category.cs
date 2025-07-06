using Common.Domain.Entities;

namespace X10sions.Fake.Features.Category;

public class Category : EntityBase<int> {
  public string CategoryName { get; set; }
  public string Description { get; set; }
  public List<Product.Product> Products { get; set; }
}
