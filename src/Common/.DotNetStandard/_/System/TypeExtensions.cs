using Common.Constants;
using Common.Dictionaries;
using Common.Helpers;
using Common.Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace System {
  public static class TypeExtensions {

    public static bool CanConvertTo(this Type fromType, Type toType) {
      if (fromType == toType) return true;
      if (TypeCastDictionary.Instance.ContainsKey(toType) && TypeCastDictionary.Instance[toType].Contains(fromType)) return true;
      if (toType.IsAssignableFrom(fromType)) return true;
      var tc = TypeDescriptor.GetConverter(fromType);
      if (tc.CanConvertTo(toType)) return true;
      tc = TypeDescriptor.GetConverter(toType);
      if (tc.CanConvertFrom(fromType)) return true;
      if (fromType.GetMethods().Any(m => m.IsStatic && m.IsPublic && m.ReturnType == toType && (m.Name == "op_Implicit" || m.Name == "op_Explicit"))) return true;
      return false;
    }

    public static T[] GetAttributes<T>(this Type type) where T : Attribute {
      if (type == null) throw new ArgumentNullException(nameof(type));
      if (!CacheHelper<T>.TypeAttributes.TryGetValue(type, out var attrs)) {
        var list = new List<object>();
        type.GetAttributesInternal(list);
        CacheHelper<T>.TypeAttributes[type] = attrs = list.OfType<T>().ToArray();
      }
      return attrs;
    }

    public static void GetAttributesInternal(this Type type, List<object> list) {
      var _typeAttributesTopInternal = new ConcurrentDictionary<Type, object[]>();
      if (_typeAttributesTopInternal.TryGetValue(type, out var attrs)) {
        list.AddRange(attrs);
      } else {
        type.GetAttributesTreeInternal(list);
        _typeAttributesTopInternal[type] = list.ToArray();
      }
    }

    public static void GetAttributesTreeInternal(this Type type, List<object> list) {
      var _typeAttributesInternal = new ConcurrentDictionary<Type, object[]>();
      var attrs = _typeAttributesInternal.GetOrAdd(type, x => type.GetCustomAttributes(false));
      list.AddRange(attrs);
      if (type.IsInterface)
        return;
      var interfaces = type.GetInterfaces();
      var nBaseInterfaces = type.BaseType != null ? type.BaseType.GetInterfaces().Length : 0;
      for (var i = 0; i < interfaces.Length; i++) {
        var intf = interfaces[i];
        if (i < nBaseInterfaces) {
          var getAttr = false;
          foreach (var mi in type.GetInterfaceMap(intf).TargetMethods) {
            // Check if the interface is reimplemented.
            if (mi.DeclaringType == type) {
              getAttr = true;
              break;
            }
          }
          if (!getAttr) continue;
        }
        intf.GetAttributesTreeInternal(list);
      }
      if (type.BaseType != null && type.BaseType != typeof(object))
        type.BaseType.GetAttributesTreeInternal(list);
    }

    public static object GetDefaultValue(this Type type) {
      var dtype = typeof(GetDefaultValueHelper<>).MakeGenericType(type);
      var helper = (IGetDefaultValueHelper)Activator.CreateInstance(dtype);
      return helper.GetDefaultValue();
    }

    public static Type[]? GetTypeCast(this Type type) => TypeCastDictionary.Instance.TryGetValue(type, out var result) ? result : null;

    public static bool IsFloatTypeCode(this Type type) => TypeCodeConstants.Float.Contains(Type.GetTypeCode(type.IsNullable() ? type.GetGenericArguments()[0] : type));

    public static bool IsIntegerTypeCode(this Type type) => TypeCodeConstants.Integer.Contains(Type.GetTypeCode(type.IsNullable() ? type.GetGenericArguments()[0] : type));

    public static bool IsNumericTypeCode(this Type type) => Type.GetTypeCode(type) == TypeCode.Object ? Nullable.GetUnderlyingType(type).IsNumeric() : TypeCodeConstants.Numeric.Contains(type.TypeCode());

    public static bool IsTextTypeCode(this Type type) => TypeCodeConstants.Text.Contains(type.TypeCode());

  }
}
