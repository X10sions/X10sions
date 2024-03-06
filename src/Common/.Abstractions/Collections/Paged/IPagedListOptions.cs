namespace Common.Collections.Paged;

public interface IPagedListOptions {
  long TotalItemCount { get; }
  int PageNumber { get; }
  int? PageSize { get; }
}

public static class IPagedListOptionsExtensions {
  public static int PageCount(this IPagedListOptions options) => options.TotalItemCount > 0 ? options.PageSize.HasValue ? (int)Math.Ceiling(decimal.Divide(options.TotalItemCount, options.PageSize.Value)) : 1 : 0;
  public static long NumberOfLastItemOnPage(this IPagedListOptions options) => options.PageSize.HasValue ? options.FirstItemOnPage() + options.PageSize.Value - 1 : options.TotalItemCount;
  public static int FirstItemOnPage(this IPagedListOptions options) => options.PageSize.HasValue ? (options.PageNumber - 1) * options.PageSize.Value + 1 : 1;
  public static long LastItemOnPage(this IPagedListOptions options) => options.NumberOfLastItemOnPage() > options.TotalItemCount ? options.TotalItemCount : options.NumberOfLastItemOnPage();
  public static bool HasPreviousPage(this IPagedListOptions options) => options.PageNumber > 1;
  public static bool HasNextPage(this IPagedListOptions options) => options.PageNumber < options.PageCount();
  public static bool IsFirstPage(this IPagedListOptions options) => options.PageNumber <= 1;
  public static bool IsLastPage(this IPagedListOptions options) => options.PageNumber >= options.PageCount();
  public static int? GetSkip(this IPagedListOptions options) => options.PageSize.HasValue ? ((options.PageNumber < 1 ? 1 : options.PageNumber) - 1) * options.PageSize.Value : null;
  public static int? GetTake(this IPagedListOptions options) => options.PageSize;
  // IQueryable
  public static IQueryable<T> Skip<T>(this IQueryable<T> query, IPagedListOptions options) => query.Skip(options.GetSkip());
  public static IQueryable<T> SkipTakeToPage<T>(this IQueryable<T> query, IPagedListOptions options) => query.Skip(options).Take(options);
  public static IQueryable<T> SkipTakeToPage<T>(this IQueryable<T> query, int pageNumber, int? pageSize) => query.SkipTakeToPage(new PagedListOptions(pageNumber, pageSize, 0));
  public static IQueryable<T> Take<T>(this IQueryable<T> query, IPagedListOptions options) => query.Take(options.GetTake());
}