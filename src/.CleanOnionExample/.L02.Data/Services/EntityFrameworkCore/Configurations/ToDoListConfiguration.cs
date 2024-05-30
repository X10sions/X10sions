using CleanOnionExample.Data.Entities;
using Common.Features.DummyFakeExamples.ToDo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanOnionExample.Services.EntityFrameworkCore.Configurations;

public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList> {
  public void Configure(EntityTypeBuilder<ToDoList> builder) {
    builder.Property(t => t.Title)
        .HasMaxLength(200)
        .IsRequired();

    builder
        .OwnsOne(b => b.Colour);
  }
}

