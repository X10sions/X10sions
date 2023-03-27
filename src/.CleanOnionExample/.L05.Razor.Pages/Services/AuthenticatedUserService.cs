using CleanOnionExample.Data.Entities.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanOnionExample.Services;

public class AuthenticatedUserService : IAuthenticatedUserService {
  public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) {
    UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
  }

  public string UserId { get; }
  public string UserName { get; }
}
