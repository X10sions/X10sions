namespace Common.Interfaces;

public interface IIdentityService {
  Task<string> GetUserNameAsync(string userId);
  Task<bool> IsInRoleAsync(string userId, string role);
  Task<bool> AuthorizeAsync(string userId, string policyName);
  Task<(Common.Data.Result Result, string UserId)> CreateUserAsync(string userName, string password);
  Task<Common.Data.Result> DeleteUserAsync(string userId);
}