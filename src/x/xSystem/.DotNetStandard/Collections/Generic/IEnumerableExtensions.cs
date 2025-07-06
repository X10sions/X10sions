using System.Data;
using System.Reflection;

namespace System.Collections.Generic {
  public static class IEnumerableExtensions {
    public static List<TResult> AsList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) => source.Select(selector).ToList();

    public static string DebugString<T>(this IEnumerable<T> source, Func<T, string> debugStringAction)
      => $"[ {string.Join(",", source.Select(_ => debugStringAction))} ]";

    public static string DebugString<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, bool includeChildren = false)
      => $"[ {string.Join(",", source.Select(x => x.DebugString(includeChildren)))} ]";

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer) where T : class
      => source.Distinct(new DynamicEqualityComparer<T>(comparer));

    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> each) {
      foreach (var item in enumerable) { each(item); }
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> enumerable, params T[] singles) => enumerable.Except((IEnumerable<T>)singles);

    public static Type GetListItemType(this IEnumerable list) {
      var typeOfObject = typeof(object);
      if (list == null)
        return typeOfObject;
      if (list is Array)
        return list.GetType().GetElementType();
      var type = list.GetType();
      if (list is IList or ITypedList or IListSource) {
        PropertyInfo? last = null;
        foreach (var pi in type.GetProperties()) {
          if (pi.GetIndexParameters().Length > 0 && pi.PropertyType != typeOfObject) {
            if (pi.Name == "Item")
              return pi.PropertyType;
            last = pi;
          }
        }
        if (last != null)
          return last.PropertyType;
      }
      if (list is IList) {
        foreach (var o in (IList)list)
          if (o != null && o.GetType() != typeOfObject)
            return o.GetType();
      } else {
        foreach (var o in list)
          if (o != null && o.GetType() != typeOfObject)
            return o.GetType();
      }
      return typeOfObject;
    }

    //public static bool HasElements<T>(this IEnumerable<T> source) => source == null || !source.Any();
    public static bool HasAnyItems<T>(this IEnumerable<T>? source) => source?.Any() ?? false;
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => !source.HasAnyItems();
    public static bool IsNullOrWhiteSpace<T>(this IEnumerable<T>? source) => source == null || !source.Any(x => !string.IsNullOrWhiteSpace(x?.ToString()));

    public static string Join<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);

    public static string JoinToCsv<T>(this IEnumerable<T> source, string separator = ",", string prefix = "", string? suffix = null) {
      suffix ??= prefix;
      return source.IsNullOrEmpty() ? string.Empty : prefix + string.Join(suffix + separator + prefix, source.Select(x => x?.ToString()).ToArray()) + suffix;
    }

    public static string JoinToCsv<T>(this IEnumerable<T> objectlist, List<string>? excludedPropertyNames = null, bool quoteEveryField = false, bool includeFieldNamesAsFirstRow = true) {
      excludedPropertyNames ??= [];
      var separator = ",";
      var t = typeof(T);
      var props = t.GetProperties();
      var arrPropNames = props.Where(p => !excludedPropertyNames.Contains(p.Name)).Select(f => f.Name).ToArray();
      var csvBuilder = new StringBuilder();
      if (includeFieldNamesAsFirstRow) {
        if (quoteEveryField) {
          for (var i = 0; i <= arrPropNames.Length - 1; i++) {
            if (i > 0) { csvBuilder.Append(separator); }
            csvBuilder.Append('"');
            csvBuilder.Append(arrPropNames[i]);
            csvBuilder.Append('"');
          }
          csvBuilder.Append(Environment.NewLine);

        } else {
          var header = string.Join(separator, arrPropNames);
          csvBuilder.AppendLine(header);
        }
      }
      foreach (var o in objectlist) {
        csvBuilder.AppendCsvRow(excludedPropertyNames, separator, quoteEveryField, props, o);
        csvBuilder.Append(Environment.NewLine);
      }
      return csvBuilder.ToString();
    }

    public static string JoinToString(this IEnumerable<object> source, string separator = ", ") => string.Join(separator, source);
    public static string JoinToString<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvPairs, string keySeparator = ";", string valueSeparator = "=", string prefix = "{", string suffix = "}") => prefix + string.Join(keySeparator, kvPairs.Select(kv => $"{kv.Key}{valueSeparator}{kv.Value}").ToArray()) + suffix;

    public static string SqlLiteralBetween<T>(this IEnumerable<T> values, string prefix, string suffix, string JoinString = " And ")
      => values.SqlLiteralMin(prefix, suffix) + JoinString + values.SqlLiteralMax(prefix, suffix);

    public static string SqlLiteralIn<T>(this IEnumerable<T> values, string prefix, string suffix)
      => prefix + string.Join($"{suffix}, {prefix}", values) + suffix;

    public static string SqlLiteralMax<T>(this IEnumerable<T> values, string prefix, string suffix) => prefix + values.Max() + suffix;
    public static string SqlLiteralMin<T>(this IEnumerable<T> values, string prefix, string suffix) => prefix + values.Min() + suffix;

    public static DataTable ToDataTable<T>(this IEnumerable<T> items) {
      var dataTable = new DataTable(typeof(T).Name);
      var Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var prop in Props) {
        dataTable.Columns.Add(prop.Name);
      }
      foreach (var item in items) {
        var values = new object[Props.Length];
        for (var i = 0; i < Props.Length; i++) {
          values[i] = Props[i].GetValue(item, null);
        }
        dataTable.Rows.Add(values);
      }
      return dataTable;
    }

    public static DataSet ToDataSet<T>(this IEnumerable<T> items) {
      var ds = new DataSet();
      ds.Tables.Add(items.ToDataTable());
      return ds;
    }

    public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector) {
      var stack = new Stack<T>(items);
      while (stack.Count > 0) {
        var next = stack.Pop();
        yield return next;
        foreach (var child in childSelector(next)) {
          stack.Push(child);
        }
      }
    }

    public static string WrapIfNotNullOrWhiteSpace<T>(this IEnumerable<T> source, string prefix, string suffix, string joinSeparator = ",", string defaultIfNullOrWhiteSpace = "") => source.IsNullOrWhiteSpace() ? defaultIfNullOrWhiteSpace : prefix + string.Join(joinSeparator, source) + suffix;

    public class DynamicEqualityComparer<T>(Func<T, T, bool> func) : IEqualityComparer<T> where T : class {
      public bool Equals(T x, T y) => func(x, y);

      public int GetHashCode(T obj) => 0; // force Equals
    }
    public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue) where T : notnull => source.Select(x => x.Equals(oldValue) ? newValue : x);

    public static IEnumerable<TSource> OrderByIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) => condition ? source.OrderBy(keySelector, comparer) : source;
    public static IEnumerable<TSource> OrderByIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector) => condition ? source.OrderBy(keySelector) : source;

    public static IEnumerable<TSource> OrderByDescendingIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) => condition ? source.OrderByDescending(keySelector, comparer) : source;
    public static IEnumerable<TSource> OrderByDescendingIf<TSource, TKey>(this IEnumerable<TSource> source, bool condition, Func<TSource, TKey> keySelector) => condition ? source.OrderByDescending(keySelector) : source;
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate) => condition ? source.Where(predicate) : source;
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate) => condition ? source.Where(predicate) : source;

    public static IEnumerable<TSource> WhereIfNotNull<TSource>(this IEnumerable<TSource> source, Func<TSource, bool>? predicate) => predicate != null ? source.Where(predicate) : source;
    public static IEnumerable<TSource> WhereIfNotNull<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool>? predicate) => predicate != null ? source.Where(predicate) : source;

  }
}