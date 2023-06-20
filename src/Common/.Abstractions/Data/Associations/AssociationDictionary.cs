using System.Linq.Expressions;
using System.Reflection;

namespace Common.Data.Assocations;

public class AssociationDictionary : Dictionary<string, IAssociation> {
  /// <summary>OneToOne</summary>
  public Association<T1, T2> Add<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2, Expression<Func<T1, T2, bool>> predicate) where T1 : class? where T2 : class? {
    var ass = new Association<T1, T2>(selector1, selector2, predicate);
    var key = GetAssociationKey(selector1, selector2);
    Add(key, ass);
    return ass;
  }

  /// <summary>ManyToOne</summary>
  public Association<TMany, T> Add<TMany, T>(Expression<Func<TMany, T?>> selector1, Expression<Func<T, IEnumerable<TMany>>> selector2, Expression<Func<TMany, T, bool>> predicate) where T : class? where TMany : class? {
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
