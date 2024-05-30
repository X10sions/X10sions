using Common.Data.Entities;

namespace Common.Features.DummyFakeExamples.Owner;

public class Owner : EntityBase<Guid> {
  public string Name { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Address { get; set; }
  public ICollection<Account.Account> Accounts { get; set; }
}

