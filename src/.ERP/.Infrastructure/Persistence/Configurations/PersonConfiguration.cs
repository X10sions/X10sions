using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using X10sions.ERP.Domain.Models;

namespace X10sions.ERP.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person> {
  public void Configure(EntityTypeBuilder<Person> builder) {
    builder.ToTable("person");

    builder.HasKey(x => x.Id);
    //        .HasColumnName("so_id")
    //        .ValueGeneratedOnAdd();

    //    builder.HasIndex(p => p.SKU).IsUnique();
    builder.Property(p => p.FirstName).IsRequired().HasMaxLength(200);
    builder.Property(p => p.LastName).IsRequired().HasMaxLength(200);


    //builder.HasIndex(e => e.OrderNumber).IsUnique().HasDatabaseName("idx_so_number");

    // 4. Default Values & Enums
    //builder.Property(e => e.Status).HasColumnName("so_status").HasMaxLength(50).HasDefaultValue("DRAFT");

    //builder.Property(e => e.OrderDate).HasColumnName("so_date").HasDefaultValueSql("CURRENT_TIMESTAMP");

    // 5. Handling Private Fields / Encapsulation
    // Tell EF to use the backing field for the 'Lines' collection
    //builder.Metadata.FindNavigation(nameof(SalesOrder.Lines))?.SetPropertyAccessMode(PropertyAccessMode.Field);

    // 6. Relationships
    //builder.HasMany(e => e.Lines).WithOne().HasForeignKey("so_id").OnDelete(DeleteBehavior.Cascade);
  }
}
