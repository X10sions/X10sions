using Common.Domain.Entities;

namespace X10sions.Fake.Features.Owner;

public class FakeOwner : EntityBase<Guid> {
  public string Name { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Address { get; set; }
  public ICollection<Account.FakeAccount> Accounts { get; set; }
}

