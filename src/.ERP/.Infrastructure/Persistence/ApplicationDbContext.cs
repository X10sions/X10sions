using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using X10sions.ERP.Domain.Models;
using X10sions.ERP.Infrastructure.Persistence.Configurations;

namespace X10sions.ERP.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext {
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options) {


  }

  public DbSet<BusinessEntity> BusinessEntity { get; set; }
  public DbSet<Person> Person { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    // This automatically finds and applies all configuration classes in the assembly
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    modelBuilder.ApplyConfiguration(new PersonConfiguration());


    BuildIndexes(modelBuilder);
    BuildRelationships(modelBuilder);
  }

  void BuildIndexes(ModelBuilder modelBuilder) {
    // Define Indexes (Cleanly separated from the model)
    //modelBuilder.Entity<Person>().HasIndex(u => new { u.FirstName, u.LastName }).IsUnique();
  }

  void BuildRelationships(ModelBuilder modelBuilder) {
    // Define Relationship behavior (e.g., Restrict Delete)
    //modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
  }


}
