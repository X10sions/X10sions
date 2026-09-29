using Common.Structures;

namespace Common.Models;

public record MinMax<T>(T Min, T Max) {
  public MinMax(IEnumerable<T> values) : this(values.Min(), values.Max()) { }
  public MinMax(T[] values) : this(values.Min(), values.Max()) { }
  public static MinMax<T> Instance(T min, T max) => new(min, max);

  //public string NameOfMin(string separator = "_") => WebPagesExtensions.NameOf<MinMax<T>>(x=> x.Min, false, separator);
  //public string NameOfMax(string separator = "_") => WebPagesExtensions.NameOf<MinMax<T>>(x=> x.Max, false, separator);
  //public static string GetMin(string namePrefix) => HttpContext.Current.Request[NameOfMin(namePrefix)];
  //public static string GetMax(string namePrefix) => HttpContext.Current.Request[NameOfMax(namePrefix)];
  //public static string NameOfMin(string namePrefix) => namePrefix + nameof(Min);
  //public static string NameOfMax(string namePrefix) => namePrefix + nameof(Max);
}

public record DateTimeMinMax : MinMax<DateTime> {
  public DateTimeMinMax(DateTime min, DateTime max) : base(min, max) { }
}

public record DateTimeNullableMinMax : MinMax<DateTime?> {
  public DateTimeNullableMinMax(DateTime? min, DateTime? max) : base(min, max) { }
}

public record IntCYYMMDDNullableMinMax : MinMax<IntCYYMMDD?> {
  //public static IntCYYMMDDNullableMinMax GetHttpRequestInstance(string namePrefix) => new (
  //  Base64FormattingOptions.re
  //  );
  //public IntCYYMMDDNullableMinMax(string namePrefix, IntCYYMMDD? defaultMin = null, IntCYYMMDD? defaultMax = null)
  //  : base(HttpRequestMin(namePrefix).As(defaultMin), HttpRequestMax(namePrefix).As(defaultMax)) { }
  public IntCYYMMDDNullableMinMax(IntCYYMMDD? min, IntCYYMMDD? max) : base(min, max) { }
  public IntCYYMMDDNullableMinMax(DateTime? min, DateTime? max) : base(min, max) { }
}