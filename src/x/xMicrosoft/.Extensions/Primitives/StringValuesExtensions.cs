using System;
namespace Microsoft.Extensions.Primitives {
  public static class StringValuesExtensions {

    [Obsolete("Try remove this.")] public static string ToStringFirstOrDefault(this StringValues sv) => sv.Count == 1 ? sv.ToString() : null;

  }
}
