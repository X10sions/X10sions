using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic {
  public static class IAsyncEnumerableExtensions {

    public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
     => new AsyncSelectEnumerable<TSource, TResult>(source, selector);

  }

}
