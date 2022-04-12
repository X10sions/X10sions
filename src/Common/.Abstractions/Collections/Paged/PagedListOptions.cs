namespace Common.Collections.Paged;

public class PagedListOptions : IPagedListOptions {
  public PagedListOptions(int pageNumber, int? pageSize, int totalItemCount) {
    PageNumber = pageNumber;
    PageSize = pageSize;
    TotalItemCount = totalItemCount;
  }
  public int TotalItemCount { get; }
  public int PageNumber { get; }
  public int? PageSize { get; }
}

