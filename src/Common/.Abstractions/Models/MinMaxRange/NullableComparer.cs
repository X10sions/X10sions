using System;
using System.Collections.Generic;

namespace Common.Models.MinMaxRange {
  public class NullableComparer<T> : IComparer<T?> where T : struct, IComparable<T> {
    public int Compare(T? x, T? y) {
      if (!x.HasValue && !y.HasValue) {
        return 0;
      }
      if (x.HasValue && !y.HasValue) {
        return 1;
      }
      if (y.HasValue && !x.HasValue) {
        return -1;
      }
      return x.Value.CompareTo(y.Value);
    }

  }
}
