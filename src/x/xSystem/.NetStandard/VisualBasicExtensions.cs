namespace System {

  [Obsolete("Old Visual Basic (VB) methods")]
  public static class VisualBasicExtensions {
    [Obsolete("Use: Convert.ToChar(s)")] public static char Chr(this string s) => Convert.ToChar(s);
    [Obsolete("Use: ? : ")] public static object IIF(this bool testExpression, object trueValue, object falseValue) => testExpression ? trueValue : falseValue;
    [Obsolete("Use: .IndexOf(find)")] public static int InStr(this string s, string find) => s.IndexOf(find, StringComparison.Ordinal);
    [Obsolete("Use: .LastIndexOf(find)")] public static int InStrRev(this string s, string find) => s.LastIndexOf(find, StringComparison.Ordinal);
    [Obsolete("Use: ReferenceEquals(o, null)")] public static bool IsNothing(this object o) => o == null;
    //public static string MsgBox(this string s) => MessageBox.Show();
    [Obsolete("Use: .GetLowerBound(0)")] public static int LBound(this Array a) => a.GetLowerBound(0);
    [Obsolete("Use: .ToLower()")] public static string LCase(this string s) => s.ToLower();

    [Obsolete("Use: .Substring(0, length)")]
    public static string Left(this string s, int length) {
      if (length < 0) {
        throw new ArgumentException(nameof(length));
      }
      return length == 0 || s == null ? string.Empty : s.Substring(0, Math.Min(s.Length, length));
    }

    [Obsolete("Use: .Length")] public static int Len(this string s) => s.Length;
    [Obsolete("Use: .TrimStart()")] public static string LTrim(this string s) => s.TrimStart();

    [Obsolete("Use: .Substring(start - 1)")] public static string Mid(this string s, int start) => s.Mid(start, s.Length);

    [Obsolete("Use: .Substring(start - 1, length);")]
    public static string Mid(this string str, int start, int length) {
      if (start <= 0) {
        throw new ArgumentException(nameof(start));
      } else if (length < 0) {
        throw new ArgumentException(nameof(length));
      }
      if (length == 0 || str == null) {
        return string.Empty;
      }
      var strLength = str.Length;
      return start > strLength ? string.Empty : start + length > strLength ? str.Substring(start - 1) : str.Substring(start - 1, length);
    }

    [Obsolete("Use: .Substring(s.Length - length, length)")]
    public static string Right(this string str, int length) {
      if (length < 0) {
        throw new ArgumentException(nameof(length));
      }
      if (length == 0 || str == null) {
        return string.Empty;
      }
      var strLength = str.Length;
      return length >= strLength ? str : str.Substring(strLength - length, length);
    }

    [Obsolete("Use: .TrimEnd()")] public static string RTrim(this string s) => s.TrimEnd();
    [Obsolete("Use: .GetUpperBound(0)")] public static int UBound(this Array a) => a.GetUpperBound(0);
    [Obsolete("Use: .ToUpper()")] public static string UCase(this string s) => s.ToUpper();
  }
}