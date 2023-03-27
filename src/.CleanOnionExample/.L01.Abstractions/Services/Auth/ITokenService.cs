using CleanOnionExample.Data.Entities;

namespace CleanOnionExample.Services.Auth;

public interface ITokenService {
  AuthTokenDTO Generate(User user);
}
