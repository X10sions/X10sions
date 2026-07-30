using System.Dynamic;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace System;
public static class ObjectExtensions {

  public static bool IsNull(this object value) => value is null;
  //public static bool IsTrue(this bool value) => value;

  ///public static string IfNullToString(this object value, string valueIfNull) => value == null ? valueIfNull : value.ToString();

  //public static T? ToEnum<T>(this object? value) where T : struct {
  //  var type = typeof(T);
  //  T? typedValue = value switch {
  //    null => null,
  //    T t => t,
  //    string s => s.ToEnum<T>(),
  //    object o => Enum.IsDefined(type, o) ? (T)Enum.ToObject(type, o) : throw new NotImplementedException($"{value}: {value.GetType()}")
  //  };
  //  return typedValue;
  //  //if (value == null) return null;
  //  //if (value is T) return (T)value;
  //  //var type = typeof(T);
  //  //if (Enum.IsDefined(type, value)) {
  //  //  return (T)Enum.ToObject(type, value);
  //  //}
  //  //return value.ToString().ToEnum<T>() ?? throw new NotImplementedException($"{value}: {value.GetType()}");
  //}

  public static object MergeToExpandoObject(this object item1, object item2) {
    //
    var dictionary1 = (IDictionary<string, object>)item1;
    var dictionary2 = (IDictionary<string, object>)item2;
    var result = new ExpandoObject();
    var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary
    foreach (var pair in dictionary1.Concat(dictionary2)) {
      d[pair.Key] = pair.Value;
    }
    return result;
  }

  //    public static bool IsEnumerable(this object o) => o is IEnumerable;
  //     public static bool IsEnumerableType(this object o) => o.GetType().IsEnumerable();

  public static bool IsDateTime(this object o) => o is DateTime or TimeSpan;

  public static bool IsInteger(this object o) => o is short or int or long;

  public static bool IsNumeric(this object o) => o is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal;

  #region "BinaryFormatterExtensions"

  public static byte[] ToByteArray(this object obj) {
    if (obj == null) {
      return new byte[] { };
      // return null;
    }
    var binaryFormatter = new BinaryFormatter();
    using (var memoryStream = new MemoryStream()) {
      binaryFormatter.Serialize(memoryStream, obj);
      return memoryStream.ToArray();
    }
  }

  public static string ToBase64String<T>(this T obj) {
    using (var stream = new MemoryStream()) {
      new BinaryFormatter().Serialize(stream, obj);
      return Convert.ToBase64String(stream.GetBuffer(), 0, checked((int)stream.Length)); // Throw an exception on overflow.
    }
  }

  #endregion

  #region "XmlSerializerExtensions"

  public static string GetXmlString<T>(this T obj) where T : notnull {
    using (var textWriter = new StringWriter()) {
      var settings = new XmlWriterSettings() { Indent = true, IndentChars = "  " };
      using (var xmlWriter = XmlWriter.Create(textWriter, settings))
        new XmlSerializer(obj.GetType()).Serialize(xmlWriter, obj);
      return textWriter.ToString();
    }
  }

  #endregion

  public static T ThrowIfNull<T>(this T? value, string? message = null) => value is null ? throw new ArgumentNullException(message ?? "Value cannot be null.") : value;
  public static T? ThrowIfNotNull<T>(this T? value, string? message = null) => value != null ? throw new ArgumentNullException(message ?? "Value must be null.") : value;

  //public static T To<T>(this object? value) => value.To<object?, T>();

  /// <summary>Convert to specified type TTo with default value support.</summary>
  //public static TTo To<T, TTo>(this T? value, TTo defaultValue = default!) {
  //  if (value is null || (value is string str && string.IsNullOrWhiteSpace(str))) return defaultValue;
  //  if (value is TTo already) return already;
  //  try {
  //    Type toType = typeof(TTo);
  //    // 1. Enum support
  //    if (toType.IsEnum) {
  //      if (value is string s)
  //        return (TTo)Enum.Parse(toType, s, ignoreCase: true);
  //      return (TTo)Enum.ToObject(toType, value);
  //    }
  //    Type coreType = Nullable.GetUnderlyingType(toType) ?? toType;
  //    if (coreType == typeof(bool)) {
  //      var result = value.ToString().ToBooleanNullable(defaultValue as bool?);
  //      return (TTo)(object)result!;
  //    }
  //    // 2. TypeConverter
  //    var converter = TypeDescriptor.GetConverter(toType);
  //    if (converter.CanConvertFrom(typeof(T)))
  //      return (TTo)converter.ConvertFrom(value)!;
  //    // 3. IConvertible fallback
  //    return (TTo)Convert.ChangeType(value, toType, CultureInfo.CurrentCulture);
  //  } catch {
  //    return defaultValue;
  //  }
  //}

  public static bool? ToBooleanNullable(this string? value, bool? defaultValue = null) => value?.Trim() switch {
    null or "" => defaultValue,
    "0" => false,
    "1" => true,
    _ => bool.TryParse(value, out bool result) ? result : defaultValue
  };


}
