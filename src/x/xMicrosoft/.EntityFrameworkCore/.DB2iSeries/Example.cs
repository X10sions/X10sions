using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Example;

public class ExampleDbContext : DbContext {
  public ExampleDbContext(DbContextOptions options) : base(options) { }
  public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options) { }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseDB2iSeries("DataSource=your-DB2iSeries-server;UserID=user;Password=pass;DefaultCollection=library");
  }

  public DbSet<ExampleProduct> Products { get; set; }
}

public class ExampleProduct {
  public int Id { get; set; }
  public string Name { get; set; }
  public decimal Price { get; set; }
  public bool IsActive { get; set; }
}


public static class Exmaple {

  public static void ProgramMain() {

    var options = new DbContextOptionsBuilder<ExampleDbContext>()
        .UseDB2iSeries("Driver={IBM i Access ODBC Driver};System=MYISERIES;Uid=USER;Pwd=PASS;Naming=1;DefaultLibraries=MYLIB")
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .Options;

    using var db = new ExampleDbContext(options);
    var active = db.Products.Where(c => c.IsActive).OrderBy(c => c.Name).Take(20).ToList();
  }

}