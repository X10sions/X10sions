namespace System {
  public static class CharExtensions {
    public static string Get(this char[] values, int start, int end) => new string(values, start - 1, end - start + 1);
    public static void Set<T>(this char[] values, int start, int end, T value) where T :notnull{
      var length = end - start + 1;
      var fixedLengthValue = value.ToString().FixedLengthLeft(length);
      for (var i = 0; i < length; i++) {
        values[start - 1 + i] = fixedLengthValue[i];
      }
    }

  }
}