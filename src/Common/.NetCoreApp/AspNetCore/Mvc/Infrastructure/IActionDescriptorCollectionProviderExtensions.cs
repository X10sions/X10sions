using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Common.AspNetCore.Mvc.Infrastructure {

  public static class IActionDescriptorCollectionProviderExtensions {

    // https://joonasw.net/view/discovering-actions-and-razor-pages
    // http://jackhiston.com/2017/9/2/aspnet-core-20-repository-overview-action-discovery/

    public static IEnumerable<xMvcControllerActionInfo> GetMvcControllerActions(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var list = actionDescriptorCollectionProvider.ActionDescriptors.ControllerActionDescriptors().Select(a => new xMvcControllerActionInfo {
        ActionDescriptorId = a.Id,
        AttributeRouteTemplate = a.AttributeRouteInfo?.Template,
        DisplayName = a.DisplayName,
        HttpMethods = string.Join(", ", a.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? new string[] { "any" }),
        Name = a.ActionName,
        ControllerInfo = new xMvcControllerInfo {
          Name = a.ControllerName,
          ClassName = a.ControllerTypeInfo.FullName,
        },
        Parameters = a.Parameters?.Select(p => new xMvcActionDescriptorInfo.Parameter {
          TypeName = p.ParameterType.Name,
          Name = p.Name
        }),
        MethodName = a.MethodInfo.Name,
        Filters = a.FilterDescriptors?.Select(f => new xMvcActionDescriptorInfo.Filter {
          ClassName = f.Filter.GetType().FullName,
          Scope = f.Scope //10 = Global, 20 = Controller, 30 = Action
        }),
        Constraints = a.ActionConstraints?.Select(c => new xMvcActionDescriptorInfo.Constraint {
          TypeName = c.GetType().Name
        }),
        RouteValues = a.RouteValues.Select(r => new xMvcActionDescriptorInfo.RouteValue {
          Key = r.Key,
          Value = r.Value
        }),
      });
      return list;
    }

    public static IEnumerable<xMvcRouteInformation> GetAllRouteInformations(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var ret = new List<xMvcRouteInformation>();
      foreach (var actionDescriptor in actionDescriptorCollectionProvider.ActionDescriptors.Items) {
        var info = new xMvcRouteInformation();
        info.SetValues(actionDescriptor);
        // Extract HTTP Verb
        // Special controller path
        if (info.RazorPageViewEnginePath == "/RouteAnalyzer_Main/ShowAllRoutes") {
          info.RazorPageViewEnginePath = string.Empty;
        }
        // Additional information of invocation
        info.DisplayName += $" ({actionDescriptor.DisplayName})";
        // Generating List
        ret.Add(info);
      }
      // Result
      return ret;
    }

    public static IEnumerable<xMvcRazorPageInfo> GetRazorPages(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var list = actionDescriptorCollectionProvider.ActionDescriptors.PageActionDescriptors().Select(a => new xMvcRazorPageInfo {
        AreaName = a.AreaName,
        DisplayName = a.DisplayName,
        ActionDescriptorId = a.Id,
        ViewEnginePath = a.ViewEnginePath,
        RelativePath = a.RelativePath,
      });
      return list;
    }

    public static IEnumerable<xMvcControllerInfo> GetMvcControllers(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var _mvcControllers = new List<xMvcControllerInfo>();
      var items = actionDescriptorCollectionProvider.ActionDescriptors.ControllerActionDescriptors()
          .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
          .ToList();
      foreach (var actionDescriptors in items) {
        if (!actionDescriptors.Any()) continue;
        var actionDescriptor = actionDescriptors.First();
        var controllerTypeInfo = actionDescriptor.ControllerTypeInfo;
        var currentController = new xMvcControllerInfo {
          AreaName = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
          DisplayName = controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
          Name = actionDescriptor.ControllerName,
        };
        var actions = new List<xMvcControllerActionInfo>();
        foreach (var descriptor in actionDescriptors.GroupBy(a => a.ActionName).Select(g => g.First())) {
          var methodInfo = descriptor.MethodInfo;
          if (IsProtectedAction(controllerTypeInfo, methodInfo))
            actions.Add(new xMvcControllerActionInfo {
              ControllerInfo = new xMvcControllerInfo {
                AreaName = currentController.AreaName,
                Name = currentController.Name
              },
              Name = descriptor.ActionName,
              DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
            });
        }
        if (actions.Any()) {
          currentController.Actions = actions;
          _mvcControllers.Add(currentController);
        }
      }
      return _mvcControllers;
    }

    public static IEnumerable<xMvcRazorPageInfo> GetMvcRazorPages(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var _mvcRazorPages = new List<xMvcRazorPageInfo>();
      var items = actionDescriptorCollectionProvider
          .ActionDescriptors.PageActionDescriptors()
          .GroupBy(descriptor => descriptor.GetType().FullName)
          .ToList();
      foreach (var pageDescriptors in items) {
        if (!pageDescriptors.Any())
          continue;
        var pageDescriptor = (CompiledPageActionDescriptor)pageDescriptors.First();
        var pageTypeInfo = pageDescriptor.GetType().GetTypeInfo();
        var currentRazorPage = new xMvcRazorPageInfo {
          AreaName = pageTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
          DisplayName = pageTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? pageDescriptor.DisplayName,
          PageId = pageDescriptor.Id,
          RelativePath = pageDescriptor.RelativePath,
          ViewEnginePath = pageDescriptor.ViewEnginePath
        };
        var handlers = new List<xMvcRazorPageHandlerInfo>();
        foreach (var handlerMethod in pageDescriptor.HandlerMethods) {
          var methodInfo = handlerMethod.MethodInfo;
          if (IsProtectedHandler(pageTypeInfo, methodInfo))
            handlers.Add(new xMvcRazorPageHandlerInfo {
              HttpMethods = handlerMethod.HttpMethod,
              Name = handlerMethod.Name,
              DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
              Parameters = handlerMethod.Parameters.Select(p => new xMvcActionDescriptorInfo.Parameter {
                Name = p.Name,
                TypeName = p.ParameterType.Name
              })
            });
        }
        if (handlers.Any()) {
          currentRazorPage.Handlers = handlers;
          _mvcRazorPages.Add(currentRazorPage);
        }
      }
      return _mvcRazorPages;
    }

    public static IEnumerable<xMvcRazorPageHandlerInfo> GetMvcRazorPageHandlers(this IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) {
      var list = actionDescriptorCollectionProvider.ActionDescriptors.PageActionDescriptors().Select(a => new xMvcRazorPageHandlerInfo {
        ActionDescriptorId = a.Id,
        AttributeRouteTemplate = a.AttributeRouteInfo?.Template,
        DisplayName = a.DisplayName,
        HttpMethods = string.Join(", ", a.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? new string[] { "any" }),
        RazorPageInfo = new xMvcRazorPageInfo {
          AreaName = a.AreaName,
          ClassName = a.GetType().FullName,
          DeclaredModelClassName = (a as CompiledPageActionDescriptor).DeclaredModelTypeInfo.FullName,
          ModelClassName = (a as CompiledPageActionDescriptor).ModelTypeInfo.FullName,
          RelativePath = a.RelativePath,
          ViewEnginePath = a.ViewEnginePath,
        },
        Parameters = a.Parameters?.Select(p => new xMvcActionDescriptorInfo.Parameter {
          TypeName = p.ParameterType.Name,
          Name = p.Name
        }),
        ClassName = (a as CompiledPageActionDescriptor).HandlerTypeInfo.FullName,
        Filters = a.FilterDescriptors?.Select(f => new xMvcActionDescriptorInfo.Filter {
          ClassName = f.Filter.GetType().FullName,
          Scope = f.Scope //10 = Global, 20 = Controller, 30 = Action
        }),
        Constraints = a.ActionConstraints?.Select(c => new xMvcActionDescriptorInfo.Constraint {
          TypeName = c.GetType().Name
        }),
        RouteValues = a.RouteValues.Select(r => new xMvcActionDescriptorInfo.RouteValue {
          Key = r.Key,
          Value = r.Value
        }),
      });
      return list;
    }

    static bool IsProtectedAction(MemberInfo controllerTypeInfo, MemberInfo actionMethodInfo) {
      if (actionMethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true) != null)
        return false;
      if (controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
        return true;
      if (actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
        return true;
      return false;
    }

    static bool IsProtectedHandler(MemberInfo pageTypeInfo, MemberInfo handlerMethodInfo) {
      if (handlerMethodInfo.GetCustomAttribute<NonHandlerAttribute>(true) != null)
        return false;
      if (handlerMethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true) != null)
        return false;
      if (pageTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
        return true;
      if (handlerMethodInfo.GetCustomAttribute<AuthorizeAttribute>(true) != null)
        return true;
      return false;
    }

  }
}