using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Mapping {
  public static class FluentMappingBuilderExtensions {

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

  }
}
