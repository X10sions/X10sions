using Microsoft.EntityFrameworkCore;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Example;

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