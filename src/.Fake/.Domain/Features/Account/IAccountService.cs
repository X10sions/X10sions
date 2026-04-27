using Common.Results;

using X10sions.Fake.Features.Auth;

namespace X10sions.Fake.Features.Account;

public interface IAccountService1 {
  Task<Response<Authentication.Response>> AuthenticateAsync(Authentication.Request request, string ipAddress);
  Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
  Task<Response<string>> ConfirmEmailAsync(string userId, string code);
  Task ForgotPassword(ForgotPasswordRequest model, string origin);
  Task<Response<string>> ResetPassword(ResetPasswordRequest model);
}

public interface IAccountService2 {
  Task<IEnumerable<FakeAccount.GetQuery>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
  Task<FakeAccount.GetQuery> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken);
  Task<FakeAccount.GetQuery> CreateAsync(Guid ownerId, FakeAccount.UpdateCommand accountForCreationDto, CancellationToken cancellationToken = default);
  Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default);
}