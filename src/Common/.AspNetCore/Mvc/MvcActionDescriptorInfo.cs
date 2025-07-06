using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.ComponentModel;
using System.Reflection;

namespace Common.AspNetCore.Mvc;

public record MvcActionDescriptorInfo(ActionDescriptor Descriptor) {
  public string ActionDescriptorId => Descriptor.Id;
  public string? DisplayName => Descriptor switch { 
    ControllerActionDescriptor cad => cad.ControllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
    _ => Descriptor.DisplayName
  };

  public string HttpMethods => Descriptor switch {
    CompiledPageActionDescriptor cpad => string.Join(", ", cpad.HandlerMethods.Select(x => x.HttpMethod)),
    _ => string.Join(", ", Descriptor.ActionConstraints?.OfType<HttpMethodActionConstraint>().SingleOrDefault()?.HttpMethods ?? ["any"])
  };

  public string? AttributeRouteTemplate => Descriptor.AttributeRouteInfo?.Template;

  public IEnumerable<Parameter> Parameters => Descriptor.Parameters?.Select(p => new Parameter(p)) ?? [];
  public IEnumerable<Filter> Filters => Descriptor.FilterDescriptors?.Select(f => new Filter(f.Filter.GetType().FullName, f.Scope)) ?? [];
  public IEnumerable<Constraint> Constraints => Descriptor.ActionConstraints?.Select(c => new Constraint(c.GetType().Name)) ?? [];
  public IEnumerable<RouteValue> RouteValues => Descriptor.RouteValues.Select(r => new RouteValue(r.Key, r.Value)) ?? [];

  public record Parameter(ParameterDescriptor Descriptor) {
    public string Name => Descriptor.Name;
    public string TypeName => Descriptor.ParameterType.Name;
  }

  public record Filter(string ClassName, int Scope);
  public record Constraint(string TypeName);
  public record RouteValue(string Key, string? Value);

}


public record MvcControllerActionInfo(ControllerActionDescriptor ControllerDescriptor, MvcControllerInfo ControllerInfo) : MvcActionDescriptorInfo(ControllerDescriptor) {
  public MvcControllerActionInfo(ControllerActionDescriptor Descriptor) : this(Descriptor, new(Descriptor)) { }
  public string Name => ControllerDescriptor.ActionName;
  public string Id => $"{ControllerId}:{Name}";
  public string ControllerId => ControllerInfo.Id;
  public string MethodName => ControllerDescriptor.MethodInfo.Name;
}

public record MvcControllerInfo(ControllerActionDescriptor ControllerDescriptor) : MvcActionDescriptorInfo(ControllerDescriptor) {
  public string Name => ControllerDescriptor.ControllerName;
  public string? ClassName => ControllerDescriptor.ControllerTypeInfo.FullName;
  public string? AreaName => ControllerDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
  public string Id => $"{AreaName}:{Name}";
}

public record MvcRazorPageHandlerInfo(PageActionDescriptor PageDescriptor, MvcRazorPageInfo RazorPageInfo) : MvcActionDescriptorInfo(PageDescriptor) {
  public MvcRazorPageHandlerInfo(PageActionDescriptor PageDescriptor, HandlerMethodDescriptor handlerMethod) :this(PageDescriptor, new MvcRazorPageInfo(PageDescriptor)) { 
  }

  //public MvcRazorPageHandlerInfo(CompiledPageActionDescriptor compiledPageActionDescriptor):this(compiledPageActionDescriptor) {
  //  compiledPageActionDescriptor.HandlerMethods.meth
  //}
  //public string Name => PageDescriptor.
  //public string Id => $"{RazorPageId}:{Name}";
  public string RazorPageId => RazorPageInfo.Id;
  public string ClassName => (PageDescriptor as CompiledPageActionDescriptor).HandlerTypeInfo.FullName;
}

public record MvcRazorPageInfo(PageActionDescriptor PageDescriptor) : MvcActionDescriptorInfo(PageDescriptor) {
  public string Id => $"{AreaName}:{ViewEnginePath}";
  public string? AreaName => PageDescriptor.AreaName;        // eg: /Areas/Identity/Pages
  public string PageId => PageDescriptor.Id;
  public string RelativePath => PageDescriptor.RelativePath;    // eg: /Areas/Identity/Pages/Manage/Accounts.cshtml  //Invocation
  public string ViewEnginePath => PageDescriptor.ViewEnginePath; // eg:                      /Manage/Accounts         //Path
  public IEnumerable<MvcRazorPageHandlerInfo> Handlers { get; set; }
  public string ClassName => PageDescriptor.GetType().Name;
  public string DeclaredModelClassName => (PageDescriptor as CompiledPageActionDescriptor).DeclaredModelTypeInfo.FullName;
  public string ModelClassName => (PageDescriptor as CompiledPageActionDescriptor).ModelTypeInfo.FullName;

}

