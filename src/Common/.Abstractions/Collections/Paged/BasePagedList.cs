using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Paged {

  public abstract class BasePagedList<T> : IPagedList<T> {
    protected readonly List<T> Subset = new List<T>();

    protected internal BasePagedList() { }

    protected internal BasePagedList(int pageNumber, int pageSize, int totalItemCount) {
      TotalItemCount = totalItemCount;
      PageSize = pageSize > 0 ? pageSize : throw new ArgumentOutOfRangeException($"{nameof(pageSize)} '{pageSize}' cannot be less than 1.");
      PageNumber = pageNumber > 0 ? pageNumber : throw new ArgumentOutOfRangeException($"{nameof(pageNumber)} '{pageNumber}' cannot be less than 1.");
    }

    public int TotalItemCount { get; protected set; }
    public int PageNumber { get; protected set; }
    public int PageSize { get; protected set; }

    #region IPagedList<T> Members
    public IEnumerator<T> GetEnumerator() => Subset.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public T this[int index] => Subset[index];
    public virtual int Count => Subset.Count;
    #endregion
  }
}