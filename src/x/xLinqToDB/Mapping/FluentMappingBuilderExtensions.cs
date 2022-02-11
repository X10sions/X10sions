using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Mapping {
  public static class FluentMappingBuilderExtensions {

    public static FluentMappingBuilder AddAssociation<T1, T2, T1Key, T2Key>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T1Key>> key1
      , Expression<Func<T2, T2Key>> key2
      ) {
      var prop1CanBeNull = typeof(T1).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
      var prop2CanBeNull = typeof(T2).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
      fluentMappingBuilder.Entity<T1>().Association(prop1, key1, key2, prop1CanBeNull);
      fluentMappingBuilder.Entity<T2>().Association(prop2, key2, key1, prop2CanBeNull);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation<TOne, TMany, TOneKey, TManyKey>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<TOne, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<TOne>>> prop2
      , Expression<Func<TOne, TOneKey>> key1
      , Expression<Func<TMany, TManyKey>> key2
      ) {
      var prop1CanBeNull = typeof(TOne).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
      var prop2CanBeNull = typeof(TMany).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
      fluentMappingBuilder.Entity<TOne>().Association(prop1, key1, key2, prop1CanBeNull);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, key2, key1, prop2CanBeNull);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation<TOne, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<TOne, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<TOne>>> prop2
      , Expression<Func<TOne, TMany, bool>> predicate
      ) {
      var prop1CanBeNull = typeof(TOne).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
      var prop2CanBeNull = typeof(TMany).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
      fluentMappingBuilder.Entity<TOne>().Association(prop1, predicate, prop1CanBeNull);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters1(), prop2CanBeNull);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation<TOne1, TOne2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<TOne1, TOne2>> prop1
      , Expression<Func<TOne2, TOne1>> prop2
      , Expression<Func<TOne1, TOne2, bool>> predicate
      ) {
      var prop1CanBeNull = typeof(TOne1).GetProperty(prop1.GetMemberInfo().Name).IsNullable();
      var prop2CanBeNull = typeof(TOne2).GetProperty(prop2.GetMemberInfo().Name).IsNullable();
      fluentMappingBuilder.Entity<TOne1>().Association(prop1, predicate, prop1CanBeNull);
      fluentMappingBuilder.Entity<TOne2>().Association(prop2, predicate.SwapParameters1(), prop2CanBeNull);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation00<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class where TMany : class {
      fluentMappingBuilder.Entity<T>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters00(), false);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation00<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class where T2 : class {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters00(), false);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation01<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
     , Expression<Func<T1, T2>> prop1
     , Expression<Func<T2, T1>> prop2
     , Expression<Func<T1, T2, bool>> predicate
     ) where T1 : class where T2 : class? {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters01(), true);
      return fluentMappingBuilder;
    }

    public static FluentMappingBuilder AddAssociation10<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class? where T2 : class {
      fluentMappingBuilder.Entity<T1>().Association(prop1, predicate, true);
      fluentMappingBuilder.Entity<T2>().Association(prop2, predicate.SwapParameters10(), false);
      return fluentMappingBuilder;
    }
    public static FluentMappingBuilder AddAssociationXX<TOne, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<TOne, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<TOne>>> prop2
      , Expression<Func<TOne, TMany, bool>> predicate
      ) {
      var t1 = typeof(TOne);
      var t2 = typeof(TMany);
      var p1name = (prop1.Body as MemberExpression).Member.Name;
      var p2name = (prop2.Body as MemberExpression).Member.Name;
      var nz1 = (t1.GetProperty(p1name).IsNullable());
      var nz2 = (t2.GetProperty(p2name).IsNullable());
      //throw new Exception($"xxxx {p1name}={nz1};{p2name}={nz2};");
      fluentMappingBuilder.Entity<TOne>().Association(prop1, predicate, false);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters1(), false);
      return fluentMappingBuilder;
    }

  }
}