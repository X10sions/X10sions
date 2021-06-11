using System.Collections.Generic;

namespace Common.Collections.Paged {
  public class StaticPagedList<T> : BasePagedList<T> {

    public StaticPagedList(IEnumerable<T> subset, IPagedList metaData)
      : this(subset, metaData.PageNumber, metaData.PageSize, metaData.TotalItemCount) {
    }

    public StaticPagedList(IEnumerable<T> subset, int pageNumber, int pageSize, int totalItemCount)
      : base(pageNumber, pageSize, totalItemCount) {
      Subset.AddRange(subset);
    }

  }
}