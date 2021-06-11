#nullable enable
using System.ComponentModel;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace System {
  public static class ObjectExtensions {

    //public static T As<T>(this object obj, T defaultValue = default) => (obj is T) ? (T)obj : defaultValue;
    //public static TTo As<TTo>(this object value, TTo defaultValue = default) => value.As<object, TTo>(defaultValue);

    //public static TTo As<TTo>(this object value, TTo defaultValue = default) => value.As<object, TTo>(defaultValue);

    ///public static string IfNullToString(this object value, string valueIfNull) => value == null ? valueIfNull : value.ToString();

    public static TTo As<TFrom, TTo>(this TFrom value, TTo defaultValue = default) {
      try {
        var converter = TypeDescriptor.GetConverter(typeof(TTo));
        if (converter.CanConvertFrom(typeof(TFrom))) {
          return (TTo)converter.ConvertFrom(value);
        }
        converter = TypeDescriptor.GetConverter(typeof(TFrom));
        if (converter.CanConvertTo(typeof(TTo))) {
          return (TTo)converter.ConvertTo(value, typeof(TTo));
        }
        return defaultValue;
      } catch {
        return defaultValue;
      }
    }

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

    public static bool IsDateTime(this object o) => o is DateTime || o is TimeSpan;

    public static bool IsInteger(this object o) => o is short || o is int || o is long;

    public static bool IsNumeric(this object o) =>
      o is byte ||
      o is sbyte ||
      o is short ||
      o is ushort ||
      o is int ||
      o is uint ||
      o is long ||
      o is ulong ||
      o is float ||
      o is double ||
      o is decimal;

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

    public static string GetXmlString<T>(this T obj) {
      using (var textWriter = new StringWriter()) {
        var settings = new XmlWriterSettings() { Indent = true, IndentChars = "  " };
        using (var xmlWriter = XmlWriter.Create(textWriter, settings))
          new XmlSerializer(obj.GetType()).Serialize(xmlWriter, obj);
        return textWriter.ToString();
      }
    }

    #endregion

  }
}