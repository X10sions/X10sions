using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Item : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public string Categories { get; set; }
  public string ColorCode { get; set; }
}
