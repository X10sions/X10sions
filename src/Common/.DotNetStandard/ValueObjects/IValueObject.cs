using System.Linq.Expressions;

namespace Common.ValueObjects;

public interface IValueObject<T> {
  T Value { get; }

  public static class Expressions {

    //public static IQueryable<T> Contains<T, TValue>(this IQueryable<T> qry, params TValue[] values)
    //  where T : IValueObject<T>
    //  => qry.Where(x => values.Select(x => x.Value).Contains(x.CONO15));

    public static Expression<Func<T, bool>> Contains<TValue, TValueObject>(Func<T, TValue> getValue, params TValueObject[] values)
      where TValueObject : IValueObject<TValue>
      => x => values.Select(x => x.Value).Contains(getValue(x));


    //public static Expression<Func<T, bool>> Contains<T,T2, TValue>(Expression<Func<T, IValueObject<T>>> getValue, params IValueObject<T>[] values)
    //  where T: class
    //     => x => values.Select(x => x.Value).Contains(getValue.Compile()(x).Value);


    //  public static Expression<Func<T, bool>> Contains<T>(Expression<Func<T, IValueObject<T>>> getValue, params IValueObject<T>[] values)
    //    => x => values.Select(x => x.Value).Contains(getValue.Compile()(x).Value);

    //  public static Expression<Func<T, bool>> Contains<T, TValue>(Expression<Func<T, TValue>> getValue, params IValueObject<TValue>[] values)
    //    => x => values.Select(x => x.Value).Contains(getValue.Compile()(x));
  }
}

public static class IValueObjectExtensions {

  public static IQueryable<T> ContainsValueObject<T, TValue, TValueObject>(this IQueryable<T> qry, Func<T, TValue> getValue, params TValueObject[] values)
    where TValueObject : IValueObject<TValue>
    => qry.Where(IValueObject<T>.Expressions.Contains(getValue, values));

  public static string ToString<T>(this IValueObject<T> valueObject) => valueObject.ToString() ?? string.Empty;
}