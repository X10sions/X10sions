using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using X10sions.Fake.Features.ToDo;

namespace CleanOnionExample.Services.EntityFrameworkCore.Configurations;

public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList> {
  public void Configure(EntityTypeBuilder<ToDoList> builder) {
    builder.Property(t => t.Title)
        .HasMaxLength(200)
        .IsRequired();

    builder.Property(b => b.Colour).HasConversion(v => v.Value, v => new(v));
  }
}

