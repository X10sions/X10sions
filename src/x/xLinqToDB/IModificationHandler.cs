using LinqToDB.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
//using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB;

public interface IModificationHandler {
  Type EntityType { get; }
}

public interface IModificationHandler<T> : IModificationHandler where T : notnull {
  //ITable<T> Table { get; }
  IQueryable<T> Queryable { get; set; }
  //IQueryable<T> Deletable { get; }
  IValueInsertable<T> Insertable { get; set; }
  IUpdatable<T> Updatable { get; set; }
}

public static class IModificationHandlerExtensions {
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Queryable.FirstOrDefault();
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler, Expression<Func<T, bool>> predicate) where T : notnull => modificationHandler.Queryable.FirstOrDefault(predicate);

  public static T FirstOrInsert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Queryable.FirstOrInsert(modificationHandler.Insertable);

  public static int InsertIfNotExists<T>(this IModificationHandler<T> modificationHandler, DataContext dataContext, T obj) where T : notnull => modificationHandler.Queryable.InsertIfNotExists(dataContext, obj);

  public static IModificationHandler<T> AddPredicateValue<T, TValue>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TValue>> getField, TValue value)
    where T : notnull
    where TValue : IEquatable<TValue> {
    //var expr = getField.AsExpression();
    //var cp = expr.Compile().Invoke()

    //var expr = getField?.Body as MemberExpression;
    //var prop = expr?.Member as PropertyInfo;
    //var attrs = prop.GetCustomAttribute<ColumnAttribute>();

    //var v = new ReferencedPropertyFinder(typeof(T));
    //v.Visit(getField);
    //var props = v.Properties;

    //?.GetSetMethod(true);

    //modificationHandler.Queryable = modificationHandler.Queryable.Where($"attrs.Name = '{value}'");
    modificationHandler.Queryable = modificationHandler.Queryable.Where(getField.ToPredicateExpression(value));
    //modificationHandler.Queryable = modificationHandler.Queryable.Where(x => prop.GetValue(x).Equals(value));
    //modificationHandler.Queryable = modificationHandler.Queryable.Where(x => getField.Compile().Invoke(x).Equals(value));


    modificationHandler.SetValue(getField, value);
    //modificationHandler.Insertable.Value(x => getField(x), value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, TV value) where T : notnull {
    modificationHandler.Insertable =  modificationHandler.Insertable.Value(field, value);
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<T, TV>> value) where T : notnull {
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<TV>> value) where T : notnull {
    modificationHandler.Insertable = modificationHandler.Insertable.Value(field, value);
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static int Delete<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Queryable.Delete();
  public static int Insert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Insertable.Insert();
  public static int Update<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Updatable.Update();

  public static Task<int> DeleteaAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.Queryable.DeleteAsync(token);
  public static Task<int> InsertAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.Insertable.InsertAsync(token);
  public static Task<int> UpdateAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.Updatable.UpdateAsync(token);

}