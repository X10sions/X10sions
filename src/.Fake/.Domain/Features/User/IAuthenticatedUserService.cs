namespace X10sions.Fake.Features.User;

public interface IAuthenticatedUserService : IUserService {
  string UserName { get; }
}
