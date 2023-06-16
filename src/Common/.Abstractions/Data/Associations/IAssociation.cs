using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Data.Assocations;

//public interface IAssocation<T1, T2> {
//  public Expression<Func<T1, T2>> T1Property { get; }
//  public Expression<Func<T1, T2, bool>> Predicate { get; }
//}

public interface IAssociation {
  LambdaExpression Selector1 { get; }
  LambdaExpression Predicate1 { get; }
  LambdaExpression Selector2 { get; }
  LambdaExpression Predicate2 { get; }
}

public static class IAssociationExtensions {
  public static bool Selector1IsMany(this IAssociation association) => association.Selector1.IsIEnumerable();
  public static bool Selector2IsMany(this IAssociation association) => association.Selector2.IsIEnumerable();
  public static bool Selector1CanBeNull(this IAssociation association) => association.Selector1.IsPropertyNullable();
  public static bool Selector2CanBeNull(this IAssociation association) => association.Selector2.IsPropertyNullable();
  //static bool IsIEnumerable(this Expression expression) => typeof(IEnumerable).IsAssignableFrom((expression as LambdaExpression)?.Body.Type);
  static bool IsIEnumerable(this LambdaExpression expression) => typeof(IEnumerable).IsAssignableFrom(expression.Body.Type);
}

public class AssociationDictionary : Dictionary<string, IAssociation> {
  //AssociationDictionary() { }

  /// <summary>OneToOne</summary>
  public Association<T1, T2> Add<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? {
    var ass = new Association<T1, T2>(selector1, selector2, predicate);
    var key = GetAssociationKey(selector1, selector2);
    Add(key, ass);
    return ass;
  }

  /// <summary>ManyToOne</summary>
  public Association<TMany, T> Add<TMany, T>(Expression<Func<TMany, T?>> selector1, Expression<Func<T, IEnumerable<TMany>>> selector2, Expression<Func<TMany, T, bool>> predicate) where T : class where TMany : class? {
    var ass = new Association<TMany, T>(selector1, selector2, predicate);
    var key = GetAssociationKey(selector1, selector2);
    Add(key, ass);
    return ass;
  }

  string GetAssociationKey(Expression selector1, Expression selector2) {
    var property1 = selector1.GetMemberInfo().GetReflectedTypeFullName();
    var property2 = selector2.GetMemberInfo().GetReflectedTypeFullName();
    return property2.CompareTo(property1) > 0 ? $"{property1}:{property2}" : $"{property2}:{property1}";
  }
}

public class Association<T1, T2> : IAssociation where T1 : class? where T2 : class? {
  Association(LambdaExpression selector1, LambdaExpression selector2, Expression<Func<T1, T2, bool>> predicate) {
    Selector1 = selector1;
    Selector2 = selector2;
    Predicate1 = predicate;
    Predicate2 = Expression.Lambda<Func<T2, T1, bool>>(predicate.Body, predicate.Parameters[1], predicate.Parameters[0]);
  }

  /// <summary>OneToOne</summary>
  public Association(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2, Expression<Func<T1, T2, bool>> predicate)
    : this((LambdaExpression)selector1, selector2, predicate) { }

  /// <summary>ManyToOne</summary>
  public Association(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, IEnumerable<T1>>> selector2, Expression<Func<T1, T2, bool>> predicate)
    : this((LambdaExpression)selector1, selector2, predicate) { }

  public LambdaExpression Selector1 { get; }
  public LambdaExpression Predicate1 { get; }
  public LambdaExpression Selector2 { get; }
  public LambdaExpression Predicate2 { get; }

}
