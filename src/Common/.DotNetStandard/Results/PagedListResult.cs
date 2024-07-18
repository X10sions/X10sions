using Common.Collections.Paged;

namespace Common.Results;

public class PagedListResult<T> : Result<List<T>>, IPagedListOptions {
  public const int DefaultPageSize = 10;

  public PagedListResult(List<T> data) : base(data, null, []) { }

  public PagedListResult(List<T> data, long totalCount, int startRowIndex, int maxRows) : this(data) {
    TotalItemCount = totalCount;
    PageSize = maxRows;
    PageNumber = (int)Math.Ceiling((double)(startRowIndex / maxRows)) + 1;
  }

  internal PagedListResult(List<T> data = default, IEnumerable<Error>? errors = null, long count = 0, int page = 1, int? pageSize = DefaultPageSize)
    : base(data, null, errors) {
    PageNumber = page;
    PageSize = pageSize;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    TotalItemCount = count;
  }

  public int PageNumber { get; }
  public int? PageSize { get; }
  public long TotalItemCount { get; }
  //[Obsolete("Use Items")] public List<T> Data => Items;
  [Obsolete("Use PageNumber")] public int Page => PageNumber;
  public int TotalPages { get; }
  [Obsolete("Use TotalItemCount")] public long TotalCount => TotalItemCount;
  public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;


  public static PagedListResult<T> Failure(List<Error> errors) => new PagedListResult<T>(null, errors);
  public static PagedListResult<T> Success(List<T> data, long count, int page, int pageSize) => new PagedListResult<T>(data, null, count, page, pageSize);

}

