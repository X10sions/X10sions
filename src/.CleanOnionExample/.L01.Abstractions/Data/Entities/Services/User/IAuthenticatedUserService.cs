namespace CleanOnionExample.Data.Entities.Services;

public interface IAuthenticatedUserService : IUserService {
  string UserName { get; }
}
