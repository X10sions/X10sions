using Common.NetCoreApp.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.AspNetCore.Mvc.Filters {
  public class MsEmpDynamicAuthorizationFilter : IAsyncAuthorizationFilter {
    public MsEmpDynamicAuthorizationFilter(MsEmpDbContext dbContext) {
      this.dbContext = dbContext;
    }
    readonly MsEmpDbContext dbContext;

    public Task OnAuthorizationAsync(AuthorizationFilterContext context) => context.OnDynamicAuthorizationAsync(dbContext);
  }
}
