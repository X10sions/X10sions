using Microsoft.EntityFrameworkCore.DB2iSeries.Infrastructure;
using Microsoft.EntityFrameworkCore.DB2iSeries.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace Microsoft.EntityFrameworkCore.DB2iSeries {

  #region /Extensions

  public static class DB2iSeriesDbContextOptionsBuilderExtensions {
    //private static DB2iSeriesOptionsExtension GetOrCreateExtension(this DbContextOptionsBuilder optionsBuilder) => optionsBuilder.GetOrCreateExtension<DB2iSeriesOptionsExtension>();

    static DbContextOptionsBuilder xUseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null, Func<DB2iSeriesOptionsExtension, DB2iSeriesOptionsExtension>? extensionWithActions = null) => optionsBuilder.Use(x => new DB2iSeriesDbContextOptionsBuilder(optionsBuilder), optionsAction, extensionWithActions);

    public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) => optionsBuilder.xUseDB2iSeries(optionsAction);
    public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, string? connectionString, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) => optionsBuilder.xUseDB2iSeries(optionsAction, x => (DB2iSeriesOptionsExtension)x.WithConnectionString(connectionString));
    public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, DbConnection connection, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) => optionsBuilder.xUseDB2iSeries(optionsAction, x => (DB2iSeriesOptionsExtension)x.WithConnection(connection));

    public static DbContextOptionsBuilder<TContext> UseDB2iSeries<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) where TContext : DbContext => (DbContextOptionsBuilder<TContext>)UseDB2iSeries((DbContextOptionsBuilder)optionsBuilder, optionsAction);
    public static DbContextOptionsBuilder<TContext> UseDB2iSeries<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, string? connectionString, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) where TContext : DbContext => (DbContextOptionsBuilder<TContext>)UseDB2iSeries((DbContextOptionsBuilder)optionsBuilder, connectionString, optionsAction);
    public static DbContextOptionsBuilder<TContext> UseDB2iSeries<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, DbConnection connection, Action<DB2iSeriesDbContextOptionsBuilder>? optionsAction = null) where TContext : DbContext => (DbContextOptionsBuilder<TContext>)UseDB2iSeries((DbContextOptionsBuilder)optionsBuilder, connection, optionsAction);

  }

  #endregion

  namespace Infrastructure {
    public class DB2iSeriesDbContextOptionsBuilder : RelationalDbContextOptionsBuilder<DB2iSeriesDbContextOptionsBuilder, DB2iSeriesOptionsExtension> {
      public DB2iSeriesDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder) : base(optionsBuilder) { }
    }

    namespace Internal {

      public class DB2iSeriesOptionsExtension : RelationalOptionsExtension {
        public override DbContextOptionsExtensionInfo Info => throw new NotImplementedException();
        public override void ApplyServices(IServiceCollection services) => throw new NotImplementedException();
        protected override RelationalOptionsExtension Clone() => throw new NotImplementedException();
      }
    }


  }

}


