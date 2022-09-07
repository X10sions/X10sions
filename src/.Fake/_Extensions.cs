using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using X10sions.Fake.Data.Models;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace X10sions.Fake {
  public static class Extensions {

    public static int? GetWholeYearsBetween(this DateTime? minDate, DateTime maxDate) => (minDate == null) ? null : maxDate.Year - minDate.Value.Year - ((maxDate.Month < minDate.Value.Month || (maxDate.Month == minDate.Value.Month && maxDate.Day < minDate.Value.Day)) ? 1 : 0);
    public static LambdaExpression GetWholeYearsBetween_LambdaExpr() => DelegateDecompiler.MethodBodyDecompiler.Decompile(typeof(Extensions).GetMethodInfo(nameof(Extensions.GetWholeYearsBetween)));
    public static Expression<Func<DateTime?, DateTime, int?>> GetWholeYearsBetween_Expr() => (Expression<Func<DateTime?, DateTime, int?>>)DelegateDecompiler.MethodBodyDecompiler.Decompile(typeof(Extensions).GetMethodInfo(nameof(Extensions.GetWholeYearsBetween)));
    public static Expression<Func<DateTime?, DateTime, int?>> GetWholeYearsBetween_Expr2() => (DateTime? minDate, DateTime maxDate) => (minDate == null) ? null : maxDate.Year - minDate.Value.Year - ((maxDate.Month < minDate.Value.Month || (maxDate.Month == minDate.Value.Month && maxDate.Day < minDate.Value.Day)) ? 1 : 0);
    public static Expression<Func<FakePerson, string>> FirstName_Expr() => (e) => e.PreferredFirstName ?? e.ActualFirstName;
    public static Expression<Func<FakePerson, string>> FullName_Expr2() => (e) => FirstName_Expr().Compile().Invoke(e) + e.LastName;





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
      ) where T : class? where TMany : class {
      fluentMappingBuilder.Entity<T>().Association(prop1, predicate, true);
      fluentMappingBuilder.Entity<TMany>().Association(prop2, predicate.SwapParameters10(), false);
      Console.WriteLine($"{nameof(AddAssociationNullableToNotNull)}: {typeof(T).Name} to {typeof(TMany).Name}");
      return fluentMappingBuilder;
    }


    /// <summary>NotNull to NotNull </summary>
    public static DataConnection AddAssociation00<T, TMany>(this DataConnection dataConnection
      , Expression<Func<T, TMany>> prop1
      , Expression<Func<TMany, IEnumerable<T>>> prop2
      , Expression<Func<T, TMany, bool>> predicate
      ) where T : class where TMany : class {
      dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociationNotNullToNotNull(prop1, prop2, predicate);
      return dataConnection;
    }

    ///// <summary>NotNull to NotNull </summary>
    //public static DataConnection AddAssociation00<T1, T2>(this DataConnection dataConnection
    //  , Expression<Func<T1, T2>> prop1
    //  , Expression<Func<T2, T1>> prop2
    //  , Expression<Func<T1, T2, bool>> predicate
    //  ) where T1 : class where T2 : class {
    //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociationNotNullToNotNull(prop1, prop2, predicate);
    //  return dataConnection;
    //}

    //public static DataConnection AddAssociation01<T1, T2>(this DataConnection dataConnection
    //  , Expression<Func<T1, T2>> prop1
    //  , Expression<Func<T2, T1>> prop2
    //  , Expression<Func<T1, T2, bool>> predicate
    //  ) where T1 : class where T2 : class? {
    //  dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociationNotNullToNullable(prop1, prop2, predicate);
    //  return dataConnection;
    //}

    /// <summary>NotNull to Nullable</summary>
    public static DataConnection AddAssociation01<T, TMany>(this DataConnection dataConnection
     , Expression<Func<T, TMany>> prop1
     , Expression<Func<TMany, IEnumerable<T>>> prop2
     , Expression<Func<T, TMany, bool>> predicate
     ) where T : class where TMany : class? {
      dataConnection.MappingSchema.GetFluentMappingBuilder().AddAssociationNotNullToNullable(prop1, prop2, predicate);
      return dataConnection;
    }

    public static MethodInfo? GetMethodInfo(this Type type, string name) => type.GetMethods().FirstOrDefault(x => x.Name == name);

    public static Expression<Func<T2, T1, TResult>> SwapParameters00<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class where T2 : class => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);
    public static Expression<Func<T2, T1, TResult>> SwapParameters01<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class where T2 : class? => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);
    public static Expression<Func<T2, T1, TResult>> SwapParameters10<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> exprFunc) where T1 : class? where T2 : class => Expression.Lambda<Func<T2, T1, TResult>>(exprFunc.Body, exprFunc.Parameters[1], exprFunc.Parameters[0]);

    public static IServiceCollection AddLinqToDBContext<TContext, TConnection>(this IServiceCollection services, IDataProvider dataProvider, string connectionString, ILogger logger)
      where TContext : IDataContext
      where TConnection : IDbConnection {
      logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
      services.AddLinqToDBContext<TContext>((provider, options) => {
        options.UseConnectionString(dataProvider, connectionString).UseDefaultLogging(provider);
      });
      return services;
    }

    //public static IServiceCollection AddLinqToDBContext<TContext, TConnection, TDataReader>(this IServiceCollection services, string connectionString, Func<LinqToDBConnectionOptions<TContext>, TContext> newContext, ILogger logger)
    //  where TContext : class, IDataContext
    //  where TConnection : DbConnection, new()
    //  where TDataReader : IDataReader {
    //  logger.LogInformation($"{nameof(AddLinqToDBContext)}<{typeof(TConnection)},{typeof(TContext)}>CS:{{connectionString}}");
    //  return services.AddScoped(x => GenericDataProvider<TConnection>.GetDataContext<TContext, TDataReader>(connectionString, newContext));
    //}

  }
}
