using Common.AspNetCore.Identity.EntityFrameworkCore;
using Common.Collections.Paged;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.AspNetCore.Mvc.Filters {
  public static class MsEmpAuthorizationFilterContextExtensions {

    public static async Task OnDynamicAuthorizationAsync(this AuthorizationFilterContext context, MsEmpDbContext dbContext) {
      // https://github.com/mo-esmp/DynamicRoleBasedAuthorizationNETCore/blob/master/src/DynamicRoleBasedAuthorization/Filters/DynamicAuthorizationFilter.cs

      if (context.HasAllowAnonymousFilter()) return;
      TypeInfo controllerOrPageInfo;
      MemberInfo actionOrHandlerInfo;
      string actionDescriptorId;

      switch (context.ActionDescriptor.GetActionDescriptorType()) {
        case ActionDescriptorType.MvcController:
          var controllerDescriptor = context.ControllerActionDescriptor();
          controllerOrPageInfo = controllerDescriptor.ControllerTypeInfo;
          actionOrHandlerInfo = controllerDescriptor.MethodInfo;
          var ca = controllerDescriptor.GetAreaControllerAction();
          actionDescriptorId = $"{Path.AltDirectorySeparatorChar}{ca.areaName}{Path.AltDirectorySeparatorChar}{ca.controllerName}{Path.AltDirectorySeparatorChar}{ca.actionName}";
          break;
        case ActionDescriptorType.MvcRazorPage:
          var pageDescriptor = context.CompiledPageActionDescriptor();
          controllerOrPageInfo = pageDescriptor.PageTypeInfo;
          actionOrHandlerInfo = pageDescriptor.HandlerTypeInfo;
          var ph = pageDescriptor.GetAreaPageHandler();
          actionDescriptorId = $"{Path.AltDirectorySeparatorChar}{ph.areaName}{Path.AltDirectorySeparatorChar}{ph.pagePath}{Path.AltDirectorySeparatorChar}{ph.handlerName}";
          break;
        default: throw new Exception("Authorization Context is not Controller or RazorPage.");
      }

      var hasAuthorizeAttribute = controllerOrPageInfo.HasAuthorizeAttribute() || actionOrHandlerInfo.HasAuthorizeAttribute();
      if (!hasAuthorizeAttribute) return;

      if (!context.HttpContext.User.Identity.IsAuthenticated) {
        context.Result = new UnauthorizedResult();
        return;
      }
      var userName = context.HttpContext.User.Identity.Name;
      var roles = await (
          from user in dbContext.Users
          join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
          join role in dbContext.Roles on userRole.RoleId equals role.Id
          where user.UserName == userName
          select role
      ).ToListAsync();
      foreach (var role in roles) {
        if (role.Access == null) continue;
        var accessList1 = JsonConvert.DeserializeObject<IEnumerable<xMvcControllerInfo>>(role.Access);
        if (accessList1.SelectMany(c => c.Actions).Any(a => a.Id == actionDescriptorId)) return;
        var accessList2 = JsonConvert.DeserializeObject<IEnumerable<xMvcRazorPageInfo>>(role.Access);
        if (accessList2.SelectMany(c => c.Handlers).Any(a => a.Id == actionDescriptorId)) return;
      }
      context.Result = new ForbidResult();
    }

  }
}
