using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Mapping {
  public static class FluentMappingBuilderExtensions {

    //public static FluentMappingBuilder AddAssociation<T1, T2, T1Key, T2Key>(this FluentMappingBuilder fluentMappingBuilder
    //  , Expression<Func<T1, T2>> prop1
    //  , Expression<Func<T2, T1>> prop2
    //  , Expression<Func<T1, T1Key>> key1
    //  , Expression<Func<T2, T2Key>> key2
    //  ) {
    //  var prop2CanBeNull = typeof(T1).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
    //  var prop1CanBeNull = typeof(T2).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
    //  fluentMappingBuilder.Entity<T1>().Association(prop1, key1, key2, prop1CanBeNull);
    //  fluentMappingBuilder.Entity<T2>().Association(prop2, key2, key1, prop2CanBeNull);
    //  return fluentMappingBuilder;
    //}

    //public static FluentMappingBuilder AddAssociation<TOne, TMany, TOneKey, TManyKey>(this FluentMappingBuilder fluentMappingBuilder
    //  , Expression<Func<TOne, TMany>> prop1
    //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
    //  , Expression<Func<TOne, TOneKey>> key1
    //  , Expression<Func<TMany, TManyKey>> key2
    //  ) {
    //  var prop2CanBeNull = typeof(TOne).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
    //  var prop1CanBeNull = typeof(TMany).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
    //  fluentMappingBuilder.Entity<TOne>().Association(prop1, key1, key2, prop1CanBeNull);
    //  fluentMappingBuilder.Entity<TMany>().Association(prop2, key2, key1, prop2CanBeNull);
    //  return fluentMappingBuilder;
    //}

    //public static FluentMappingBuilder AddAssociation<TOne, TMany>(this FluentMappingBuilder fluentMappingBuilder
    //  , Expression<Func<TOne, TMany>> prop1
    //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
    //  , Expression<Func<TOne, TMany, bool>> predicate
    //  ) {
    //  var prop2CanBeNull = typeof(TOne).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
    //  var prop1CanBeNull = typeof(TMany).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
    //  Console.WriteLine($"{nameof(AddAssociation)}:  {typeof(TOne).Name}:{prop1CanBeNull}  {typeof(TMany).Name}:{prop2CanBeNull}");
    //  fluentMappingBuilder.Entity<TOne>().Association(prop1, predicate, prop1CanBeNull);
    //  fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters1(), prop2CanBeNull);
    //  return fluentMappingBuilder;
    //}

    //public static FluentMappingBuilder AddAssociation<TOne1, TOne2>(this FluentMappingBuilder fluentMappingBuilder
    //  , Expression<Func<TOne1, TOne2>> prop1
    //  , Expression<Func<TOne2, TOne1>> prop2
    //  , Expression<Func<TOne1, TOne2, bool>> predicate
    //  ) {
    //  var prop2CanBeNull = typeof(TOne1).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
    //  var prop1CanBeNull = typeof(TOne2).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
    //  fluentMappingBuilder.Entity<TOne1>().Association(prop1, predicate, prop1CanBeNull);
    //  fluentMappingBuilder.Entity<TOne2>().Association(prop2, predicate.SwapParameters1(), prop2CanBeNull);
    //  return fluentMappingBuilder;
    //}


    /// <summary>NotNull to NotNull </summary>
    public static FluentMappingBuilder AddAssociationNotNullToNotNull<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class where TMany : class {
      fluentMappingBuilder.Entity<T>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters00(), false);
      Console.WriteLine($"{nameof(AddAssociationNotNullToNotNull)}: {typeof(T).Name} to {typeof(TMany).Name}");
      return fluentMappingBuilder;
    }

    /// <summary>NotNull to NotNull </summary>
    public static FluentMappingBuilder AddAssociationNotNullToNotNull<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class where T2 : class {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters00(), false);
      Console.WriteLine($"{nameof(AddAssociationNotNullToNotNull)}: {typeof(T1).Name} to {typeof(T2).Name}");
      return fluentMappingBuilder;
    }

    /// <summary>NotNull to Nullable </summary>
    public static FluentMappingBuilder AddAssociationNotNullToNullable<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
     , Expression<Func<T1, T2>> prop1
     , Expression<Func<T2, T1>> prop2
     , Expression<Func<T1, T2, bool>> predicate
     ) where T1 : class where T2 : class? {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters01(), true);
      Console.WriteLine($"{nameof(AddAssociationNotNullToNullable)}: {typeof(T1).Name} to {typeof(T2).Name}");
      return fluentMappingBuilder;
    }

    /// <summary>NotNull to Nullable </summary>
    public static FluentMappingBuilder AddAssociationNotNullToNullable<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class where TMany : class? {
      fluentMappingBuilder.Entity<T>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters01(), true);
      Console.WriteLine($"{nameof(AddAssociationNotNullToNullable)}: {typeof(T).Name} to {typeof(TMany).Name}");
      return fluentMappingBuilder;
    }

    /// <summary>Nullable to NotNull </summary>
    public static FluentMappingBuilder AddAssociationNullableToNotNull<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class? where T2 : class {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, true);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters10(), false);
      Console.WriteLine($"{nameof(AddAssociationNullableToNotNull)}: {typeof(T1).Name} to {typeof(T2).Name}");
      return fluentMappingBuilder;
    }

    /// <summary>Nullable to NotNull </summary>
    public static FluentMappingBuilder AddAssociationNullableToNotNull<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T  : class? where TMany : class {
      fluentMappingBuilder.Entity<T>().Association(prop1, predicate, true);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters10(), false);
      Console.WriteLine($"{nameof(AddAssociationNullableToNotNull)}: {typeof(T).Name} to {typeof(TMany).Name}");
      return fluentMappingBuilder;
    }

  }
}