using System.Linq;
using System.Reflection;

namespace System {
  public static class TypeExtensions {

    public static T[] GetAttributes<T>(this Type type, bool inherit = true) where T : Attribute {
      var attrs = type.GetCustomAttributes(typeof(T), inherit);
      var arr = new T[attrs.Length];
      for (var i = 0; i < attrs.Length; i++) {
        arr[i] = (T)attrs[i];
      }
      return arr;
    }

    public static TField GetFieldValueAs<T, TField>(this Type type, string fieldName, T obj) => (TField)type.GetField(fieldName).GetValue(obj);
    public static TProperty GetPropertyValueAs<T, TProperty>(this Type type, string propertyName, T obj) => (TProperty)type.GetProperty(propertyName).GetValue(obj);

    public static PropertyInfo[] GetInstanceStaticProperties(this Type type) => type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    public static MemberInfo GetMemberInfo(this Type type, string name) => type.GetMembers().FirstOrDefault(x => x.Name == name);
    public static MethodInfo GetMethodInfo(this Type type, string name) => type.GetMethods().FirstOrDefault(x => x.Name == name);
    public static ParameterInfo[] GetMethodInfoParameters(this Type type, string name) => GetMethodInfo(type, name).GetParameters();

    public static MemberInfo[] GetStaticMembers(this Type type, string name) => type.GetMember(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

    public static bool IsNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;
    public static bool IsNullable2(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    public static bool IsNullableEnum(this Type _type) => Nullable.GetUnderlyingType(_type)?.IsEnum ?? false;

    public static bool IsSameOrParentOf(this Type parent, Type child) {
      if (parent == null)
        throw new ArgumentNullException(nameof(parent));
      if (child == null)
        throw new ArgumentNullException(nameof(child));
      if (parent == child || child.IsEnum && Enum.GetUnderlyingType(child) == parent || child.IsSubclassOf(parent)) {
        return true;
      }
      if (parent.IsGenericTypeDefinition)
        for (var t = child; t != typeof(object) && t != null; t = t.BaseType)
          if (t.IsGenericType && t.GetGenericTypeDefinition() == parent)
            return true;
      if (parent.IsInterface) {
        foreach (var t in child.GetInterfaces()) {
          if (parent.IsGenericTypeDefinition) {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == parent)
              return true;
          } else if (t == parent)
            return true;
        }
      }
      return false;
    }

    public static Type ToNullableUnderlying(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

    public static Type ToUnderlying(this Type type) => type.IsNullable() ? type.GetGenericArguments()[0] : type.IsEnum ? Enum.GetUnderlyingType(type) : type;

    public static TypeCode TypeCode(this Type type) => Type.GetTypeCode(type);

    public static Type UnwrapEnumType(this Type type) => type.IsEnum ? Enum.GetUnderlyingType(type) : type;

    public static Type UnwrapNullableType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

  }
}