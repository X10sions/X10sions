using X10sions.Fake.Features.Account;
using X10sions.Fake.Features.Owner;
using X10sions.Fake.Features.Person;

namespace X10sions.Fake.Domain.Services;

public interface IServiceManager {
  IOwnerService OwnerService { get; }
  IAccountService2 AccountService2 { get; }
  IPersonService PersonService { get; }
}
