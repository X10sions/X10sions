using Common.Domain.Entities;

namespace Common.Features.DummyFakeExamples.Item;
public class Item : EntityAuditableBase<int> {
  public string Name { get; set; }
  public string Description { get; set; }
  public string Categories { get; set; }
  public string ColorCode { get; set; }
}
