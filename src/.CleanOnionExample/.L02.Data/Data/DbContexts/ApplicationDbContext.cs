using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;
using CleanOnionExample.Services;
using Common.Data.DbContexts;
using Common.Data.Entities;
using Common.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Reflection;

namespace CleanOnionExample.Data.DbContexts;

public interface IApplicationDbContext {
  DbSet<Category> Categories { get; set; }
  DbSet<Customer> Customers { get; set; }
  DbSet<Order> Orders { get; set; }
  DbSet<Person> Persons { get; set; }
  DbSet<Product> Products { get; set; }
  DbSet<Supplier> Suppliers { get; set; }
  DbSet<ToDoList> TodoLists { get; }
  DbSet<ToDoItem> TodoItems { get; }

  IDbConnection Connection { get; }
  bool HasChanges { get; }
  EntityEntry Entry(object entity);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}

public class ApplicationDbContext : AuditableContext, IApplicationDbContext {

  public ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    //IIOptions<OperationalStoreOptions> operationalStoreOptions,
    ICurrentUserService currentUserService,
    IDomainEventService domainEventService,
    IDateTimeService dateTime,
    IAuthenticatedUserService authenticatedUser) : base(options) {
    _dateTime = dateTime;
    _authenticatedUser = authenticatedUser;

    _currentUserService = currentUserService;
    _domainEventService = domainEventService;

    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
  }
  private readonly IDateTimeService _dateTime;
  private readonly IAuthenticatedUserService _authenticatedUser;
  private readonly ICurrentUserService _currentUserService;
  private readonly IDomainEventService _domainEventService;

  public DbSet<Customer> Customers { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<Person> Persons { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Supplier> Suppliers { get; set; }
  public DbSet<ToDoList> TodoLists => Set<ToDoList>();
  public DbSet<ToDoItem> TodoItems => Set<ToDoItem>();


  public IDbConnection Connection => Database.GetDbConnection();

  public bool HasChanges => ChangeTracker.HasChanges();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<OrderDetail>().HasKey(o => new { o.OrderId, o.ProductId });

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


    //foreach (var property in modelBuilder.Model.GetEntityTypes()
    //   .SelectMany(t => t.GetProperties())
    //   .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))) {
    //  property.SetColumnType("decimal(18,2)");
    //}

    base.OnModelCreating(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    if (!optionsBuilder.IsConfigured) {
      optionsBuilder
      .UseSqlServer("DataSource=app.db");
    }

  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
    foreach (var entry in ChangeTracker.Entries<EntityAuditableBase<int>>().ToList()) {
      switch (entry.State) {
        case EntityState.Added:
          entry.Entity.InsertDate = _dateTime.Now;
          //entry.Entity.CreatedOn = _dateTime.NowUtc;
          entry.Entity.InsertBy = _authenticatedUser.UserId;
          //entry.Entity.CreatedBy = _currentUserService.UserId;
          break;

        case EntityState.Modified:
          entry.Entity.UpdateDate = _dateTime.Now;
          //entry.Entity.LastModifiedOn = _dateTime.NowUtc;
          entry.Entity.UpdateBy = _authenticatedUser.UserId;
          //entry.Entity.LastModifiedBy = _currentUserService.UserId;
          break;
      }
    }

    var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

    var result = await base.SaveChangesAsync(cancellationToken);

    await DispatchEvents(events);

    if (_authenticatedUser.UserId == null) {
      return await base.SaveChangesAsync(cancellationToken);
    } else {
      return await base.SaveChangesAsync(_authenticatedUser.UserId);
    }
  }


  private async System.Threading.Tasks.Task DispatchEvents(DomainEvent[] events) {
    foreach (var @event in events) {
      @event.IsPublished = true;
      await _domainEventService.Publish(@event);
    }
  }

}
