using System.Collections;
using System.Data;
using System.Data.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Xml;

namespace System {
  public static class TypeExtensions {

    public static Expression<Func<string, IDbConnection>> CreateConnectionExpression(this Type @this) {
      var p = Expression.Parameter(typeof(string));
      return Expression.Lambda<Func<string, IDbConnection>>(Expression.New(@this.GetConstructor(new Type[] {
        typeof(string)
      }), p), new ParameterExpression[] {
        p
      });
    }

    public static object GetNullValue(this Type type) {
      var getValue = Expression.Lambda<Func<object>>(Expression.Convert(Expression.Field(null, type, "Null"), typeof(object)));
      return getValue.Compile()();
    }

    public static bool IsEnumerable(this Type type) => type.GetInterface(nameof(IEnumerable)) != null;

    public static bool IsScalar(this Type type, bool checkArrayElementType = true) {
      while (checkArrayElementType && type.IsArray) { type = type.GetElementType(); }
      return type.IsValueType
        || type == typeof(string)
        || type == typeof(Binary)
        || type == typeof(Stream)
        || type == typeof(XmlReader)
        || type == typeof(XmlDocument);
    }

  }
}