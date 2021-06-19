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

    public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new AsyncSelectEnumerator(this, cancellationToken);

    public class AsyncSelectEnumerator : IAsyncEnumerator<TResult> {
      readonly IAsyncEnumerator<TSource> _enumerator;
      readonly Func<TSource, CancellationToken, Task<TResult>> _selector;

      public AsyncSelectEnumerator(AsyncSelectEnumerable<TSource, TResult> enumerable, CancellationToken cancellationToken = default) {
        _enumerator = enumerable._source.GetAsyncEnumerator(cancellationToken);
        _selector = enumerable._selector;
        _cancellationToken = cancellationToken;
      }
      CancellationToken _cancellationToken;
      public async ValueTask<bool> MoveNextAsync() {
        if (!await _enumerator.MoveNextAsync(_cancellationToken).ConfigureAwait(false)) {
          return false;
        }
        Current = await _selector(_enumerator.Current, _cancellationToken).ConfigureAwait(false);
        return true;
      }

      public TResult Current { get; private set; }
      //public void Dispose() => _enumerator.Dispose();

      public async ValueTask DisposeAsync()  =>  await _enumerator.DisposeAsync();
    }

  }
}
