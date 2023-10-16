namespace Common.Features.DummyFakeExamples.User;

public interface IAuthenticatedUserService : IUserService {
  string UserName { get; }
}
