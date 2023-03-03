using System.Reflection;

namespace System.Text;
public static class StringBuilderExtensions {

  public static void AppendCsvRow(this StringBuilder sb, List<string> excludedPropertyNames, string separator, bool alwaysQuote, PropertyInfo[] props, object? o) {
    var index = 0;
    foreach (var f in props) {
      if (!excludedPropertyNames.Contains(f.Name)) {
        if (index > 0)
          sb.Append(separator);
        var x = f.GetValue(o);
        if (x != null) {
          sb.AppendCsvCell(x.ToString(), alwaysQuote);
        }
        index++;
      }
    }
  }

  public static void AppendCsvCell(this StringBuilder sb, string str, bool alwaysQuote) {
    var mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
    if (mustQuote) {
      sb.Append("\"");
      foreach (var nextChar in str) {
        sb.Append(nextChar);
        if (nextChar == '"')
          sb.Append("\"");
      }
      sb.Append("\"");
    } else if (alwaysQuote) {
      sb.Append("\"");
      sb.Append(str);
      sb.Append("\"");
    } else {
      sb.Append(str);
    }
  }

  public static StringBuilder AppendIfTrue(this StringBuilder sb, bool test, string value) {
    if (test) {
      sb.Append(value);
    }
    return sb;
  }

  public static StringBuilder AppendLineIfTrue(this StringBuilder sb, bool test, string value) {
    if (test) {
      sb.AppendLine(value);
    }
    return sb;
  }

  public static StringBuilder AppendJoin(this StringBuilder stringBuilder,
    IEnumerable<string> values, string separator = ", ")
    => stringBuilder.AppendJoin(values, (sb, value) => sb.Append(value), separator);

  public static StringBuilder AppendJoin(
      this StringBuilder stringBuilder, string separator, params string[] values)
      => stringBuilder.AppendJoin(values, (sb, value) => sb.Append(value), separator);

  public static StringBuilder AppendJoin<T>(this StringBuilder stringBuilder,
    IEnumerable<T> values,
    Action<StringBuilder, T> joinAction,
    string separator = ", ") {
    var appended = false;
    foreach (var value in values) {
      joinAction(stringBuilder, value);
      stringBuilder.Append(separator);
      appended = true;
    }
    if (appended) {
      stringBuilder.Length -= separator.Length;
    }
    return stringBuilder;
  }

  public static StringBuilder AppendJoin<T, TParam>(this StringBuilder stringBuilder,
      IEnumerable<T> values,
      TParam param,
      Action<StringBuilder, T, TParam> joinAction,
      string separator = ", ") {
    var appended = false;
    foreach (var value in values) {
      joinAction(stringBuilder, value, param);
      stringBuilder.Append(separator);
      appended = true;
    }
    if (appended) {
      stringBuilder.Length -= separator.Length;
    }
    return stringBuilder;
  }

  public static StringBuilder AppendJoin<T, TParam1, TParam2>(this StringBuilder stringBuilder,
      IEnumerable<T> values,
      TParam1 param1,
      TParam2 param2,
      Action<StringBuilder, T, TParam1, TParam2> joinAction,
      string separator = ", ") {
    var appended = false;
    foreach (var value in values) {
      joinAction(stringBuilder, value, param1, param2);
      stringBuilder.Append(separator);
      appended = true;
    }
    if (appended) {
      stringBuilder.Length -= separator.Length;
    }
    return stringBuilder;
  }

  public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase = true) {
    int index;
    var length = value.Length;
    var maxSearchLength = (sb.Length - length) + 1;
    if (ignoreCase) {
      for (var i = startIndex; i < maxSearchLength; ++i) {
        if (char.ToLower(sb[i]) == char.ToLower(value[0])) {
          index = 1;
          while ((index < length) && (char.ToLower(sb[i + index]) == char.ToLower(value[index])))
            ++index;
          if (index == length)
            return i;
        }
      }
      return -1;
    }
    for (var i = startIndex; i < maxSearchLength; ++i) {
      if (sb[i] == value[0]) {
        index = 1;
        while ((index < length) && (sb[i + index] == value[index]))
          ++index;

        if (index == length)
          return i;
      }
    }
    return -1;
  }

  public static StringBuilder Prepend(this StringBuilder builder, string content) => builder.Insert(0, content);
  public static StringBuilder PrependLine(this StringBuilder builder, string content) => builder.Prepend(content + Environment.NewLine);

}
