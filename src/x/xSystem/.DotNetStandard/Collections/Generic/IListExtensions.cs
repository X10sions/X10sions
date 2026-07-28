namespace System.Collections.Generic;
  public static class IListExtensions {

  public static T AddReturn<T>(this IList<T> list, T newObject) {
    list.Add(newObject);
    return newObject;
  }

  public static T AddNew<T>(this IList<T> list, Func<T> createNewObject) {
    var newObject = createNewObject();
    list.Add(newObject);
    return newObject;
  }

  public static IList<int> AddTryParse<T>(this IList<int> list, T value) {
    if (value is not null && int.TryParse(value.ToString(), out var result)) {
      list.Add(result);
    }
    return list;
  }

}
