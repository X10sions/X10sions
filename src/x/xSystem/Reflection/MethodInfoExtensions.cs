namespace System.Reflection {

  public static class MethodInfoExtensions {

    public static PropertyInfo? GetInstanceStaticPropertyInfo(this MethodInfo method) {
      if (method != null) {
        var type = method.DeclaringType;
        foreach (var info in type.GetInstanceStaticProperties()) {
          if (info.CanRead && method == info.GetGetMethod(true)) {
            return info;
          }
          if (info.CanWrite && method == info.GetSetMethod(true)) {
            return info;
          }
        }
      }
      return null;
    }

    public static PropertyInfo? GetPropertyInfo(this MethodInfo method) {
      if (method != null) {
        var type = method.DeclaringType;
        foreach (var info in type.GetProperties()) {
          if (info.CanRead && method == info.GetGetMethod(true))
            return info;
          if (info.CanWrite && method == info.GetSetMethod(true))
            return info;
        }
      }
      return null;
    }

  }

}