using Common.Data;

namespace CleanOnionExample.Data.Entities;

public class Owner : BaseEntity<Guid> {
  public string Name { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Address { get; set; }
  public ICollection<Account> Accounts { get; set; }
}
