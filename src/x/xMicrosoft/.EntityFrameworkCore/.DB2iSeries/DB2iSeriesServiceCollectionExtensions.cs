using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Odbc;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Infrastructure;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Migrations;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Query;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Storage;
using xMicrosoft.EntityFrameworkCore.DB2iSeries.Update;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public static class DB2iSeriesServiceCollectionExtensions {

  public static IServiceCollection AddEntityFrameworkDB2iSeries(
      this IServiceCollection services) {
    var builder = new EntityFrameworkRelationalServicesBuilder(services)
        .TryAdd<IDatabaseProvider, DatabaseProvider<DB2iSeriesOptionsExtension>>()
        .TryAdd<IRelationalTypeMappingSource, DB2iSeriesTypeMappingSource>()
        .TryAdd<ISqlGenerationHelper, DB2iSeriesSqlGenerationHelper>()
        .TryAdd<IRelationalConnection, DB2iSeriesOdbcRelationalConnection>()
        .TryAdd<IMigrationsSqlGenerator, DB2iSeriesMigrationsSqlGenerator>()
        .TryAdd<IUpdateSqlGenerator, DB2iSeriesUpdateSqlGenerator>()
        .TryAdd<IModificationCommandBatchFactory, DB2iSeriesModificationCommandBatchFactory>()
        .TryAdd<IQuerySqlGeneratorFactory, DB2iSeriesQuerySqlGeneratorFactory>()
        .TryAdd<IRelationalDatabaseCreator, DB2iSeriesDatabaseCreator>()
        .TryAdd<IQueryTranslationPostprocessorFactory, DB2iSeriesQueryTranslationPostprocessorFactory>();

    // EFCore.ISeries/ISeriesOptionsExtension.cs (ApplyServices)
    services.AddScoped(sp => {
      var conn = sp.GetRequiredService<IRelationalConnection>().DbConnection as OdbcConnection;
      if (conn == null || conn.State != System.Data.ConnectionState.Open) {
        conn = new OdbcConnection(sp.GetRequiredService<DB2iSeriesOptionsExtension>().ConnectionString);
        conn.Open();
      }
      return DB2iSeriesServerCapabilities.Detect(conn);
    });

    builder.TryAddCoreServices();
    return services;
  }

}