using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Features.Auth;

namespace Common.Behaviours;
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull {
  private readonly ILogger _logger;
  private readonly ICurrentUserService _currentUserService;
  private readonly Interfaces.IIdentityService _identityService;

  public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService,  Interfaces.IIdentityService identityService) {
    _logger = logger;
    _currentUserService = currentUserService;
    _identityService = identityService;
  }

  public async Task Process(TRequest request, CancellationToken cancellationToken) {
    var requestName = typeof(TRequest).Name;
    var userId = _currentUserService.UserId ?? string.Empty;
    var userName = string.Empty;

    if (!string.IsNullOrEmpty(userId)) {
      userName = await _identityService.GetUserNameAsync(userId);
    }

    _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
        requestName, userId, userName, request);
  }
}
