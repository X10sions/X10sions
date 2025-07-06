namespace X10sions.Fake.Features.Owner;

public class GetOwnerQuery {
  public Guid Id { get; set; }
  public string Name { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Address { get; set; }
  public IEnumerable<Account.Account.GetQuery> Accounts { get; set; }
}

