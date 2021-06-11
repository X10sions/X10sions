using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate_v5_2.Util {
  public static class ReflectionCache {
    // When adding a method to this cache, please follow the naming convention of those subclasses and fields:
    //  - Add your method to a subclass named according to the type holding the method, and suffixed with "Methods".
    //  - Name the field according to the method name.
    //  - If the method has overloads, suffix it with "With" followed by its parameter names. Do not list parameters
    //    common to all overloads.
    //  - If the method is a generic method definition, add "Definition" as final suffix.
    //  - If the method is generic, suffix it with "On" followed by its generic parameter type names.
    // Avoid caching here narrow cases, such as those using specific types and unlikely to be used by many classes.
    // Cache them instead in classes using them.

    public static class EnumerableMethods {
      public static readonly MethodInfo AggregateDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Aggregate<object>(null, null));
      public static readonly MethodInfo AggregateWithSeedDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Aggregate<object, object>(null, null, null));
      public static readonly MethodInfo AggregateWithSeedAndResultSelectorDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Aggregate<object, object, object>(null, null, null, null));
      public static readonly MethodInfo AllDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.All<object>(null, null));
      public static readonly MethodInfo CastDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Cast<object>(null));
      public static readonly MethodInfo GroupByWithElementSelectorDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.GroupBy<object, object, object>(null, null, default(Func<object, object>)));
      public static readonly MethodInfo MaxDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Max<object>(null));
      public static readonly MethodInfo MinDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Min<object>(null));
      public static readonly MethodInfo SelectDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.Select(null, default(Func<object, object>)));
      public static readonly MethodInfo SumOnInt = ReflectHelper.GetMethod(() => Enumerable.Sum(default(IEnumerable<int>)));
      public static readonly MethodInfo ToArrayDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.ToArray<object>(null));
      public static readonly MethodInfo ToListDefinition = ReflectHelper.GetMethodDefinition(() => Enumerable.ToList<object>(null));
    }

    public static class MethodBaseMethods {
      public static readonly MethodInfo GetMethodFromHandle = ReflectHelper.GetMethod(() => MethodBase.GetMethodFromHandle(default));
      public static readonly MethodInfo GetMethodFromHandleWithDeclaringType = ReflectHelper.GetMethod(() => MethodBase.GetMethodFromHandle(default, default));
    }

    public static class QueryableMethods {
      public static readonly MethodInfo SelectDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Select(null, default(Expression<Func<object, object>>)));
      public static readonly MethodInfo CountDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Count<object>(null));
      public static readonly MethodInfo CountWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Count<object>(null, null));
      public static readonly MethodInfo LongCountDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.LongCount<object>(null));
      public static readonly MethodInfo LongCountWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.LongCount<object>(null, null));
      public static readonly MethodInfo AnyDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Any<object>(null));
      public static readonly MethodInfo AnyWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Any<object>(null, null));
      public static readonly MethodInfo AllDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.All<object>(null, null));
      public static readonly MethodInfo FirstDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.First<object>(null));
      public static readonly MethodInfo FirstWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.First<object>(null, null));
      public static readonly MethodInfo FirstOrDefaultDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.FirstOrDefault<object>(null));
      public static readonly MethodInfo FirstOrDefaultWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.FirstOrDefault<object>(null, null));
      public static readonly MethodInfo SingleDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Single<object>(null));
      public static readonly MethodInfo SingleWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Single<object>(null, null));
      public static readonly MethodInfo SingleOrDefaultDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.SingleOrDefault<object>(null));
      public static readonly MethodInfo SingleOrDefaultWithPredicateDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.SingleOrDefault<object>(null, null));
      public static readonly MethodInfo MinDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Min<object>(null));
      public static readonly MethodInfo MinWithSelectorDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Min<object, object>(null, null));
      public static readonly MethodInfo MaxDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Max<object>(null));
      public static readonly MethodInfo MaxWithSelectorDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Max<object, object>(null, null));
      public static readonly MethodInfo SumOfInt = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<int>)));
      public static readonly MethodInfo SumOfNullableInt = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<int?>)));
      public static readonly MethodInfo SumOfLong = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<long>)));
      public static readonly MethodInfo SumOfNullableLong = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<long?>)));
      public static readonly MethodInfo SumOfFloat = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<float>)));
      public static readonly MethodInfo SumOfNullableFloat = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<float?>)));
      public static readonly MethodInfo SumOfDouble = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<double>)));
      public static readonly MethodInfo SumOfNullableDouble = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<double?>)));
      public static readonly MethodInfo SumOfDecimal = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<decimal>)));
      public static readonly MethodInfo SumOfNullableDecimal = ReflectHelper.GetMethod(() => Queryable.Sum(default(IQueryable<decimal?>)));
      public static readonly MethodInfo SumWithSelectorOfIntDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, int>>)));
      public static readonly MethodInfo SumWithSelectorOfNullableIntDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, int?>>)));
      public static readonly MethodInfo SumWithSelectorOfLongDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, long>>)));
      public static readonly MethodInfo SumWithSelectorOfNullableLongDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, long?>>)));
      public static readonly MethodInfo SumWithSelectorOfFloatDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, float>>)));
      public static readonly MethodInfo SumWithSelectorOfNullableFloatDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, float?>>)));
      public static readonly MethodInfo SumWithSelectorOfDoubleDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, double>>)));
      public static readonly MethodInfo SumWithSelectorOfNullableDoubleDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, double?>>)));
      public static readonly MethodInfo SumWithSelectorOfDecimalDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, decimal>>)));
      public static readonly MethodInfo SumWithSelectorOfNullableDecimalDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Sum(null, default(Expression<Func<object, decimal?>>)));
      public static readonly MethodInfo AverageOfInt = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<int>)));
      public static readonly MethodInfo AverageOfNullableInt = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<int?>)));
      public static readonly MethodInfo AverageOfLong = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<long>)));
      public static readonly MethodInfo AverageOfNullableLong = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<long?>)));
      public static readonly MethodInfo AverageOfFloat = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<float>)));
      public static readonly MethodInfo AverageOfNullableFloat = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<float?>)));
      public static readonly MethodInfo AverageOfDouble = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<double>)));
      public static readonly MethodInfo AverageOfNullableDouble = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<double?>)));
      public static readonly MethodInfo AverageOfDecimal = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<decimal>)));
      public static readonly MethodInfo AverageOfNullableDecimal = ReflectHelper.GetMethod(() => Queryable.Average(default(IQueryable<decimal?>)));
      public static readonly MethodInfo AverageWithSelectorOfIntDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, int>>)));
      public static readonly MethodInfo AverageWithSelectorOfNullableIntDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, int?>>)));
      public static readonly MethodInfo AverageWithSelectorOfLongDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, long>>)));
      public static readonly MethodInfo AverageWithSelectorOfNullableLongDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, long?>>)));
      public static readonly MethodInfo AverageWithSelectorOfFloatDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, float>>)));
      public static readonly MethodInfo AverageWithSelectorOfNullableFloatDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, float?>>)));
      public static readonly MethodInfo AverageWithSelectorOfDoubleDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, double>>)));
      public static readonly MethodInfo AverageWithSelectorOfNullableDoubleDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, double?>>)));
      public static readonly MethodInfo AverageWithSelectorOfDecimalDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, decimal>>)));
      public static readonly MethodInfo AverageWithSelectorOfNullableDecimalDefinition = ReflectHelper.GetMethodDefinition(() => Queryable.Average(null, default(Expression<Func<object, decimal?>>)));
    }

    public static class TypeMethods {
      public static readonly MethodInfo GetTypeFromHandle = ReflectHelper.GetMethod(() => Type.GetTypeFromHandle(default));
    }
  }
}