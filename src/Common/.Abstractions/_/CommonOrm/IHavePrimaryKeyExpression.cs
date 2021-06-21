//using LinqToDB;
//using LinqToDB.Data;
//using LinqToDB.Mapping;
using System;
using System.Linq.Expressions;

namespace CommonOrm {
  public interface IHavePrimaryKeyExpression<T> {
    Expression<Func<T, object>> PrimaryKeyExpression { get; }
  }

  public static class IHavePrimaryKeyExpressionExtensions {

    //// LINQ2DB:

    //public static ITable<T> GetTableWithPrimaryKey<T>(this DataConnection dc)
    //  where T : class, IHavePrimaryKeyExpression<T> {
    //  T t = default;
    //  dc.MappingSchema.GetFluentMappingBuilder().Entity<T>().HasPrimaryKey(t.PrimaryKeyExpression);
    //  return dc.GetTable<T>();
    //}

  }
}