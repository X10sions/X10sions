using System;
using System.Collections;
using System.Collections.Generic;

namespace IQToolkit {
  /// <summary>
  /// Represents a key with multiple values as a single instance.
  /// </summary>
  public class CompoundKey : IEquatable<CompoundKey>, IEnumerable<object>, IEnumerable {
    private readonly object[] values;
    private readonly int hc;

    public CompoundKey(params object[] values) {
      this.values = values;
      for (int i = 0, n = values.Length; i < n; i++) {
        var value = values[i];
        if (value != null) {
          hc ^= (value.GetHashCode() + i);
        }
      }
    }

    public override int GetHashCode() => hc;

    public override bool Equals(object obj) => base.Equals(obj);

    public bool Equals(CompoundKey other) {
      if (other == null || other.values.Length != values.Length)
        return false;
      for (int i = 0, n = other.values.Length; i < n; i++) {
        if (!object.Equals(values[i], other.values[i]))
          return false;
      }
      return true;
    }

    public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>)values).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}