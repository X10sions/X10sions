using LinqToDB.Linq;
using System.Linq.Expressions;

namespace LinqToDB;

public interface IModificationHandler {
  Type EntityType { get; }
}

public interface IModificationHandler<T> : IModificationHandler where T : notnull {
  ITable<T> Table { get; }
  Expression<Func<T, bool>> Predicate { get; set; }

  //IQueryable<T> Queryable { get;  }
  //IQueryable<T> Deletable { get; }
  //IValueInsertable<T> Insertable { get; }
  //IUpdatable<T> Updatable { get; }

  //IDictionary<Expression<Func<T, object>>, object> FieldValues { get; }
  IDictionary<Expression, object> FieldValues { get; }
}

public static class IModificationHandlerExtensions {
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetQueryable().FirstOrDefault();
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler, Expression<Func<T, bool>> predicate) where T : notnull => modificationHandler.GetQueryable().FirstOrDefault(predicate);
  public static T FirstOrInsert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetQueryable().FirstOrInsert(modificationHandler.GetInsertable());
  public static int InsertIfNotExists<T>(this IModificationHandler<T> modificationHandler, T obj) where T : notnull => modificationHandler.Table.InsertIfNotExists(obj, modificationHandler.Predicate);
  public static T InsertWithOutputIfNotExists<T>(this IModificationHandler<T> modificationHandler, T obj) where T : notnull => modificationHandler.Table.InsertWithOutputIfNotExists(obj, modificationHandler.Predicate);

  public static IModificationHandler<T> AddPredicateValue<T, TValue>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TValue>> getField, TValue value)
    where T : notnull
    where TValue : IEquatable<TValue> {
    modificationHandler.Predicate = modificationHandler.Predicate.AndAlso(getField.ToPredicateExpression(value));
    //modificationHandler.Queryable = modificationHandler.Queryable.Where(getField.ToPredicateExpression(value));
    modificationHandler.SetValue(getField, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, TV value) where T : notnull {

    //var valueConstantExpr = Expression.Constant(value, typeof(TV));
    //var fieldQuoted = Expression.Quote(field);

    //var queryU = ((Updatable<T>)modificationHandler.Updatable).Query;
    //queryU = queryU.Provider.CreateQuery<T>(
    //  Expression.Call(null, Methods.LinqToDB.Update.SetUpdatableValue.MakeGenericMethod(typeof(T), typeof(TV)), queryU.Expression, fieldQuoted, valueConstantExpr));
    //var updatable = new Updatable<T>(queryU);

    //var sourceI = modificationHandler.Table;
    //var queryI = (IQueryable<T>)sourceI;
    //var q = queryI.Provider.CreateQuery<T>(
    //  Expression.Call(null, MethodHelper.GetMethodInfo(LinqExtensions.Value, sourceI, field, value), queryI.Expression, fieldQuoted, valueConstantExpr));
    //var Insertable = new ValueInsertable<T>(q);

    modificationHandler.FieldValues.Add(field, value);

    //modificationHandler.Insertable = modificationHandler.Insertable.Value(field, value);
    //modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<T, TV>> value) where T : notnull {
    modificationHandler.FieldValues.Add(field, value);
    //modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<TV>> value) where T : notnull {
    modificationHandler.FieldValues.Add(field, value);
    //modificationHandler.Insertable = modificationHandler.Insertable.Value(field, value);
    //modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static int Delete<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Table.Where(modificationHandler.Predicate).Delete();
  public static int Insert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetInsertable().Insert();
  public static int Update<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetUpdatable().Update();

  public static IQueryable<T> GetQueryable<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Table.Where(modificationHandler.Predicate);

  public static IValueInsertable<T> GetInsertable<T>(this IModificationHandler<T> modificationHandler) where T : notnull {
    var insertable = modificationHandler.Table.AsValueInsertable();//  table.DataContext.Into(table);
    foreach (KeyValuePair<Expression, object> kvp in modificationHandler.FieldValues) {
      Expression<Func<T, object>> field = kvp.Key.GetField<T, object>();
      if (kvp.Value is Expression<Func<object>>) {
        Expression<Func<object>> value = (Expression<Func<object>>)kvp.Value;
        insertable = insertable.Value(field, value);
      } else {
        object value = kvp.Value;
        insertable = insertable.Value(field, value);
      }
    }
    return insertable;
  }

  public static Expression<Func<T, TV>> GetField<T, TV>(this Expression expr) where T : notnull {
    Expression<Func<T, TV>> key = (Expression<Func<T, TV>>)expr;
    return key;
  }

  public static IUpdatable<T> GetUpdatable<T>(this IModificationHandler<T> modificationHandler) where T : notnull {
    var updatable = modificationHandler.Table.Where(modificationHandler.Predicate).AsUpdatable();
    foreach (KeyValuePair<Expression, object> kvp in modificationHandler.FieldValues) {
      Expression<Func<T, object>> field = kvp.Key.GetField<T, object>();
      if (kvp.Value is Expression<Func<object>>) {
        Expression<Func<object>> value = (Expression<Func<object>>)kvp.Value;
        updatable = updatable.Set(field, value);
      } else {
        object value = kvp.Value;
        updatable = updatable.Set(field, value);
      }
    }
    return updatable;
  }

  public static Task<int> DeleteaAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.Table.Where(modificationHandler.Predicate).DeleteAsync(token);
  public static Task<int> InsertAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.GetInsertable().InsertAsync(token);
  public static Task<int> UpdateAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.GetUpdatable().UpdateAsync(token);

}