using Common.Domain.Entities;

namespace Common.Features.DummyFakeExamples.Brand;

public class Brand : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Tax { get; set; }
}
