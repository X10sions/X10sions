using Common.Domain.Entities;

namespace X10sions.Fake.Features.Brand;

public class Brand : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Tax { get; set; }
}
