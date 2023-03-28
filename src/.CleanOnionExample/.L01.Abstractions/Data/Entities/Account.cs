using Common.Data.Entities;

namespace CleanOnionExample.Data.Entities;

public class Account : EntityBase<Guid> {
  public DateTime DateCreated { get; set; }
  public string AccountType { get; set; }
  public Guid OwnerId { get; set; }


  public record GetQuery(Guid Id, Guid OwnerId, DateTime DateCreated, string AccountType);
  public record UpdateCommand(DateTime DateCreated, string AccountType);
}
