namespace Common {
  /// <summary>
  /// Author: NMick Chapsas
  ///
  /// for each(var i in 0..10){
  ///   DoSomething(i);
  /// }
  /// </summary>
  /// <see cref="https://www.youtube.com/watch?v=jmmz1cInNow"/>
  public class CustomIntEnumerator {
    private readonly int _end;
    public CustomIntEnumerator(Range range) {
      if (range.End.IsFromEnd) {
        throw new NotSupportedException();
      }
      Current = range.Start.Value - 1;
      _end = range.End.Value;
    }

    public CustomIntEnumerator(int maxNumber) : this(new Range(0, maxNumber)) { }

    public int Current { get; private set; }
    public bool MoveNext() {
      Current++;
      return Current <= _end;
    }
  }

  public static class RangeExtensions {

    public static CustomIntEnumerator GetEnumerator<T>(this Range range) => new CustomIntEnumerator(range);
    public static CustomIntEnumerator GetEnumerator<T>(this int maxNumber) => new CustomIntEnumerator(maxNumber);

  }

}
