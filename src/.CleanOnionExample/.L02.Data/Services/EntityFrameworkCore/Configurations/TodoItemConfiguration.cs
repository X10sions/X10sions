﻿using CleanOnionExample.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanOnionExample.Services.EntityFrameworkCore.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<ToDoItem> {
  public void Configure(EntityTypeBuilder<ToDoItem> builder) {
    builder.Ignore(e => e.DomainEvents);

    builder.Property(t => t.Title)
        .HasMaxLength(200)
        .IsRequired();
  }
}

