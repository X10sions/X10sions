using Microsoft.EntityFrameworkCore;

namespace X10sions.Examples.Delta;

public record Employee(int Id, DateTime RowVersion, int CompanyId, string? Content, int Age) {
  public Company Company { get; }
}

public record Company(int Id, DateTime RowVersion, string? Content) {
  public IList<Employee> Employees { get; }
}

public class SampleDbContext(DbContextOptions options) : DbContext(options) {
  public DbSet<Employee> Employees { get; set; } = null!;
  public DbSet<Company> Companies { get; set; } = null!;

  DatabaseTypes databaseType = DatabaseTypes.SqlServer;
  enum DatabaseTypes { SqlServer, Postgres }

  protected override void OnModelCreating(ModelBuilder builder) {
    var company = builder.Entity<Company>();
    company.HasKey(_ => _.Id);
    company.HasMany(_ => _.Employees).WithOne(_ => _.Company).IsRequired();

    var employee = builder.Entity<Employee>();
    employee.HasKey(_ => _.Id);

    if (databaseType == DatabaseTypes.SqlServer) {
      company.Property(_ => _.RowVersion).IsRowVersion().HasConversion<byte[]>();
      employee.Property(_ => _.RowVersion).IsRowVersion().HasConversion<byte[]>();
    }
    if (databaseType == DatabaseTypes.Postgres) {
    }

  }
}
