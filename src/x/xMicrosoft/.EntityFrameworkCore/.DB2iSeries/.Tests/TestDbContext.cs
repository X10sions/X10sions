using Microsoft.EntityFrameworkCore;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests;

public class TestDbContext : DbContext {
  public TestDbContext() { }
  public TestDbContext(DbContextOptions options) : base(options) { }
  public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

  public DbSet<TestEntity> TestEntities { get; set; }
  public DbSet<TestCustomer> TestCustomers => Set<TestCustomer>();
}


public class TestEntity {
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public DateTime CreatedDate { get; set; }
}


public sealed class TestCustomer {
  public Guid Id { get; set; }
  public string Name { get; set; } = "";
}
