using CleanOnionExample.Services.Auth;

namespace CleanOnionExample.Data.Entities.Services;

public interface IAccountService1 {
  Task<Response<Authentication.Response>> AuthenticateAsync(Authentication.Request request, string ipAddress);
  Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
  Task<Response<string>> ConfirmEmailAsync(string userId, string code);
  System.Threading.Tasks.Task ForgotPassword(ForgotPasswordRequest model, string origin);
  Task<Response<string>> ResetPassword(ResetPasswordRequest model);
}

public interface IAccountService2 {
  Task<IEnumerable<Account.GetQuery>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
  Task<Account.GetQuery> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken);
  Task<Account.GetQuery> CreateAsync(Guid ownerId, Account.UpdateCommand accountForCreationDto, CancellationToken cancellationToken = default);
  System.Threading.Tasks.Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default);
}
