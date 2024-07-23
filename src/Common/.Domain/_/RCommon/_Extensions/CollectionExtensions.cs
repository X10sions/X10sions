using RCommon.Collections;
using System.Text;

namespace RCommon;
public static class CollectionExtensions {
  public static readonly char CommaDelimiter = ',';

  public async static Task<int> ForEachCountAsync<T>(this IEnumerable<T> rows, Func<T, Task> countFunction) {
    var rowCount = 0;
    foreach (var row in rows) {
      await countFunction(row);
      rowCount++;
    }
    return rowCount;
  }

  /// <summary>
  /// Returns a comma-delimited string from an <c>IList</c>
  /// </summary>
  /// <param name="source">The list of elements to create delimited string from</param>
  /// <returns>The string consisting of comma-separated elements (using the ToString() method) from the input list</returns>
  /// <exception cref="ArgumentNullException">if <paramref name="source"/> is null</exception>
  public static string GetCommaDelimitedString<T>(this IEnumerable<T> source) => source.GetDelimitedString(CommaDelimiter);

  /// <summary>
  /// Returns a comma-delimited string containing the data from each element in the specified list.
  /// </summary>
  /// <typeparam name="T">The type of the elements</typeparam>
  /// <param name="source">The list of elements whose data will be concatenated.</param>
  /// <param name="funcToGetString">The delegate to return data to be extracted from each element</param>
  /// <returns>comma-delimited string</returns>
  /// <exception cref="ArgumentNullException">if <paramref name="source"/> is null</exception>
  public static string GetCommaDelimitedString<T>(this IEnumerable<T> source, Func<T, string> funcToGetString) => source.GetDelimitedString(funcToGetString, CommaDelimiter, false, true);

  /// <summary>
  /// Returns a string of elements separated by a user-specified delimiter character
  /// </summary>
  /// <param name="delimiter">The specified delimiter character to be used to separate integers</param>
  /// <param name="source">The list of elements to create delimited string from</param>
  /// <returns>The string consisting of delimiter-separated elements (using the ToString() method) from the input list</returns>
  /// <exception cref="ArgumentNullException">if <paramref name="source"/> is null</exception>
  public static string GetDelimitedString<T>(this IEnumerable<T> source, char delimiter) => source.GetDelimitedString(t => t.ToString(), delimiter, false, true);

  /// <summary>
  /// Returns a string of elements separated by a user-specified delimiter character
  /// </summary>
  /// <param name="source">The list of elements to create delimited string from</param>
  /// <param name="funcToGetString">The delegate to return data to be extracted from each element</param>
  /// <param name="delimiter">The specified delimiter character to be used to separate integers</param>
  /// <param name="addLeadingDelimiter">A flag indicating if the trailing delimiter should be removed</param>
  /// <param name="removeTrailingDelimiter">A flag indicating if the trailing delimiter should be removed</param>
  /// <returns>The string consisting of delimiter-separated elements (using the ToString() method) from the input list</returns>
  /// <exception cref="ArgumentNullException">if <paramref name="source"/> is null</exception>
  /// <exception cref="ArgumentNullException">if <paramref name="funcToGetString"/> is null</exception>
  public static string GetDelimitedString<T>(this IEnumerable<T> source, Func<T, string> funcToGetString, char delimiter, bool addLeadingDelimiter, bool removeTrailingDelimiter) {
    Guard.IsNotNull(source, "source");
    Guard.IsNotNull(funcToGetString, "funcToGetString");
    if (source.Count() == 0) return null;
    StringBuilder sbuf = source.Aggregate(new StringBuilder(),
      (soFar, item) => soFar.Append(funcToGetString(item)).Append(delimiter));

    if (addLeadingDelimiter) sbuf.Insert(0, delimiter);
    if (removeTrailingDelimiter) sbuf.Remove(sbuf.Length - 1, 1);

    return sbuf.ToString();
  }


  public static IPaginatedList<T> ToPaginatedList<T>(this ICollection<T> query, int? pageIndex, int pageSize) {
    Guard.IsNotNegativeOrZero(pageSize, "pageSize");
    return new PaginatedList<T>(query, pageIndex, pageSize);
  }

  public static IPaginatedList<T> ToPaginatedList<T>(this IList<T> query, int? pageIndex, int pageSize) {
    Guard.IsNotNegativeOrZero(pageSize, "pageSize");
    return new PaginatedList<T>(query, pageIndex, pageSize);
  }

}
