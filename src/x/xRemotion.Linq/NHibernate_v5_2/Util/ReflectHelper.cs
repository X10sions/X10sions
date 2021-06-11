using System;

namespace NHibernate_v5_2.Util {
  public static class ReflectHelper_v5_2 {

    public static T CastOrThrow<T>(this object obj, string supportMessage) where T : class {
      if (obj is T t) return t;
      var typeKind = typeof(T).IsInterface ? "interface" : "class";
      var objType = obj?.GetType().FullName ?? "Object must not be null and";
      throw new ArgumentException($@"{objType} requires to implement {typeof(T).FullName} {typeKind} to support {supportMessage}.");
    }

  }
}
