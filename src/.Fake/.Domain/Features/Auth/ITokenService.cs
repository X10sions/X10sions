namespace X10sions.Fake.Features.Auth;

public interface ITokenService {
  AuthTokenDTO Generate(User.User user);
}
