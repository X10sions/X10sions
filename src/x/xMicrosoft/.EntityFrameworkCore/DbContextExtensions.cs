using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore {
  public static class DbContextExtensions {

    //private static readonly MethodInfo SetMethod = typeof(DbContext).GetTypeInfo().GetDeclaredMethod("Set");

    //public static object Find(this DbContext context, Type entityType, params object[] keyValues) {
    //  dynamic set = SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    //  return set.Find(keyValues);
    //}

    public static IEntityType FindEntityType<T>(this DbContext context) => context.Model.FindEntityType(typeof(T));

    public static IEnumerable<string> FindPrimaryKeyNames<T>(this DbContext dbContext, T entity)
      => from p in dbContext.FindPrimaryKeyProperties(entity) select p.Name;

    public static IEnumerable<object> FindPrimaryKeyValues<T>(this DbContext dbContext, T entity)
      => from p in dbContext.FindPrimaryKeyProperties(entity) select entity.GetTypePropertyValueAs(p.Name);

    public static IKey FindPrimaryKey<T>(this DbContext dbContext, T entity)
     => dbContext.FindEntityType<T>().FindPrimaryKey();

    public static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T entity)
      => dbContext.FindPrimaryKey(entity).Properties;


    public static object[] GetEntityKey<T>(this DbContext context, T entity) where T : class {
      var state = context.Entry(entity);
      var metadata = state.Metadata;
      var key = metadata.FindPrimaryKey();
      var props = key.Properties.ToArray();
      return props.Select(x => x.GetGetter().GetClrValue(entity)).ToArray();
    }

    //public static IRelationalEntityTypeAnnotations GetEntityRelationalMetaData<TEntity>(this DbContext dbContext)
    //  => dbContext.Model.FindEntityType(typeof(TEntity)).Relational();

    //public static IProperty GetDiscriminatorProperty<TEntity>(this DbContext dbContext)
    //  => dbContext.GetEntityRelationalMetaData<TEntity>().DiscriminatorProperty;

    //public static object GetDiscriminatorValue<TEntity>(this DbContext dbContext)
    //  => dbContext.GetEntityRelationalMetaData<TEntity>().DiscriminatorValue;

    //public static string GetSchema<TEntity>(this DbContext dbContext)
    //  => dbContext.GetEntityRelationalMetaData<TEntity>().Schema;

    //public static string GetTableName<TEntity>(this DbContext dbContext)
    //  => dbContext.GetEntityRelationalMetaData<TEntity>().TableName;

    //public static string GetUserName(this DbContext dbContext) {
    //  throw new NotImplementedException("MoveTo NetCoreApp");//TODO
    //  //var httpContext = dbContext.GetService<IHttpContextAccessor>();
    //  //return httpContext.HttpContext.User.Identity.Name;
    //}

    /// <summary>
    /// The Reload method tells Entity Framework to re-hydrate an already loaded entity from the database, to account for any changes that might have occurred after the entity was loaded by EF.
    /// </summary>
    //public static TEntity Reload<TEntity>(this DbContext context, TEntity entity) where TEntity : class {
    //  context.Entry(entity).Reload();
    //  return entity;
    //}


  }
}