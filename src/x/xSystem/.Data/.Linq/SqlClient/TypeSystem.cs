using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Linq.SqlClient {
  internal static class TypeSystem {
    private static ILookup<string, MethodInfo> _sequenceMethods;

    private static ILookup<string, MethodInfo> _queryMethods;

    internal static bool IsSequenceType(Type seqType) {
      if (seqType != typeof(string) && seqType != typeof(byte[]) && seqType != typeof(char[])) {
        return FindIEnumerable(seqType) != null;
      }
      return false;
    }

    internal static bool HasIEnumerable(Type seqType) => FindIEnumerable(seqType) != null;

    private static Type FindIEnumerable(Type seqType) {
      if (seqType == null || seqType == typeof(string)) {
        return null;
      }
      if (seqType.IsArray) {
        return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
      }
      if (seqType.IsGenericType) {
        foreach (var type in seqType.GetGenericArguments()) {
          var type2 = typeof(IEnumerable<>).MakeGenericType(type);
          if (type2.IsAssignableFrom(seqType)) {
            return type2;
          }
        }
      }
      var interfaces = seqType.GetInterfaces();
      if (interfaces != null && interfaces.Length != 0) {
        foreach (var seqType2 in interfaces) {
          var type3 = FindIEnumerable(seqType2);
          if (type3 != null) {
            return type3;
          }
        }
      }
      if (seqType.BaseType != null && seqType.BaseType != typeof(object)) {
        return FindIEnumerable(seqType.BaseType);
      }
      return null;
    }

    internal static Type GetFlatSequenceType(Type elementType) {
      var type = FindIEnumerable(elementType);
      if (type != null) {
        return type;
      }
      return typeof(IEnumerable<>).MakeGenericType(elementType);
    }

    internal static Type GetSequenceType(Type elementType) => typeof(IEnumerable<>).MakeGenericType(elementType);

    internal static Type GetElementType(Type seqType) {
      var type = FindIEnumerable(seqType);
      if (type == null) {
        return seqType;
      }
      return type.GetGenericArguments()[0];
    }

    internal static bool IsNullableType(Type type) {
      if (type != null && type.IsGenericType) {
        return type.GetGenericTypeDefinition() == typeof(Nullable<>);
      }
      return false;
    }

    internal static bool IsNullAssignable(Type type) {
      if (type.IsValueType) {
        return IsNullableType(type);
      }
      return true;
    }

    internal static Type GetNonNullableType(Type type) {
      if (IsNullableType(type)) {
        return type.GetGenericArguments()[0];
      }
      return type;
    }

    internal static Type GetMemberType(MemberInfo mi) {
      var fieldInfo = mi as FieldInfo;
      if (fieldInfo != null) {
        return fieldInfo.FieldType;
      }
      var propertyInfo = mi as PropertyInfo;
      if (propertyInfo != null) {
        return propertyInfo.PropertyType;
      }
      var eventInfo = mi as EventInfo;
      if (eventInfo != null) {
        return eventInfo.EventHandlerType;
      }
      return null;
    }

    internal static IEnumerable<FieldInfo> GetAllFields(Type type, BindingFlags flags) {
      var dictionary = new Dictionary<MetaPosition, FieldInfo>();
      var type2 = type;
      do {
        foreach (var fieldInfo in type2.GetFields(flags)) {
          if (fieldInfo.IsPrivate || type == type2) {
            var key = new MetaPosition(fieldInfo);
            dictionary[key] = fieldInfo;
          }
        }
        type2 = type2.BaseType;
      }
      while (type2 != null);
      return dictionary.Values;
    }

    internal static IEnumerable<PropertyInfo> GetAllProperties(Type type, BindingFlags flags) {
      var dictionary = new Dictionary<MetaPosition, PropertyInfo>();
      var type2 = type;
      do {
        foreach (var propertyInfo in type2.GetProperties(flags)) {
          if (type == type2 || IsPrivate(propertyInfo)) {
            var key = new MetaPosition(propertyInfo);
            dictionary[key] = propertyInfo;
          }
        }
        type2 = type2.BaseType;
      }
      while (type2 != null);
      return dictionary.Values;
    }

    private static bool IsPrivate(PropertyInfo pi) {
      var methodInfo = pi.GetGetMethod() ?? pi.GetSetMethod();
      if (methodInfo != null) {
        return methodInfo.IsPrivate;
      }
      return true;
    }

    internal static MethodInfo FindSequenceMethod(string name, Type[] args, params Type[] typeArgs) {
      if (_sequenceMethods == null) {
        _sequenceMethods = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup((MethodInfo m) => m.Name);
      }
      var methodInfo = _sequenceMethods[name].FirstOrDefault((MethodInfo m) => ArgsMatchExact(m, args, typeArgs));
      if (methodInfo == null) {
        return null;
      }
      if (typeArgs != null) {
        return methodInfo.MakeGenericMethod(typeArgs);
      }
      return methodInfo;
    }

    internal static MethodInfo FindSequenceMethod(string name, IEnumerable sequence) => FindSequenceMethod(name, new Type[1]
      {
      sequence.GetType()
      }, GetElementType(sequence.GetType()));

    internal static MethodInfo FindQueryableMethod(string name, Type[] args, params Type[] typeArgs) {
      if (_queryMethods == null) {
        _queryMethods = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).ToLookup((MethodInfo m) => m.Name);
      }
      var methodInfo = _queryMethods[name].FirstOrDefault((MethodInfo m) => ArgsMatchExact(m, args, typeArgs));
      if (methodInfo == null) {
        throw Error.NoMethodInTypeMatchingArguments(typeof(Queryable));
      }
      if (typeArgs != null) {
        return methodInfo.MakeGenericMethod(typeArgs);
      }
      return methodInfo;
    }

    internal static MethodInfo FindStaticMethod(Type type, string name, Type[] args, params Type[] typeArgs) {
      var methodInfo = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(delegate (MethodInfo m) {
        if (m.Name == name) {
          return ArgsMatchExact(m, args, typeArgs);
        }
        return false;
      });
      if (methodInfo == null) {
        throw Error.NoMethodInTypeMatchingArguments(type);
      }
      if (typeArgs != null) {
        return methodInfo.MakeGenericMethod(typeArgs);
      }
      return methodInfo;
    }

    private static bool ArgsMatchExact(MethodInfo m, Type[] argTypes, Type[] typeArgs) {
      var parameters = m.GetParameters();
      if (parameters.Length != argTypes.Length) {
        return false;
      }
      if (!m.IsGenericMethodDefinition && m.IsGenericMethod && m.ContainsGenericParameters) {
        m = m.GetGenericMethodDefinition();
      }
      if (m.IsGenericMethodDefinition) {
        if (typeArgs == null || typeArgs.Length == 0) {
          return false;
        }
        if (m.GetGenericArguments().Length != typeArgs.Length) {
          return false;
        }
        m = m.MakeGenericMethod(typeArgs);
        parameters = m.GetParameters();
      } else if (typeArgs != null && typeArgs.Length != 0) {
        return false;
      }
      var i = 0;
      for (var num = argTypes.Length; i < num; i++) {
        var parameterType = parameters[i].ParameterType;
        if (parameterType == null) {
          return false;
        }
        var c = argTypes[i];
        if (!parameterType.IsAssignableFrom(c)) {
          return false;
        }
      }
      return true;
    }

    internal static bool IsSimpleType(Type type) {
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
        type = type.GetGenericArguments()[0];
      }
      if (type.IsEnum) {
        return true;
      }
      if (!(type == typeof(Guid))) {
        switch (Type.GetTypeCode(type)) {
          case TypeCode.Boolean:
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
          case TypeCode.Decimal:
          case TypeCode.DateTime:
          case TypeCode.String:
            return true;
          case TypeCode.Object:
            if (!(typeof(TimeSpan) == type)) {
              return typeof(DateTimeOffset) == type;
            }
            return true;
          default:
            return false;
        }
      }
      return true;
    }
  }
}
