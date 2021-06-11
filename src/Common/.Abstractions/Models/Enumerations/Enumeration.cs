using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Models.Enumerations {
  public abstract class Enumeration : IComparable {

    // https://github.com/dotnet-architecture/eShopOnContainers/blob/master/src/Services/Ordering/Ordering.Domain/SeedWork/Enumeration.cs
    // https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/

    protected Enumeration() { }
    protected Enumeration(int id, string name) { Id = id; Name = name; }

    public string Name { get; }
    public int Id { get; }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration, new() {
      var type = typeof(T);
      foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)) {
        var instance = new T();
        var locatedValue = info.GetValue(instance) as T;
        if (locatedValue != null) {
          yield return locatedValue;
        }
      }
    }

    public override bool Equals(object obj) {
      var otherValue = obj as Enumeration;
      if (otherValue == null) {
        return false;
      }
      var typeMatches = GetType().Equals(obj.GetType());
      var valueMatches = Id.Equals(otherValue.Id);
      return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue) => Math.Abs(firstValue.Id - secondValue.Id);

    public static T FromValue<T>(int value) where T : Enumeration, new() => parse<T, int>(value, "value", item => item.Id == value);

    public static T FromDisplayName<T>(string displayName) where T : Enumeration, new() => parse<T, string>(displayName, "display name", item => item.Name == displayName);

    static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new() {
      var matchingItem = GetAll<T>().FirstOrDefault(predicate);
      if (matchingItem == null) {
        var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
        throw new ApplicationException(message);
      }
      return matchingItem;
    }


  }
}