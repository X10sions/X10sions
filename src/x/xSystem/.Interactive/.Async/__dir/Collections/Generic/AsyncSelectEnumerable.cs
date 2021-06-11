using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic {
  public class AsyncSelectEnumerable<TSource, TResult> : IAsyncEnumerable<TResult> {
    readonly IAsyncEnumerable<TSource> _source;
    readonly Func<TSource, CancellationToken, Task<TResult>> _selector;

    public AsyncSelectEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector) {
      _source = source;
      _selector = selector;
    }

    public IAsyncEnumerator<TResult> GetEnumerator() => new AsyncSelectEnumerator(this);

    public class AsyncSelectEnumerator : IAsyncEnumerator<TResult> {
      readonly IAsyncEnumerator<TSource> _enumerator;
      readonly Func<TSource, CancellationToken, Task<TResult>> _selector;

      public AsyncSelectEnumerator(AsyncSelectEnumerable<TSource, TResult> enumerable) {
        _enumerator = enumerable._source.GetEnumerator();
        _selector = enumerable._selector;
      }

      public async Task<bool> MoveNext(CancellationToken cancellationToken) {
        if (!await _enumerator.MoveNext(cancellationToken).ConfigureAwait(false)) {
          return false;
        }
        Current = await _selector(_enumerator.Current, cancellationToken).ConfigureAwait(false);
        return true;
      }

      public TResult Current { get; private set; }
      public void Dispose() => _enumerator.Dispose();
    }

  }
}
