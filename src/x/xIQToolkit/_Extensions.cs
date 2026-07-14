using System.Reflection;

namespace IQToolkit {
  public static class _Extensions {

    public static object? xGetValue(this MemberInfo member, object instance) => member switch {
      PropertyInfo pi => pi != null ? pi.GetValue(instance, null) : null,
      FieldInfo fi => fi != null ? fi.GetValue(instance) : null,
      _ => throw new InvalidOperationException()
    };

  }
}
