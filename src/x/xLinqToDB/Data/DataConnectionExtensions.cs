using LinqToDB.Mapping;
using System.Linq.Expressions;

namespace LinqToDB.Data {
  public static class DataConnectionExtensions {

    //public static DataConnection AddAssociation<T1, T2, T1Key, T2Key>(this DataConnection dataConnection
    //  , Expression<Func<T1, T2>> prop1
    //  , Expression<Func<T2, T1>> prop2
    //  , Expression<Func<T1, T1Key>> key1
    //  , Expression<Func<T2, T2Key>> key2
    //  ) {
    //  dataConnection.GetFluentMappingBuilder().AddAssociation(prop1, prop2, key1, key2);
    //  return dataConnection;
    //}


    //public static DataConnection AddAssociation<TOne, TMany, TOneKey, TManyKey>(this DataConnection dataConnection
    //  , Expression<Func<TOne, TMany>> prop1
    //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
    //  , Expression<Func<TOne, TOneKey>> key1
    //  , Expression<Func<TMany, TManyKey>> key2
    //  ) {
    //  dataConnection.GetFluentMappingBuilder().AddAssociation(prop1, prop2, key1, key2);
    //  return dataConnection;
    //}

    //public static DataConnection AddAssociation<TOne, TMany>(this DataConnection dataConnection
    //  , Expression<Func<TOne, TMany>> prop1
    //  , Expression<Func<TMany, IEnumerable<TOne>>> prop2
    //  , Expression<Func<TOne, TMany, bool>> predicate
    //  ) {
    //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociation(prop1, prop2, predicate);
    //  return dataConnection;
    //}

    //public static DataConnection AddAssociation<TOne1, TOne2>(this DataConnection dataConnection
    //  , Expression<Func<TOne1, TOne2>> prop1
    //  , Expression<Func<TOne2, TOne1>> prop2
    //  , Expression<Func<TOne1, TOne2, bool>> predicate
    //  ) {
    //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociation(prop1, prop2, predicate);
    //  return dataConnection;
    //}

    /// <summary>NotNull to NotNull </summary>
    public static FluentMappingBuilder AddAssociation00<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class where TMany : class => fluentMappingBuilder.AddAssociationNotNullToNotNull(prop1, prop2, predicate);

    /// <summary>NotNull to NotNull </summary>
    public static FluentMappingBuilder AddAssociation00<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class where T2 : class => fluentMappingBuilder.AddAssociationNotNullToNotNull(prop1, prop2, predicate);

    /// <summary>NotNull to Nullable</summary>
    public static FluentMappingBuilder AddAssociation01<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
     , Expression<Func<T1, T2>> prop1
     , Expression<Func<T2, T1>> prop2
     , Expression<Func<T1, T2, bool>> predicate
     ) where T1 : class where T2 : class? => fluentMappingBuilder.AddAssociationNotNullToNullable(prop1, prop2, predicate);

    /// <summary>NotNull to Nullable</summary>
    public static FluentMappingBuilder AddAssociation01<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
     , Expression<Func<T, TMany>> prop1
     , Expression<Func<TMany, IEnumerable<T>>> prop2
     , Expression<Func<T, TMany, bool>> predicate
     ) where T : class where TMany : class? => fluentMappingBuilder.AddAssociationNotNullToNullable(prop1, prop2, predicate);

    /// <summary>NUllable to NotNull </summary>
    public static FluentMappingBuilder AddAssociation10<T1, T2>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T1, T2>> prop1
      , Expression<Func<T2, T1>> prop2
      , Expression<Func<T1, T2, bool>> predicate
      ) where T1 : class? where T2 : class => fluentMappingBuilder.AddAssociationNullableToNotNull(prop1, prop2, predicate);

    /// <summary>NUllable to NotNull </summary>
    public static FluentMappingBuilder AddAssociation10<T, TMany>(this FluentMappingBuilder fluentMappingBuilder
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class? where TMany : class => fluentMappingBuilder.AddAssociationNullableToNotNull(prop1, prop2, predicate);

    public static FluentMappingBuilder GetFluentMappingBuilder(this DataConnection dc) => new FluentMappingBuilder(dc.MappingSchema);

    public static ITable<T> GetTableWithPrimaryKey<T>(this DataConnection dataConnection, Expression<Func<T, object>> primaryKey, bool isPrimaryKeyIdentity)
      where T : class {
      var fmb = new FluentMappingBuilder(dataConnection.MappingSchema).Entity<T>().HasPrimaryKey(primaryKey);
      if (isPrimaryKeyIdentity) {
        fmb.HasIdentity(primaryKey);
      }
      fmb.Build();
      return dataConnection.GetTable<T>();
    }

  }
}