using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CommonOrm {
  public static class LinqHelper {

    public static IEnumerable<MemberInfo> GetArgumentMemberInfos<T>(this Expression<Func<T, object>> expression) {
      var body = expression.Body;
      switch (body.NodeType) {
        case ExpressionType.New: return (from a in ((body as NewExpression).Arguments) select (a as MemberExpression).Member);
        case ExpressionType.MemberAccess: return (new[] { (body as MemberExpression).Member });
        case ExpressionType.MemberInit: return (from b in ((body as MemberInitExpression).Bindings) select b.Member);
        default: throw new Exception($"{nameof(expression)} has unknown nodeType '{body.NodeType}' for {body}.");
      }
    }

    public static MemberInfo GetArgumentMemberInfo<T>(this Expression<Func<T, object>> expression, int argumentNumber) {
      var body = expression.Body;
      switch (body.NodeType) {
        case ExpressionType.New: return ((body as NewExpression).Arguments[argumentNumber] as MemberExpression).Member;
        case ExpressionType.MemberAccess: return (body as MemberExpression).Member;
        case ExpressionType.MemberInit: return (body as MemberInitExpression).Bindings[argumentNumber].Member;
        default: throw new Exception($"{nameof(expression)} has unknown nodeType '{body.NodeType}' for {body}.");
      }
    }

    public static IEnumerable<string> GetArgumentsMemberNames<T>(this Expression<Func<T, object>> expression) => from x in GetArgumentMemberInfos(expression) select x.Name;

    public static Expression<Func<T, object>> GetSelectExpression<T>(string keyName, string parameterExpressionName = "x") => GetSelectExpression<T>(new[] { keyName }, parameterExpressionName);

    public static Expression<Func<T, object>> GetSelectExpression<T>(IEnumerable<string> keyNames, string parameterExpressionName = "x") {
      var typeSource = typeof(T);
      var parameterExpression = Expression.Parameter(typeSource, parameterExpressionName);
      var memberBindings = new List<MemberBinding>();
      foreach (var keyName in keyNames) {
        var memberExpression = Expression.Property(parameterExpression, keyName);
        var sourceProperty = typeSource.GetProperty(memberExpression.Member.Name, BindingFlags.Public | BindingFlags.Instance);
        memberBindings.Add(Expression.Bind(sourceProperty, memberExpression));
      }
      var partExpression = Expression.New(typeSource);
      var memberInit = Expression.MemberInit(partExpression, memberBindings);
      return Expression.Lambda<Func<T, object>>(memberInit, parameterExpression);
    }


    //public static Expression<Func<TEntity, TCacheItem>> BuildSelector0<TEntity, TCacheItem>(TEntity entity) {
    //  List<MemberBinding> memberBindings = new List<MemberBinding>();
    //  foreach (var entityPropertyInfo in typeof(TEntity).GetProperties()) {
    //    foreach (var cachePropertyInfo in typeof(TCacheItem).GetProperties()) {
    //      if (entityPropertyInfo.PropertyType == cachePropertyInfo.PropertyType && entityPropertyInfo.Name == cachePropertyInfo.Name) {
    //        var fieldExpressoin = Expression.Field(Expression.Constant(entity), entityPropertyInfo.Name);
    //        memberBindings.Add(Expression.Bind(cachePropertyInfo, fieldExpressoin));
    //      }
    //    }
    //  }
    //  ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "x");
    //  NewExpression newExpression = Expression.New(typeof(TCacheItem));
    //  MemberInitExpression memberInit = Expression.MemberInit(newExpression, memberBindings);
    //  return Expression.Lambda<Func<TEntity, TCacheItem>>(memberInit, parameterExpression);
    //}

    //public static Expression<Func<TSource, TTarget>> BuildSelector1<TSource, TTarget>() {
    //  Type typeSource = typeof(TSource);
    //  Type typeDto = typeof(TTarget);
    //  ParameterExpression parameterExpression = Expression.Parameter(typeSource, "source");
    //  List<MemberBinding> bindings = new List<MemberBinding>();
    //  foreach (PropertyInfo sourceProperty in typeSource.GetProperties().Where(x => x.CanRead)) {
    //    PropertyInfo targetProperty = typeDto.GetProperty(sourceProperty.Name);
    //    if (targetProperty != null && targetProperty.CanWrite && targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)) {
    //      MemberExpression propertyExpression = Expression.Property(parameterExpression, sourceProperty);
    //      bindings.Add(Expression.Bind(targetProperty, propertyExpression));
    //    }
    //  }
    //  NewExpression newExpression = Expression.New(typeDto);
    //  MemberInitExpression memberInit = Expression.MemberInit(newExpression, bindings);
    //  return Expression.Lambda<Func<TSource, TTarget>>(memberInit, parameterExpression);
    //}

    //public static Expression<Func<TEntity, TCacheItem>> BuildSelector2<TEntity, TCacheItem>() {
    //  Type typeSource = typeof(TEntity);
    //  Type typeDto = typeof(TCacheItem);
    //  ParameterExpression parameterExpression = Expression.Parameter(typeSource, "p");
    //  PropertyInfo[] propertiesDto = typeDto.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    //  IEnumerable<MemberAssignment> memberAssignments = propertiesDto.Select(p => {
    //    PropertyInfo propertyInfo = typeSource.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
    //    MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);
    //    return Expression.Bind(p, memberExpression);
    //  });
    //  NewExpression newExpression = Expression.New(typeDto);
    //  MemberInitExpression memberInit = Expression.MemberInit(newExpression, memberAssignments);
    //  return Expression.Lambda<Func<TEntity, TCacheItem>>(memberInit, parameterExpression);
    //}

    //public Expression<Func<T, object>> CreateSelector<T>( string[] columns, string memberName= "x") {
    //  ParameterExpression aParamExpression = Expression.Parameter(typeof(T), memberName);
    //  for (int i = 0; i <= columns.Length - 1; ++i) {
    //    MemberExpression memberExpression = Expression.Property(aParamExpression, columns[i]);
    //  }
    //  List<ParameterExpression> parameterList = new List<ParameterExpression> { aParamExpression };
    //  ReadOnlyCollection<ParameterExpression> parameters = new ReadOnlyCollection<ParameterExpression>(parameterList);
    //  NewExpression newExpression = Expression.New(typeof(T)); // ConstructorInfo Needed
    //  return Expression.Lambda<Func<T, object>>(newExpression, parameters);
    //}

  }
}