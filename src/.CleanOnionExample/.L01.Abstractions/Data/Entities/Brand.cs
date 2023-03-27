using Common.Data;

namespace CleanOnionExample.Data.Entities;

public class Brand : BaseEntityAuditable<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Tax { get; set; }
}

