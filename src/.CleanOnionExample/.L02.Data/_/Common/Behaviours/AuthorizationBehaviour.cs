using Common.Exceptions;
using Common.Security;
using MediatR;
using System.Reflection;
using X10sions.Fake.Features.Auth;

namespace Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(ICurrentUserService currentUserService, Interfaces.IIdentityService identityService) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> {
  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
    var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
    if (authorizeAttributes.Any()) {
      // Must be authenticated user
      if (currentUserService.UserId == null) {
        throw new UnauthorizedAccessException();
      }
      // Role-based authorization
      var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
      if (authorizeAttributesWithRoles.Any()) {
        var authorized = false;
        foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(','))) {
          foreach (var role in roles) {
            var isInRole = await identityService.IsInRoleAsync(currentUserService.UserId, role.Trim());
            if (isInRole) {
              authorized = true;
              break;
            }
          }
        }
        // Must be a member of at least one role in roles
        if (!authorized) {
          throw new ForbiddenAccessException();
        }
      }
      // Policy-based authorization
      var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
      if (authorizeAttributesWithPolicies.Any()) {
        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy)) {
          var authorized = await identityService.AuthorizeAsync(currentUserService.UserId, policy);
          if (!authorized) {
            throw new ForbiddenAccessException();
          }
        }
      }
    }

    // User is authorized / authorization not required
    return await next();
  }

}
