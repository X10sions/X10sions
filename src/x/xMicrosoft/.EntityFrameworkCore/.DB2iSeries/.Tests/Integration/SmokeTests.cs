using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Tests.Integration;

// EFCore.ISeries.Tests/Integration/SmokeTests.cs


public class SmokeTests {
  [Fact(Skip = "Requires live IBM i v5r4")]
  public void basic_crud_works() {
    var opts = new DbContextOptionsBuilder<AppDb>().UseDB2iSeries("Driver={IBM i Access ODBC Driver};System=MYISERIES;Uid=USER;Pwd=PASS;Naming=1;DefaultLibraries=MYLIB").Options;
    using var db = new AppDb(opts);
    db.Database.EnsureCreated();
    var c = new Customer { Id = Guid.NewGuid(), Name = "A", CreatedAt = DateTime.Now };
    db.Customers.Add(c);
    db.SaveChanges();
    var found = db.Customers.Where(x => x.Name.ToUpper() == "A").FirstOrDefault();
    Assert.NotNull(found);
  }

  public sealed class AppDb : DbContext {
    public AppDb(DbContextOptions options) : base(options) { }
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }
    public DbSet<Customer> Customers => Set<Customer>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Customer>(e => {
        e.ToTable("CUSTOMER", "MYLIB");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnType("CHAR(36)");
        e.Property(x => x.CreatedAt).HasColumnType("TIMESTAMP");
        e.Property(x => x.IsActive).HasColumnType("SMALLINT");
      });
    }
  }

  public sealed class Customer {
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
  }

}