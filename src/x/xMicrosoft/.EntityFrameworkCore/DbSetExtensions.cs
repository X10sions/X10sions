using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore {

  public static class DbSetExtensions {

    public static void Evict<TEntity>(this DbContext context, TEntity entity) where TEntity : class
      => context.Entry(entity).State = EntityState.Detached;

    public static void Evict<TEntity>(this DbContext context, params object[] keyValues) where TEntity : class {
      //https://weblogs.asp.net/ricardoperes/implementing-missing-features-in-entity-framework-core
      var tracker = context.ChangeTracker;
      var entries = tracker.Entries<TEntity>();
      if(keyValues.Any() == true) {
        var entityType = context.FindEntityType<TEntity>();
        var keys = entityType.GetKeys();
        var i = 0;
        foreach(var property in keys.SelectMany(x => x.Properties)) {
          var keyValue = keyValues[i];
          entries = entries.Where(e => keyValue.Equals(e.Property(property.Name).CurrentValue));
          i++;
        }
      }
      foreach(var entry in entries.ToList()) {
        entry.State = EntityState.Detached;
      }
    }

    public static TEntity Find<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class {
      var context = set.GetDbContext();
      var entityType = context.FindEntityType<TEntity>();
      var keys = entityType.GetKeys();
      var entries = context.ChangeTracker.Entries<TEntity>();
      var parameter = Expression.Parameter(typeof(TEntity), "x");
      IQueryable<TEntity> query = context.Set<TEntity>();
      //first, check if the entity exists in the cache
      var i = 0;
      //iterate through the key properties
      foreach(var property in keys.SelectMany(x => x.Properties)) {
        var keyValue = keyValues[i];
        //try to get the entity from the local cache
        entries = entries.Where(e => keyValue.Equals(e.Property(property.Name).CurrentValue));
        //build a LINQ expression for loading the entity from the store
        var expression = Expression.Lambda(Expression.Equal(Expression.Property(parameter, property.Name), Expression.Constant(keyValue)), parameter) as Expression<Func<TEntity, bool>>;
        query = query.Where(expression);
        i++;
      }
      var entity = entries.Select(x => x.Entity).FirstOrDefault();
      if(entity != null) {
        return entity;
      }
      //second, try to load the entity from the data store
      entity = query.FirstOrDefault();
      return entity;
    }

    public static IEntityType FindEntityType<TEntity>(this DbSet<TEntity> set) where TEntity : class
      => set.GetDbContext().FindEntityType<TEntity>();

    public static DbContext GetDbContext<TEntity>(this DbSet<TEntity> set) where TEntity : class
      => set.GetInfrastructure().GetService<IDbContextServices>().CurrentContext.Context;

    public static IAnnotation GetAnnotation<T>(this DbSet<T> dbSet, string annotationName) where T : class {
      var dbContext = dbSet.GetDbContext();
      var model = dbContext.Model;
      var entityTypes = model.GetEntityTypes();
      var entityType = entityTypes.First(t => t.ClrType == typeof(T));
      var annotation = entityType.GetAnnotation(annotationName);
      return annotation;
    }

    public static string GetSchema<T>(this DbSet<T> dbSet) where T : class
      => dbSet.GetAnnotation(RelationalAnnotationNames.Schema).Value.ToString();

    public static string GetTableName<T>(this DbSet<T> dbSet) where T : class
      => dbSet.GetAnnotation(RelationalAnnotationNames.TableName).Value.ToString();

    /// <summary>
    /// Retrieve cached entities that were previously loaded.
    /// </summary>
    public static IEnumerable<EntityEntry<TEntity>> Local<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class {
      var context = set.GetDbContext();
      var entries = context.ChangeTracker.Entries<TEntity>();
      if(keyValues.Any() == true) {
        var entityType = context.FindEntityType<TEntity>();
        var keys = entityType.GetKeys();
        var i = 0;
        foreach(var property in keys.SelectMany(x => x.Properties)) {
          var keyValue = keyValues[i];
          entries = entries.Where(e => keyValue.Equals(e.Property(property.Name).CurrentValue));
          i++;
        }
      }
      return entries;
    }

  }
}