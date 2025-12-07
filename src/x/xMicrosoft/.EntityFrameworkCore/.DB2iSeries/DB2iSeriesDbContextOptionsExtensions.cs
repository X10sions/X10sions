using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

public static class DB2iSeriesDbContextOptionsExtensions {


  /*

    public class ExampleMyDbContext : DbContext {
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseDB2iSeries("DataSource=your-DB2iSeries-server;UserID=user;Password=pass;DefaultCollection=library");
      }

      public DbSet<ExampleProduct> Products { get; set; }
    }

    public class ExampleProduct {
      public int Id { get; set; }
      public string Name { get; set; }
      public decimal Price { get; set; }
    }
   
    // Usage:
    using (var context = new MyDbContext()) {
      var products = context.Products.Where(p => p.Price > 10).ToList();
    }

   */

  public static DbContextOptionsBuilder UseDB2iSeries(this DbContextOptionsBuilder optionsBuilder, string connectionString, Action<DB2iSeriesDbContextOptionsBuilder> DB2iSeriesOptionsAction = null) {
    var extension = GetOrCreateExtension(optionsBuilder);
    extension = extension.WithConnectionString(connectionString);
    ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
    DB2iSeriesOptionsAction?.Invoke(new DB2iSeriesDbContextOptionsBuilder(optionsBuilder));
    return optionsBuilder;
  }

  private static DB2iSeriesOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.Options.FindExtension<DB2iSeriesOptionsExtension>() ?? new DB2iSeriesOptionsExtension();

}