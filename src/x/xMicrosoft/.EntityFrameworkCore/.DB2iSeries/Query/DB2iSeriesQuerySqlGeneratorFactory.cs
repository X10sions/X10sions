using Microsoft.EntityFrameworkCore.Query;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Infrastructure;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;

public class DB2iSeriesQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory {
  private readonly QuerySqlGeneratorDependencies _dependencies;
  private readonly DB2iSeriesServerCapabilities _capabilities;

  public DB2iSeriesQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies, DB2iSeriesServerCapabilities capabilities) {
    _dependencies = dependencies;
    _capabilities = capabilities;
  }

  public QuerySqlGenerator Create() => new DB2iSeriesQuerySqlGenerator(_dependencies, _capabilities);

}
