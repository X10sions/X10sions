using CleanOnionExample.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanOnionExample.Services.EntityFrameworkCore.Configurations;
internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account> {
  public void Configure(EntityTypeBuilder<Account> builder) {
    builder.ToTable(nameof(Account));
    builder.HasKey(account => account.Id);
    builder.Property(account => account.Id).ValueGeneratedOnAdd();
    builder.Property(account => account.AccountType).HasMaxLength(50);
    builder.Property(account => account.DateCreated).IsRequired();
  }
}

