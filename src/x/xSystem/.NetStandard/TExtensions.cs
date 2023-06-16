namespace System;
public static class TExtensions {

  public static TTo? As<TFrom, TTo>(this TFrom value, TTo? defaultValue = default) {
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

  public static Task<T> Async<T>(this Func<T> func, CancellationToken cancellationToken = default) {
    if (cancellationToken.IsCancellationRequested) {
      return Task.FromCanceled<T>(cancellationToken);
    }
    try {
      return Task.Run(func, cancellationToken);
    } catch (Exception e) {
      return Task.FromException<T>(e);
    }
  }

  public static object GetTypeFieldValueAs<T>(this T obj, string fieldName) where T : notnull => obj.GetTypeFieldValueAs<T, object>(fieldName);
  public static object GetTypePropertyValueAs<T>(this T obj, string propertyName) where T : notnull => obj.GetTypePropertyValueAs<T, object>(propertyName);

  public static TField GetTypeFieldValueAs<T, TField>(this T obj, string fieldName) where T : notnull => obj.GetType().GetFieldValueAs<T, TField>(fieldName, obj);
  public static TProperty GetTypePropertyValueAs<T, TProperty>(this T obj, string propertyName) where T : notnull => obj.GetType().GetPropertyValueAs<T, TProperty>(propertyName, obj);

  //public static bool IsNullable<T>(this T o) => typeof(T).IsNullable();

  // Internal Field/Property helper
  //    public static TField GetTypeFieldValueAs<T, TField>(this T obj, string fieldName) => typeof(T).GetFieldValueAs<T, TField>(fieldName, obj);
  //    public static TProperty GetTypePropertyValueAs<T, TProperty>(this T obj, string propertyName) => typeof(T).GetPropertyValueAs<T, TProperty>(propertyName, obj);

  // public static bool IsNullable<T>(this T obj) => (obj == null) ? true : typeof(T).IsNullable();

  public static T Set<T>(this T input, Action<T> updater) {
    // https://robvolk.com/linq-select-an-object-but-change-some-properties-without-creating-a-new-object-af4072738e33
    // select some monkeys and modify a property in the select statement instead of creating a new monkey and manually setting all 
    // example:  var list = from monkey in monkeys select monkey.Set(monkey1 => {  monkey1.FavoriteFood += " and banannas"; });
    updater(input);
    return input;
  }

  public static T SetAndReturn<T>(this T newValue, ref T setThis) => setThis = newValue;

  public static T SetAndReturnIfNull<T>(this Func<T> newValueFunc, ref T setThis) {
    if (setThis == null) {
      setThis = newValueFunc();
    }
    return setThis;
  }

  public static T SetAndReturnIfNull<T>(this Func<T> newValueFunc, ref T setThis, Action actionIfNull) {
    if (setThis == null) {
      setThis = newValueFunc();
      actionIfNull();
    }
    return setThis;
  }

  public static string WrapIfNotNull<T>(this T value, string prefix = "", string suffix = "", string defaultIfNull = "") => (value == null) ? defaultIfNull : $"{prefix}{value}{suffix}";

}