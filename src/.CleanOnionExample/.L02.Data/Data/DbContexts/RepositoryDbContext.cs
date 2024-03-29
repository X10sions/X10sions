﻿using CleanOnionExample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanOnionExample.Data.DbContexts;

public interface IRepositoryDbContext {
  DbSet<Account> Accounts { get; set; }
  DbSet<Owner> Owners { get; set; }

  void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);

}

public class RepositoryDbContext : DbContext, IRepositoryDbContext {
  public RepositoryDbContext(DbContextOptions options) : base(options) {
    //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
  }

  public DbSet<Owner> Owners { get; set; }
  public DbSet<Account> Accounts { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
}
