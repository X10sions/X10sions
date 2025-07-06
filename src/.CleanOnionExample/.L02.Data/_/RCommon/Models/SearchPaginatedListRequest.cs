namespace RCommon.Models;

public class SearchPaginatedListRequest : PaginatedListRequest, ISearchPaginatedListRequest {
  public SearchPaginatedListRequest() {

  }
  public string SearchString { get; set; }
}
