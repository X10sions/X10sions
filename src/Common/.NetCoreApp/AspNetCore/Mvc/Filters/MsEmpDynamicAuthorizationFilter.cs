using Common.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc.Filters {
  public class MsEmpDynamicAuthorizationFilter : IAsyncAuthorizationFilter {
    public MsEmpDynamicAuthorizationFilter(MsEmpDbContext dbContext) {
      this.dbContext = dbContext;
    }
    readonly MsEmpDbContext dbContext;

    public Task OnAuthorizationAsync(AuthorizationFilterContext context) => context.OnDynamicAuthorizationAsync(dbContext);
  }
}
