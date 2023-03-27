using Common.Collections.Paged;

namespace Common.Data;

public class PagedListResult<T> : Result, IPagedListOptions  {
  public PagedListResult(List<T> data) {
    Items = data;
  }

  public PagedListResult(List<T> items, int totalCount, int startRowIndex, int maxRows) {
    Items = items;
    TotalItemCount = totalCount;
    PageSize = maxRows;
    PageNumber = (int)Math.Ceiling((double)(startRowIndex / maxRows)) + 1;
  }

  internal PagedListResult(List<T> items = default, IEnumerable<string>? errorMessages = null, int count = 0, int page = 1, int? pageSize = 10) {
    Items = items;
    if (errorMessages != null) Errors.AddRange(errorMessages);
    PageNumber = page;
    PageSize = pageSize;
    TotalItemCount = count;
  }

  public int PageNumber { get; set; }
  public int? PageSize { get; set; }
  public int TotalItemCount { get; set; }
  public List<T> Items { get; set; }

  public static new PagedListResult<T> Failure(List<string> errorMessages) => new PagedListResult<T>(default, errorMessages);
  public static PagedListResult<T> Success(List<T> data, int count, int page, int pageSize) => new PagedListResult<T>(data, null, count, page, pageSize);

}