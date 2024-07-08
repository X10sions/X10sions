using Common.AspNetCore.Identity;
using LinqToDB.Mapping;
using System.Linq.Expressions;

namespace LinqToDB;
public static class IDataContextExtensions {

  public static T GetById<T>(this IDataContext context, params object[] keyValues) where T : class {
    var keys = (from p in typeof(T).GetProperties()
                where p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() > 0
                select p).ToList();
    if (keyValues.Count() != keyValues.Count())
      throw new ArgumentException("Amount of KeyValues does not match the amount Keys for this entity.");
    IQueryable<T> result = context.GetTable<T>();
    var i = 0;
    foreach (var keyValue in keyValues) {
      var funcPredicate = SimpleComparison<T>(keys[i].Name, keyValue);
      Expression<Func<T, bool>> exprPredicate = p => funcPredicate(p);
      result = result.Where(exprPredicate);
      i += 1;
    }
    return result.FirstOrDefault();
  }

  public static async Task<T> GetByIdAsync<T>(this IDataContext context, params object[] keyValues) where T : class
    => await Task.FromResult(context.GetById<T>(keyValues));

  public static void SetValue(this MappingSchema ms, object o, object val, ColumnDescriptor column) {
    var ex = ms.GetConvertExpression(val.GetType(), column.MemberType);
    column.MemberAccessor.SetValue(o, ex.Compile().DynamicInvoke(val));
  }

  private static Func<T, bool> SimpleComparison<T>(string property, object value) where T : class {
    var type = typeof(T);
    var pe = Expression.Parameter(type, "p");
    var propertyReference = Expression.Property(pe, property);
    var constantReference = Expression.Constant(value);
    return Expression.Lambda<Func<T, bool>>(Expression.Equal(propertyReference, constantReference), new[] { pe }).Compile();
  }

  private static Func<T, bool> SimpleComparison<T, D>(string propertyName, D value) where T : class {
    var type = typeof(T);
    var pe = Expression.Parameter(type, "p");
    var constantReference = Expression.Constant(value);
    var propertyReference = Expression.Property(pe, propertyName);
    return Expression.Lambda<Func<T, bool>>(Expression.Equal(propertyReference, constantReference), new[] { pe }).Compile();
  }

  public static T TryInsertAndSetIdentity<T>(this IDataContext db, T obj) where T : class {
    var ms = db.MappingSchema;
    var od = ms.GetEntityDescriptor(obj.GetType());
    var identity = od.Columns.FirstOrDefault(_ => _.IsIdentity);
    if (identity != null) {
      var res = db.InsertWithIdentity(obj);
      ms.SetValue(obj, res, identity);
    } else {
      db.Insert(obj);
    }
    return obj;
  }

  public static int UpdateConcurrent<T, TKey>(this IDataContext dc, T obj)
   where T : class, IConcurrency<TKey>
   where TKey : IEquatable<TKey> {
    var stamp = Guid.NewGuid().ToString();
    var query = dc.GetTable<T>()
        .Where(_ => _.Id.Equals(obj.Id) && _.ConcurrencyStamp == obj.ConcurrencyStamp)
        .Set(_ => _.ConcurrencyStamp, stamp);
    var ed = dc.MappingSchema.GetEntityDescriptor(typeof(T));
    var p = Expression.Parameter(typeof(T));
    foreach (var column in ed.Columns.Where(_ => _.MemberName != nameof(IConcurrency<TKey>.ConcurrencyStamp) && !_.IsPrimaryKey && !_.SkipOnUpdate)) {
      var expr = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.PropertyOrField(p, column.MemberName), typeof(object)), p);
      var val = column.MemberAccessor.Getter(obj);
      query = query.Set(expr, val);
    }
    var res = query.Update();
    obj.ConcurrencyStamp = stamp;
    return res;
  }

}
