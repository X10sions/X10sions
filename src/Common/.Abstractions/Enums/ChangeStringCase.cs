namespace Common.Enums {
  public enum StringCaseOptions {
    NoChange,
    ToLower,
    ToUpper
  }

  public static class StringCaseOptionsExtensions {

    public static IEnumerable<int> ToIntegerList(this string s, string separator = ",") => string.IsNullOrWhiteSpace(s) ? Enumerable.Empty<int>() : (from x in s.ToStringList(separator)
                                                                                                                                                     where x.IsNumeric()
                                                                                                                                                     select Convert.ToInt32(x));

    public static IEnumerable<string> ToStringList(this string s, string separator = ",", StringCaseOptions caseOptions = StringCaseOptions.NoChange, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries, bool isDistinct = true) {
      if (string.IsNullOrWhiteSpace(s)) {
        return Enumerable.Empty<string>();
      }
      var values = from x in s.Split(separator, splitOptions) select x.Trim();
      switch (caseOptions) {
        case StringCaseOptions.ToLower:
          values = values.Select((string x) => x.ToLower());
          break;
        case StringCaseOptions.ToUpper:
          values = values.Select((string x) => x.ToUpper());
          break;
      }
      if (isDistinct) {
        values = values.Distinct();
      }
      return values;
    }
  }
}
