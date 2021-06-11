namespace SharpRepository.ODataRepository.Linq {
  public static class ODataQueryFactory {
    public static ODataQueryable<T> Queryable<T>(string url, string databaseName) => new ODataQueryable<T>(url, databaseName);
  }
}