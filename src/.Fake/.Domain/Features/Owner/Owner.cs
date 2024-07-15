using Common.Domain.Entities;

namespace X10sions.Fake.Features.Owner;

public class Owner : EntityBase<Guid> {
  public string Name { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Address { get; set; }
  public ICollection<Account.Account> Accounts { get; set; }
}

