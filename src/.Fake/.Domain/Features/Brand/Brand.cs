using Common.Domain.Entities;
using Common.Results;

namespace X10sions.Fake.Features.Brand;

public class Brand : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Tax { get; set; }

  public static class Errors {
    public static readonly Error NotFound = new("Brand Not Found.");
  }

}
