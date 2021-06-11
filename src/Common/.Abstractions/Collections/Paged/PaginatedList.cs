using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Collections.Paged {
  public class PaginatedList<T> : List<T>, IPagedList {

    public PaginatedList(List<T> items, int totalItemCount, int pageNumber, int pageSize) {
      TotalItemCount = items == null ? 0 : totalItemCount;
      PageNumber = Math.Max(0, pageNumber);
      PageSize = Math.Max(1, pageSize);
      if (items != null && TotalItemCount > 0) {
        AddRange(items);
      }
    }

    public int TotalItemCount { get; protected set; }
    public int PageNumber { get; protected set; }
    public int PageSize { get; protected set; }

    public static PaginatedList<T> Create(IQueryable<T> source, int currentPage, int pageSize) {
      var count = source.Count();
      var items = source.SkipTakeToPage(currentPage, pageSize).ToList();
      return new PaginatedList<T>(items, count, currentPage, pageSize);
    }

  }
}
