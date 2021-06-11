using System;
using System.Collections.Generic;

namespace Common.Collections.Paged {

  public interface IPagedList {
    int TotalItemCount { get; }
    int PageNumber { get; }
    int PageSize { get; }
  }

  public interface IPagedList<out T> : IPagedList, IEnumerable<T> {
    T this[int index] { get; }
    int Count { get; }
  }

  public static class IPagedListExtensions {
    public static int PageCount(this IPagedList pagesList) => pagesList.TotalItemCount > 0 ? (int)Math.Ceiling(decimal.Divide(pagesList.TotalItemCount, pagesList.PageSize)) : 0;
    public static int NumberOfLastItemOnPage(this IPagedList pagesList) => pagesList.FirstItemOnPage() + pagesList.PageSize - 1;
    public static int FirstItemOnPage(this IPagedList pagesList) => (pagesList.PageNumber - 1) * pagesList.PageSize + 1;
    public static int LastItemOnPage(this IPagedList pagesList) => pagesList.NumberOfLastItemOnPage() > pagesList.TotalItemCount ? pagesList.TotalItemCount : pagesList.NumberOfLastItemOnPage();
    public static bool HasPreviousPage(this IPagedList pagesList) => pagesList.PageNumber > 1;
    public static bool HasNextPage(this IPagedList pagesList) => pagesList.PageNumber < pagesList.PageCount();
    public static bool IsFirstPage(this IPagedList pagesList) => pagesList.PageNumber <= 1;
    public static bool IsLastPage(this IPagedList pagesList) => pagesList.PageNumber >= pagesList.PageCount();
  }

}