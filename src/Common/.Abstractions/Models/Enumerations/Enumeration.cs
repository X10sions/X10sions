using System.Reflection;

namespace Common.Models.Enumerations;

//public abstract class Enumeration : Enumeration<int,string>{ }

public abstract class Enumeration<TKey, TValue> : IComparable where TKey : IComparable where TValue : IComparable {

  // https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/Services/Ordering/Ordering.Domain/SeedWork/Enumeration.cs
  // https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/

  //protected Enumeration() { }
  protected Enumeration(TKey key, TValue value) { Key = key; Value = value; }

  public TValue Value { get; }
  public TKey Key { get; }

  public override string ToString() => Value.ToString();

  public override bool Equals(object obj) {
    var otherValue = obj as Enumeration<TKey, TValue>;
    if (otherValue == null) {
      return false;
    }
    var typeMatches = GetType().Equals(obj.GetType());
    var valueMatches = Key.Equals(otherValue.Key);
    return typeMatches && valueMatches;
  }
  public override int GetHashCode() => Key.GetHashCode();

  public int CompareTo(object other) => Key.CompareTo(((Enumeration<TKey, TValue>)other).Key);


  public static int AbsoluteDifference(Enumeration<int, TValue> firstValue, Enumeration<int, TValue> secondValue) => Math.Abs(firstValue.Key - secondValue.Key);

  public static T FromDisplayName<T>(TValue displayName) where T : Enumeration<TKey, TValue>, new() => parse<T>(displayName, "display name", item => item.Value.Equals(displayName));
  public static T FromValue<T>(TValue value) where T : Enumeration<TKey, TValue>, new() => parse<T>(value, "value", item => item.Value.Equals(value));

  public static IEnumerable<T> GetAll<T>() where T : Enumeration<TKey, TValue>, new() {
    var type = typeof(T);
    foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
      var instance = new T();
      var locatedValue = info.GetValue(instance) as T;
      if (locatedValue != null) {
        yield return locatedValue;
      }
    }
  }

  static T parse<T>(TValue value, string description, Func<T, bool> predicate) where T : Enumeration<TKey, TValue>, new() {
    var matchingItem = GetAll<T>().FirstOrDefault(predicate);
    if (matchingItem == null) {
      var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
      throw new ApplicationException(message);
    }
    return matchingItem;
  }
}

public static class EnumerationExtensions {




}