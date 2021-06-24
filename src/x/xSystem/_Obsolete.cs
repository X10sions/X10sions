#nullable enable

namespace System.Reflection {
  public static class _ObsoleteExtensions {

    [Obsolete("Try remove this")] public static bool Obsolete_EqualsTo(this MemberInfo member1, MemberInfo member2, Type declaringType = null) {
      if (ReferenceEquals(member1, member2)) return true;
      if (member1 == null || member2 == null) return false;
      if (member1.Name == member2.Name) {
        if (member1.DeclaringType == member2.DeclaringType) return true;
        if (member1 is PropertyInfo) {
          var isSubclass = member1.DeclaringType.IsSameOrParentOf(member2.DeclaringType) || member2.DeclaringType.IsSameOrParentOf(member1.DeclaringType);
          if (isSubclass) return true;
          if (declaringType != null && member2.DeclaringType.IsInterface) {
            var getter1 = ((PropertyInfo)member1).GetGetMethod();
            var getter2 = ((PropertyInfo)member2).GetGetMethod();
            var map = declaringType.GetInterfaceMap(member2.DeclaringType);
            for (var i = 0; i < map.InterfaceMethods.Length; i++)
              if (getter2.Name == map.InterfaceMethods[i].Name && getter2.DeclaringType == map.InterfaceMethods[i].DeclaringType &&
                getter1.Name == map.TargetMethods[i].Name && getter1.DeclaringType == map.TargetMethods[i].DeclaringType)
                return true;
          }
        }
      }
      if (member2.DeclaringType.IsInterface && !member1.DeclaringType.IsInterface && member1.Name.EndsWith(member2.Name, StringComparison.Ordinal) && member1 is PropertyInfo) {
        var isSubclass = member2.DeclaringType.IsAssignableFrom(member1.DeclaringType);
        if (isSubclass) {
          var getter1 = ((PropertyInfo)member1).GetGetMethod();
          var getter2 = ((PropertyInfo)member2).GetGetMethod();
          var map = member1.DeclaringType.GetInterfaceMap(member2.DeclaringType);
          for (var i = 0; i < map.InterfaceMethods.Length; i++) {
            var imi = map.InterfaceMethods[i];
            var tmi = map.TargetMethods[i];
            if ((getter2 == null || (getter2.Name == imi.Name && getter2.DeclaringType == imi.DeclaringType)) &&
                (getter1 == null || (getter1.Name == tmi.Name && getter1.DeclaringType == tmi.DeclaringType))) {
              return true;
            }
          }
        }
      }
      return false;
    }

    [Obsolete("Try remove this")] public static T[] Obsolete_xGetAttributes<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute {
      var attrs = memberInfo.GetCustomAttributes(typeof(T), inherit);
      var arr = new T[attrs.Length];
      for (var i = 0; i < attrs.Length; i++) {
        arr[i] = (T)attrs[i];
      }
      return arr;
    }

    [Obsolete("Try remove this")] public static Type Obsolete_GetMemberType(this MemberInfo memberInfo) {
      switch (memberInfo.MemberType) {
        case MemberTypes.Property: return ((PropertyInfo)memberInfo).PropertyType;
        case MemberTypes.Field: return ((FieldInfo)memberInfo).FieldType;
        case MemberTypes.Method: return ((MethodInfo)memberInfo).ReturnType;
        case MemberTypes.Constructor: return memberInfo.DeclaringType;
      }
      throw new InvalidOperationException();
    }

    [Obsolete("Try remove this")] public static bool Obsolete_HasAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit) where TAttribute : Attribute => memberInfo.Obsolete_HasAttribute(typeof(TAttribute), inherit);

    [Obsolete("Try remove this")] public static bool Obsolete_HasAttribute(this MemberInfo memberInfo, Type attributeType, bool inherit) => Attribute.IsDefined(memberInfo, attributeType, inherit);

    [Obsolete("Try remove these")] public static Type Obsolete_GetMemberType01(this MemberInfo memberInfo) => (memberInfo as PropertyInfo)?.PropertyType ?? ((FieldInfo)memberInfo)?.FieldType;

    [Obsolete("Use default(type) instead.")] public static object GetDefaultValue(this Type type) => DefaultValueDictionary.Instance.TryGetValue(type, out var result) ? result : null;

  }
}
