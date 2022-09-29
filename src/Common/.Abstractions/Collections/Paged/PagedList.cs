namespace Common.Collections.Paged;

public class PagedList<T> : List<T>, IPagedListOptions {
  PagedList(int pageNumber, int? pageSize) {
    PageNumber = pageNumber < 1 ? 1 : pageNumber;
    PageSize = pageSize == null ? null : pageSize < 1 ? 1 : pageSize;
  }

  //PagedList(IPagedListOptions options) : this(options.PageNumber, options.PageSize) {
  //  TotalItemCount = options.TotalItemCount;
  //}

  public PagedList(IEnumerable<T> items, int pageNumber, int? pageSize = null, int? totalItemCount = null) : this(pageNumber, pageSize) {
    TotalItemCount = totalItemCount ?? items.Count();
    AddRange(items);
  }

  public PagedList(IEnumerable<T> items, IPagedListOptions options)
    : this(items, options.PageNumber, options.PageSize, options.TotalItemCount) { }

  //public PagedList(IQueryable<T> query, int pageNumber, int? pageSize = null, CancellationToken token = default)
  //  : this(query.SkipTakeToPage(pageNumber, pageSize).ToListAsync(token).Result, pageNumber, pageSize, query.CountAsync(token).Result) { }

  //public PagedList(IQueryable<T> query, IPagedListOptions options, CancellationToken token = default)
  //  : this(query, options.PageNumber, options.PageSize, token) { }

  public int TotalItemCount { get; }
  public int PageNumber { get; }
  public int? PageSize { get; }

  //public static PaginatedList<T> Create(IQueryable<T> source, int currentPage, int pageSize) {
  //  var count = source.Count();
  //  var items = source.SkipTakeToPage(currentPage, pageSize).ToList();
  //  return new PaginatedList<T>(items, count, currentPage, pageSize);
  //}

}