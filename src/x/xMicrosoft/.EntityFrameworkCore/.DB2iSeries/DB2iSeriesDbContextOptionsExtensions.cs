using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.Odbc;
namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public static class DB2iSeriesDbContextOptionsExtensions {

  public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, string connectionString, Action<DB2iSeriesDbContextOptionsBuilder>? DB2iSeriesOptionsAction = null) {
    var extension = GetOrCreateExtension(optionsBuilder);
    extension = extension.WithConnectionString(connectionString);
    ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
    DB2iSeriesOptionsAction?.Invoke(new DB2iSeriesDbContextOptionsBuilder(optionsBuilder));
    return optionsBuilder;
  }

  public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder builder, OdbcConnection connection) {
    var ext = GetOrCreateExtension(builder);
    ext.ExistingConnection = connection;
    ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(ext);
    return builder;
  }

  public static DbContextOptionsBuilder UseNamingConvention(this DbContextOptionsBuilder optionsBuilder, NamingConvention namingConvention, Action<DB2iSeriesDbContextOptionsBuilder>? DB2iSeriesOptionsAction = null) {
    var extension = GetOrCreateExtension(optionsBuilder);
    extension = extension.WithNamingConvention(namingConvention);
    ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
    DB2iSeriesOptionsAction?.Invoke(new DB2iSeriesDbContextOptionsBuilder(optionsBuilder));
    return optionsBuilder;
  }

  private static DB2iSeriesOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.Options.FindExtension<DB2iSeriesOptionsExtension>() ?? new DB2iSeriesOptionsExtension();

}
