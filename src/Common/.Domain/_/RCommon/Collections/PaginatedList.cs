namespace RCommon.Collections;

public class PaginatedList<T> : List<T>, IPaginatedList<T> {
  public PaginatedList() { }
  public int PageIndex { get; private set; }
  public int PageSize { get; private set; }
  public int TotalCount { get; private set; }
  public int TotalPages { get; private set; }

  public PaginatedList(IQueryable<T> source, int? pageIndex, int pageSize) {
    PageIndex = pageIndex ?? 1;
    PageSize = pageSize;
    TotalCount = source.Count();
    TotalPages = ((TotalCount - 1) / PageSize) + 1;
    AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));
  }

  public PaginatedList(IList<T> source, int? pageIndex, int pageSize) {
    PageIndex = pageIndex ?? 1;
    PageSize = pageSize;
    TotalCount = source.Count();
    TotalPages = ((TotalCount - 1) / PageSize) + 1;
    AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));
  }

  public PaginatedList(ICollection<T> source, int? pageIndex, int pageSize) {
    PageIndex = pageIndex ?? 1;
    PageSize = pageSize;
    TotalCount = source.Count();
    TotalPages = ((TotalCount - 1) / PageSize) + 1;
    AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));
  }

  public bool HasPreviousPage => (PageIndex > 1);

  public bool HasNextPage => (PageIndex < TotalPages);
}