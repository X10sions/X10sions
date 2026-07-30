namespace System.Collections.Generic;

public static class SortedSetExtensions {
  public static SortedSet<T> AddIf<T>(this SortedSet<T> set, bool addIfTrue, T value) {
    if (addIfTrue) {
      set.Add(value);
    }
    return set;
  }

  public static SortedSet<T> AddIf<T>(this SortedSet<T> set, bool addIfTrue, T ifTrueValue, T ifFalseValue) {
    set.Add(addIfTrue ? ifTrueValue : ifFalseValue);
    return set;
  }

}