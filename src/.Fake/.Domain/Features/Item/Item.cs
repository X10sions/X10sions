using Common.Domain.Entities;

namespace X10sions.Fake.Features.Item;
public class Item : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public string Categories { get; set; }
  public string ColorCode { get; set; }
}
