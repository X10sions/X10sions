using RCommon.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace RCommon;

public static class ObjectExtensions {


  /// <summary>
  /// Returns the value of a specified property from an object 
  /// using reflection.
  /// </summary>
  /// <param name="sourceObject">The source object from which the property is to be fetched</param>
  /// <param name="propertyName">The name of the property</param>
  /// <returns></returns>
  public static T GetPropertyValueWithReflection<T>(this object sourceObject,
    string propertyName) {
    Guard.Against<ArgumentNullException>(sourceObject == null, "sourceObject cannot be null");
    Guard.Against<ArgumentException>(string.IsNullOrEmpty(propertyName), "propertyName, cannot be null or empty");

    BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    PropertyInfo propertyInfo = sourceObject.GetType().GetProperty(propertyName, eFlags);
    if (propertyInfo == null) {
      throw new ApplicationException("Cannot find property [" + propertyName + "] in object type: " + sourceObject.GetType().Name);
    }
    return (T)propertyInfo.GetValue(sourceObject, null);
  }

  /// <summary>
  /// Sets the value of a specified property for an object 
  /// using reflection.
  /// </summary>
  /// <param name="anObject">the object whose property value will be set</param>
  /// <param name="propertyName">the name of the property</param>
  /// <param name="propertyValue">the value of the property</param>
  public static void SetPropertyValueWithReflection(this object anObject,
    string propertyName, object propertyValue) {
    Guard.Against<ArgumentNullException>(anObject == null, "anObject cannot be null");
    Guard.Against<ArgumentException>(string.IsNullOrEmpty(propertyName), "propertyName, cannot be null or empty");

    BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    PropertyInfo propertyInfo = anObject.GetType().GetProperty(propertyName, eFlags);

    Type dataType = propertyInfo.PropertyType;
    if (!(dataType.IsGenericType && (dataType.GetGenericTypeDefinition() == typeof(Nullable<>)))) {
      if (propertyValue == null) {
        throw new ArgumentNullException("propertyValue");
      }
    }
    if (propertyInfo == null)
      throw new ApplicationException("Cannot find property [" + propertyName + "] in object type: " + anObject.GetType().FullName);

    propertyInfo.SetValue(anObject, propertyValue, null);
  }





 

  /// <summary>
  /// Check if an item is in a list.
  /// </summary>
  /// <param name="item">Item to check</param>
  /// <param name="list">List of items</param>
  /// <typeparam name="T">Type of the items</typeparam>
  public static bool IsIn<T>(this T item, params T[] list) {
    return list.Contains(item);
  }

  /// <summary>
  /// Check if an item is in the given enumerable.
  /// </summary>
  /// <param name="item">Item to check</param>
  /// <param name="items">Items</param>
  /// <typeparam name="T">Type of the items</typeparam>
  public static bool IsIn<T>(this T item, IEnumerable<T> items) {
    return items.Contains(item);
  }

  /// <summary>
  /// Can be used to conditionally perform a function
  /// on an object and return the modified or the original object.
  /// It is useful for chained calls.
  /// </summary>
  /// <param name="obj">An object</param>
  /// <param name="condition">A condition</param>
  /// <param name="func">A function that is executed only if the condition is <code>true</code></param>
  /// <typeparam name="T">Type of the object</typeparam>
  /// <returns>
  /// Returns the modified object (by the <paramref name="func"/> if the <paramref name="condition"/> is <code>true</code>)
  /// or the original object if the <paramref name="condition"/> is <code>false</code>
  /// </returns>
  public static T If<T>(this T obj, bool condition, Func<T, T> func) {
    if (condition) {
      return func(obj);
    }

    return obj;
  }

  /// <summary>
  /// Can be used to conditionally perform an action
  /// on an object and return the original object.
  /// It is useful for chained calls on the object.
  /// </summary>
  /// <param name="obj">An object</param>
  /// <param name="condition">A condition</param>
  /// <param name="action">An action that is executed only if the condition is <code>true</code></param>
  /// <typeparam name="T">Type of the object</typeparam>
  /// <returns>
  /// Returns the original object.
  /// </returns>
  public static T If<T>(this T obj, bool condition, Action<T> action) {
    if (condition) {
      action(obj);
    }

    return obj;
  }

  public static string GetGenericTypeName(this object @object) {
    return @object.GetType().GetGenericTypeName();
  }

  /// <summary>
  /// Traverses and object, and it's properties recursively to find any object which matches the type param of <typeparamref name="T"/>
  /// and returns that object into the output collection.
  /// </summary>
  /// <typeparam name="T">Type we are looking for in the object graph</typeparam>
  /// <param name="root">starting object.</param>
  /// <returns>A flattened list of objects that match the <typeparamref name="T"/> in this object graph</returns>
  public static IEnumerable<T> TraverseGraphFor<T>(this object root) where T : class {
    return ObjectGraphWalker.TraverseGraphFor<T>(root);
  }
}
