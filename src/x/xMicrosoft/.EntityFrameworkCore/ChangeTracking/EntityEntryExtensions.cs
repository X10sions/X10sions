using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.ChangeTracking {
  public static class EntityEntryExtensions {
    /// <summary>
    /// The Reload method tells Entity Framework to re-hydrate an already loaded entity from the database, to account for any changes that might have occurred after the entity was loaded by EF.
    /// </summary>
    //public static TEntity Reload<TEntity>(this EntityEntry<TEntity> entry) where TEntity : class {
    //  if (entry.State == EntityState.Detached) {
    //    return entry.Entity;
    //  }
    //  var context = entry.Context;
    //  var entity = entry.Entity;
    //  var keyValues = context.GetEntityKey(entity);
    //  entry.State = EntityState.Detached;
    //  var newEntity = context.Set<TEntity>().Find(keyValues);
    //  var newEntry = context.Entry(newEntity);
    //  foreach (var prop in newEntry.Metadata.GetProperties()) {
    //    prop.GetSetter().SetClrValue(entity, prop.GetGetter().GetClrValue(newEntity));
    //  }
    //  newEntry.State = EntityState.Detached;
    //  entry.State = EntityState.Unchanged;
    //  return entry.Entity;
    //}

    public static void Load<TSource, TDestination>(
      this EntityEntry<TSource> entry,
      Expression<Func<TSource, IEnumerable<TDestination>>> path,
      Expression<Func<TDestination, TSource>> pathBack = null
      ) where TSource : class where TDestination : class {
      var entity = entry.Entity;
      var context = entry.Context;
      var entityType = context.FindEntityType<TSource>();
      var keys = entityType.GetKeys();
      var keyValues = context.GetEntityKey(entity);
      var query = context.Set<TDestination>() as IQueryable<TDestination>;
      var parameter = Expression.Parameter(typeof(TDestination), "x");
      PropertyInfo foreignKeyProperty = null;
      if (pathBack == null) {
        foreignKeyProperty = typeof(TDestination).GetProperties().Single(p => p.PropertyType == typeof(TSource));
      } else {
        foreignKeyProperty = (pathBack.Body as MemberExpression).Member as PropertyInfo;
      }
      var i = 0;
      foreach (var property in keys.SelectMany(x => x.Properties)) {
        var keyValue = keyValues[i];
        var expression = Expression.Lambda(
                Expression.Equal(
                    Expression.Property(Expression.Property(parameter, foreignKeyProperty.Name), property.Name),
                    Expression.Constant(keyValue)),
                parameter) as Expression<Func<TDestination, bool>>;
        query = query.Where(expression);
        i++;
      }
      var list = query.ToList();
      var prop = (path.Body as MemberExpression).Member as PropertyInfo;
      prop.SetValue(entity, list);
    }

  }
}