using LinqToDB.Async;
using LinqToDB.Linq;
using LinqToDB.Mapping;
using System.Linq.Expressions;

namespace LinqToDB;

public interface IModificationHandler {
  Type EntityType { get; }
}

public interface IModificationHandler<T> : IModificationHandler where T : notnull {
  //IDictionary<LambdaExpression, object> ExpressionValues { get; }
  IDataContext DataContext { get; }
  Expression<Func<T, bool>> Predicate { get; set; }
  ITable<T> Table { get; }
  ITable<T> TableHint([SqlQueryDependent] string hint);
  IQueryable<T> TableWherePredicate { get; }
  IValueInsertable<T> ValueInsertable { get; set; }
  IUpdatable<T> Updatable { get; set; }
}

public static class IModificationHandlerExtensions {
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetQueryable().FirstOrDefault();
  public static async Task<T> FirstOrDefaultAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken cancellationToken) where T : notnull => await modificationHandler.GetQueryable().FirstOrDefaultAsync(cancellationToken);
  public static T FirstOrDefault<T>(this IModificationHandler<T> modificationHandler, Expression<Func<T, bool>> predicate) where T : notnull => modificationHandler.GetQueryable().FirstOrDefault(predicate);
  public static T FirstOrInsert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetQueryable().FirstOrInsert(modificationHandler.GetValueInsertable());
  public static int InsertIfNotExists<T>(this IModificationHandler<T> modificationHandler, T obj) where T : notnull => modificationHandler.Table.InsertIfNotExists(obj, modificationHandler.Predicate);
  public async static Task<int?> InsertWithInt32IdentityAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken cancellation) where T : notnull => await modificationHandler.ValueInsertable.InsertWithInt32IdentityAsync(cancellation);
  public async static Task<int?> InsertWithInt32IdentityAsync<T>(this IModificationHandler<T> modificationHandler, T obj, CancellationToken cancellation) where T : notnull => await modificationHandler.ValueInsertable.InsertWithInt32IdentityAsync(cancellation);
  public async static Task<T> InsertWithOutputAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken cancellation) where T : notnull => await modificationHandler.ValueInsertable.InsertWithOutputAsync(cancellation);
  public async static Task<T> InsertWithOutputAsync<T>(this IModificationHandler<T> modificationHandler, T obj, CancellationToken cancellation) where T : notnull => await modificationHandler.ValueInsertable.InsertWithOutputAsync(cancellation);
  public static T InsertWithOutputIfNotExists<T>(this IModificationHandler<T> modificationHandler, T obj) where T : notnull => modificationHandler.Table.FirstOrInsertWithOutput(obj, modificationHandler.Predicate);

  //public static IModificationHandler<T> AddPredicateValue<T, TValue>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TValue>> getField, TValue value) where T : notnull {
  //  modificationHandler.Predicate = modificationHandler.Predicate.AndAlso(getField.Equal(value));
  //  //modificationHandler.Queryable = modificationHandler.Queryable.Where(getField.ToPredicateExpression(value));
  //  modificationHandler.SetValue(getField, value);
  //  return modificationHandler;
  //}

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, TV value) where T : notnull {
    modificationHandler.ValueInsertable = modificationHandler.ValueInsertable.Value(field, value);
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<T, TV>> value) where T : notnull {
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IModificationHandler<T> SetValue<T, TV>(this IModificationHandler<T> modificationHandler, Expression<Func<T, TV>> field, Expression<Func<TV>> value) where T : notnull {
    modificationHandler.ValueInsertable = modificationHandler.ValueInsertable.Value(field, value);
    modificationHandler.Updatable = modificationHandler.Updatable.Set(field, value);
    return modificationHandler;
  }

  public static IQueryable<T> GetQueryable<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.Table.Where(modificationHandler.Predicate);

  public static IValueInsertable<T> GetValueInsertable<T>(this IModificationHandler<T> modificationHandler) where T : notnull {
    return modificationHandler.ValueInsertable;
    //var insertable = modificationHandler.Table.AsValueInsertable();//  table.DataContext.Into(table);
    //foreach (var kvp in modificationHandler.ExpressionValues) {
    //  var exprType = kvp.Key.Body.Type;
    //  var valueType = kvp.Value.GetType();
    //  Console.WriteLine($"GetInsertable:({exprType}){kvp.Key} => ({valueType}){kvp.Value}");

    //  insertable = insertable.ValueLambdaByType(kvp.Key, kvp.Value);
    //  //insertable = insertable.ValueLambda(kvp.Key, kvp.Value);
    //  //insertable = insertable.ValueLambda(kvp.Key, Convert.ChangeType(kvp.Value, exprType));

    //  //if (kvp.Value is Expression<Func<object>> exprFuncValue) {
    //  //  insertable = insertable.ValueLambda(expression, exprFuncValue);
    //  //} else {
    //  //  insertable = insertable.ValueLambda(expression, kvp.Value);
    //  //}
    //}
    //return insertable;
  }

  public static IUpdatable<T> GetUpdatable<T>(this IModificationHandler<T> modificationHandler) where T : notnull {
    return modificationHandler.Updatable;
    //var updatable = modificationHandler.GetQueryable().AsUpdatable();
    //foreach (var kvp in modificationHandler.ExpressionValues) {
    //  var exprType = kvp.Key.Body.Type;
    //  var valueType = kvp.Value.GetType();
    //  Console.WriteLine($"GetUpdatable:({exprType}){kvp.Key} => ({valueType}){kvp.Value}");

    //  updatable = updatable.SetLambdaByType(kvp.Key, kvp.Value);
    //  //updatable = updatable.SetLambda(kvp.Key, kvp.Value);
    //  //updatable = updatable.SetLambda(kvp.Key, Convert.ChangeType(kvp.Value, exprType));

    //  //if (kvp.Value is Expression<Func<object>> exprFuncValue) {
    //  //  updatable = updatable.SetLambda(expression, exprFuncValue);
    //  //} else {
    //  //  updatable = updatable.SetLambda(expression, value);
    //  //}
    //}
    //return updatable;
  }

  public static int Delete<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetQueryable().Delete();
  public static Task<int> DeleteAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.GetQueryable().DeleteAsync(token);
  public static int Insert<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetValueInsertable().Insert();
  public static Task<int> InsertAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.GetValueInsertable().InsertAsync(token);
  public static int Update<T>(this IModificationHandler<T> modificationHandler) where T : notnull => modificationHandler.GetUpdatable().Update();
  public static Task<int> UpdateAsync<T>(this IModificationHandler<T> modificationHandler, CancellationToken token = default) where T : notnull => modificationHandler.GetUpdatable().UpdateAsync(token);
  public static IModificationHandler<T> UpdateWithAudit<T, TAudit>(this IModificationHandler<T> modificationHandler,
    Expression<Func<T, TAudit>> beforeSetter,
    Expression<Func<T, TAudit>> afterSetter
    ) where TAudit : class {
    // Insert Audit BEFORE record
    var qryBefore = modificationHandler.TableWherePredicate.Insert(modificationHandler.DataContext.GetTable<TAudit>(), x => beforeSetter.Compile()(x));
    // Update
    modificationHandler.Updatable.Update();
    // Insert Audit AFTER record
    var qryAfter = modificationHandler.TableWherePredicate.Insert(modificationHandler.DataContext.GetTable<TAudit>(), x => afterSetter.Compile()(x));
    return modificationHandler;
  }

}
