using System.ComponentModel;
using System.Globalization;

namespace RCommon;

public static class ObjectExtensions {
  /// <summary>
  /// Used to simplify and beautify casting an object to a type.
  /// </summary>
  /// <typeparam name="T">Type to be casted</typeparam>
  /// <param name="obj">Object to cast</param>
  /// <returns>Casted object</returns>
  public static T As<T>(this object obj) where T : class => (T)obj;

  /// <summary>
  /// Helper method to determine if two byte arrays are the same value even if they are different object references
  /// </summary>
  /// <param name="binaryValue1">This Object</param>
  /// <param name="binaryValue2">The Object you want to compare against</param>
  /// <returns>true if the two objects are equal</returns>
  public static bool BinaryEquals(this object binaryValue1, object binaryValue2) {
    if (Object.ReferenceEquals(binaryValue1, binaryValue2)) {
      return true;
    }
    byte[] array1 = binaryValue1 as byte[];
    byte[] array2 = binaryValue2 as byte[];
    if (array1 != null && array2 != null) {
      if (array1.Length != array2.Length) {
        return false;
      }
      for (int i = 0; i < array1.Length; i++) {
        if (array1[i] != array2[i]) {
          return false;
        }
      }
      return true;
    }
    return false;
  }

  ///<summary>
  /// Builds a key that from the full name of the type and the supplied user key.
  ///</summary>
  ///<param name="userKey">The user supplied key, if any.</param>
  ///<typeparam name="T">The type for which the key is built.</typeparam>
  ///<returns>string.</returns>
  public static string BuildFullKey<T>(this object userKey) => userKey == null ?  typeof(T).FullName: typeof(T).FullName + userKey;

  /// <summary>
  /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
  /// </summary>
  /// <param name="obj">Object to be converted</param>
  /// <typeparam name="T">Type of the target object</typeparam>
  /// <returns>Converted object</returns>
  public static T To<T>(this object obj)      where T : struct {
    if (typeof(T) == typeof(Guid)) {
      return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
    }
    return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
  }

}
