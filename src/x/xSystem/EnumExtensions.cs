using System.Collections.Generic;
using System.Linq;

namespace System {
  public static class EnumExtensions {

    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute {
      var type = enumValue.GetType();
      var name = Enum.GetName(type, enumValue);
      return type.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();
    }

    public static IEnumerable<T> GetValuesCast<T>() where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(typeof(T)).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>() where T : Enum => Enum.GetValues(typeof(T)).OfType<T>();

    public static IEnumerable<T> GetValuesCast<T>(this T enumObj) where T : struct, IConvertible, IComparable, IFormattable => Enum.GetValues(enumObj.GetType()).Cast<T>();
    public static IEnumerable<T> GetValuesOfType<T>(this T enumObj) where T : Enum => Enum.GetValues(enumObj.GetType()).OfType<T>();

  }
}