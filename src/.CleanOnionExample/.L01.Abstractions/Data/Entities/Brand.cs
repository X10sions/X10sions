using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Brand : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Tax { get; set; }
}

