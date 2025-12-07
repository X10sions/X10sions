using Microsoft.EntityFrameworkCore.Query;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public class DB2iSeriesQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory {
  private readonly QuerySqlGeneratorDependencies _dependencies;

  public DB2iSeriesQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) {
    _dependencies = dependencies;
  }

  public QuerySqlGenerator Create() => new DB2iSeriesQuerySqlGenerator(_dependencies);
}
