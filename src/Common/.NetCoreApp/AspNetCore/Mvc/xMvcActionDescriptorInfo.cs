using System.Collections.Generic;

namespace Common.AspNetCore.Mvc {
  public class xMvcActionDescriptorInfo {
    public string ActionDescriptorId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string HttpMethods { get; set; }
    public string AttributeRouteTemplate { get; set; }
    public IEnumerable<Parameter> Parameters { get; set; }
    public IEnumerable<Filter> Filters { get; set; }
    public IEnumerable<Constraint> Constraints { get; set; }
    public IEnumerable<RouteValue> RouteValues { get; set; }

    public class Parameter {
      public string Name { get; set; }
      public string TypeName { get; set; }
    }

    public class Filter {
      public string ClassName { get; set; }
      public int Scope { get; set; }
    }

    public class Constraint {
      public string TypeName { get; set; }
    }

    public class RouteValue {
      public string Key { get; set; }
      public string Value { get; set; }
    }

  }
}
