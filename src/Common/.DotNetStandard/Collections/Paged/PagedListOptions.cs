namespace Common.Collections.Paged;

public class PagedListOptions : IPagedListOptions {
  public PagedListOptions(int pageNumber, int? pageSize, long totalItemCount) {
    PageNumber = pageNumber;
    PageSize = pageSize;
    TotalItemCount = totalItemCount;
  }
  public long TotalItemCount { get; }
  public int PageNumber { get; }
  public int? PageSize { get; }
}

