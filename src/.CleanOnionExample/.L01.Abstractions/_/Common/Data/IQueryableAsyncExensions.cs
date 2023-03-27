namespace Common.Data;

public static class IQueryableAsyncExensions {

  public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IQueryable<T> source) {
    if (source is IAsyncEnumerable<T> asyncEnumerable) {
      return asyncEnumerable;
    }
    throw new InvalidOperationException($"IQueryableNotAsync: {typeof(T)}");
  }

  public static async Task<List<T>> xToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default) {
    var list = new List<T>();
    await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken)) {
      list.Add(element);
    }
    return list;
  }

}
