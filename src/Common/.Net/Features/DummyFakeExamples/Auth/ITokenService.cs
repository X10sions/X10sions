namespace Common.Features.DummyFakeExamples.Auth;

public interface ITokenService {
  AuthTokenDTO Generate(User.User user);
}
