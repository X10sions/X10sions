using CleanOnionExample.Services.Auth;
using Common.Data;

namespace CleanOnionExample.Services.Identity;

public interface IIdentityService {
  Task<Result<Token.Response>> GetTokenAsync(Token.Request request, string ipAddress);
  Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);
  Task<Result<string>> ConfirmEmailAsync(string userId, string code);
  Task ForgotPassword(ForgotPasswordRequest model, string origin);
  Task<Result<string>> ResetPassword(ResetPasswordRequest model);
}