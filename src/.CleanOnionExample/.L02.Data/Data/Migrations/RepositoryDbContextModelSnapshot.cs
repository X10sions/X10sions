﻿using CleanOnionExample.Data.DbContexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CleanOnionExample.Data.Migrations {
  [DbContext(typeof(RepositoryDbContext))]
  sealed class RepositoryDbContextModelSnapshot : ModelSnapshot {
    protected override void BuildModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("Relational:MaxIdentifierLength", 63)
          .HasAnnotation("ProductVersion", "5.0.7")
          .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

      modelBuilder.Entity("Domain.Entities.Account", b => {
        b.Property<Guid>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("uuid");

        b.Property<string>("AccountType")
            .HasMaxLength(50)
            .HasColumnType("character varying(50)");

        b.Property<DateTime>("DateCreated")
            .HasColumnType("timestamp without time zone");

        b.Property<Guid>("OwnerId")
            .HasColumnType("uuid");

        b.HasKey("Id");

        b.HasIndex("OwnerId");

        b.ToTable("Account");
      });

      modelBuilder.Entity("Domain.Entities.Owner", b => {
        b.Property<Guid>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("uuid");

        b.Property<string>("Address")
            .HasMaxLength(100)
            .HasColumnType("character varying(100)");

        b.Property<DateTime>("DateOfBirth")
            .HasColumnType("timestamp without time zone");

        b.Property<string>("Name")
            .HasMaxLength(60)
            .HasColumnType("character varying(60)");

        b.HasKey("Id");

        b.ToTable("Owner");
      });

      modelBuilder.Entity("Domain.Entities.Account", b => {
        b.HasOne("Domain.Entities.Owner", null)
            .WithMany("Accounts")
            .HasForeignKey("OwnerId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
      });

      modelBuilder.Entity("Domain.Entities.Owner", b => {
        b.Navigation("Accounts");
      });
#pragma warning restore 612, 618
    }
  }
}