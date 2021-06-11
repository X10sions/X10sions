using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace System.Data.Linq {
  public static class DBConvert {
    private static Type[] StringArg = new Type[1]    {
    typeof(string)
    };

    public static T ChangeType<T>(object value) => (T)ChangeType(value, typeof(T));

    public static object ChangeType(object value, Type type) {
      if (value == null) {
        return null;
      }
      var nonNullableType = TypeSystem.GetNonNullableType(type);
      var type2 = value.GetType();
      if (nonNullableType.IsAssignableFrom(type2)) {
        return value;
      }
      Guid guid;
      if (nonNullableType == typeof(Binary)) {
        if (type2 == typeof(byte[])) {
          return new Binary((byte[])value);
        }
        if (type2 == typeof(Guid)) {
          guid = (Guid)value;
          return new Binary(guid.ToByteArray());
        }
        var binaryFormatter = new BinaryFormatter();
        var value2 = default(byte[]);
        using (var memoryStream = new MemoryStream()) {
          binaryFormatter.Serialize(memoryStream, value);
          value2 = memoryStream.ToArray();
        }
        return new Binary(value2);
      }
      if (nonNullableType == typeof(byte[])) {
        if (type2 == typeof(Binary)) {
          return ((Binary)value).ToArray();
        }
        if (type2 == typeof(Guid)) {
          guid = (Guid)value;
          return guid.ToByteArray();
        }
        var binaryFormatter2 = new BinaryFormatter();
        using (var memoryStream2 = new MemoryStream()) {
          binaryFormatter2.Serialize(memoryStream2, value);
          return memoryStream2.ToArray();
        }
      }
      if (type2 == typeof(byte[])) {
        if (nonNullableType == typeof(Guid)) {
          return new Guid((byte[])value);
        }
        var binaryFormatter3 = new BinaryFormatter();
        using (var serializationStream = new MemoryStream((byte[])value)) {
          return ChangeType(binaryFormatter3.Deserialize(serializationStream), nonNullableType);
        }
      }
      if (type2 == typeof(Binary)) {
        if (nonNullableType == typeof(Guid)) {
          return new Guid(((Binary)value).ToArray());
        }
        var binaryFormatter4 = new BinaryFormatter();
        using (var serializationStream2 = new MemoryStream(((Binary)value).ToArray(), false)) {
          return ChangeType(binaryFormatter4.Deserialize(serializationStream2), nonNullableType);
        }
      }
      if (nonNullableType.IsEnum) {
        if (type2 == typeof(string)) {
          var value3 = ((string)value).Trim();
          return Enum.Parse(nonNullableType, value3);
        }
        return Enum.ToObject(nonNullableType, Convert.ChangeType(value, Enum.GetUnderlyingType(nonNullableType), CultureInfo.InvariantCulture));
      }
      if (type2.IsEnum) {
        if (nonNullableType == typeof(string)) {
          return Enum.GetName(type2, value);
        }
        return Convert.ChangeType(Convert.ChangeType(value, Enum.GetUnderlyingType(type2), CultureInfo.InvariantCulture), nonNullableType, CultureInfo.InvariantCulture);
      }
      DateTimeOffset dateTimeOffset;
      if (nonNullableType == typeof(TimeSpan)) {
        if (type2 == typeof(string)) {
          return TimeSpan.Parse(value.ToString(), CultureInfo.InvariantCulture);
        }
        if (type2 == typeof(DateTime)) {
          return DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture).TimeOfDay;
        }
        if (type2 == typeof(DateTimeOffset)) {
          dateTimeOffset = DateTimeOffset.Parse(value.ToString(), CultureInfo.InvariantCulture);
          return dateTimeOffset.TimeOfDay;
        }
        return new TimeSpan((long)Convert.ChangeType(value, typeof(long), CultureInfo.InvariantCulture));
      }
      if (type2 == typeof(TimeSpan)) {
        TimeSpan timeSpan;
        if (nonNullableType == typeof(string)) {
          timeSpan = (TimeSpan)value;
          return timeSpan.ToString("", CultureInfo.InvariantCulture);
        }
        if (nonNullableType == typeof(DateTime)) {
          return default(DateTime).Add((TimeSpan)value);
        }
        if (nonNullableType == typeof(DateTimeOffset)) {
          return default(DateTimeOffset).Add((TimeSpan)value);
        }
        timeSpan = (TimeSpan)value;
        return Convert.ChangeType(timeSpan.Ticks, nonNullableType, CultureInfo.InvariantCulture);
      }
      if (nonNullableType == typeof(DateTime) && type2 == typeof(DateTimeOffset)) {
        dateTimeOffset = (DateTimeOffset)value;
        return dateTimeOffset.DateTime;
      }
      if (nonNullableType == typeof(DateTimeOffset) && type2 == typeof(DateTime)) {
        return new DateTimeOffset((DateTime)value);
      }
      if (nonNullableType == typeof(string) && !typeof(IConvertible).IsAssignableFrom(type2)) {
        if (type2 == typeof(char[])) {
          return new string((char[])value);
        }
        return value.ToString();
      }
      if (type2 == typeof(string)) {
        if (nonNullableType == typeof(Guid)) {
          return new Guid((string)value);
        }
        if (nonNullableType == typeof(char[])) {
          return ((string)value).ToCharArray();
        }
        if (nonNullableType == typeof(XDocument) && (string)value == string.Empty) {
          return new XDocument();
        }
        MethodInfo method;
        if (!typeof(IConvertible).IsAssignableFrom(nonNullableType) && (method = nonNullableType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, StringArg, null)) != null) {
          try {
            return SecurityUtils.MethodInfoInvoke(method, null, new object[1]
            {
            value
            });
          } catch (TargetInvocationException ex) {
            throw ex.GetBaseException();
          }
        }
        return Convert.ChangeType(value, nonNullableType, CultureInfo.InvariantCulture);
      }
      if (!nonNullableType.IsGenericType || !(nonNullableType.GetGenericTypeDefinition() == typeof(IQueryable<>)) || !typeof(IEnumerable<>).MakeGenericType(nonNullableType.GetGenericArguments()[0]).IsAssignableFrom(type2)) {
        try {
          return Convert.ChangeType(value, nonNullableType, CultureInfo.InvariantCulture);
        } catch (InvalidCastException) {
          throw System.Data.Linq.Error.CouldNotConvert(type2, nonNullableType);
        }
      }
      return ((IEnumerable)value).AsQueryable();
    }
  }

}