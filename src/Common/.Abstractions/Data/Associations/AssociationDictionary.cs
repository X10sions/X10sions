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
  string GetAssociationKey(string propertyName1, string propertyName2) => propertyName2.CompareTo(propertyName1) > 0 ? $"{propertyName1}:{propertyName2}" : $"{propertyName2}:{propertyName1}";
  string GetAssociationKey(Expression selector1, Expression selector2) => GetAssociationKey(GetPropertyName(selector1), GetPropertyName(selector2));
  string GetPropertyName(Expression selector) => selector.GetMemberInfo().GetReflectedTypeFullName();
  //string GetPropertyName(MemberInfo member1) => member1.GetReflectedTypeFullName();

  public IAssociation Get<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2) where T1 : class? where T2 : class? {
    var property1 = GetPropertyName(selector1);
    var property2 = GetPropertyName(selector2);
    var key = GetAssociationKey(property1, property2);
    return this[key];
  }

  public IAssociationJoin GetAssociationJoin<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2) where T1 : class? where T2 : class? {
    var property1 = GetPropertyName(selector1);
    var property2 = GetPropertyName(selector2);
    var key = GetAssociationKey(property1, property2);
    var ass = this[key];
    return property2.CompareTo(property1) > 0 ? ass.Join2 : ass.Join1;
  }

  public LambdaExpression GetPredicate<T1, T2>(Expression<Func<T1, T2?>> selector1, Expression<Func<T2, T1?>> selector2) where T1 : class? where T2 : class? {
    var property1 = GetPropertyName(selector1);
    var property2 = GetPropertyName(selector2);
    var key = GetAssociationKey(property1, property2);
    var ass = this[key];
    return (property2.CompareTo(property1) > 0 ? ass.Join2 : ass.Join1).Predicate;
  }

  //public Expression<Func<T1, T2, bool>> GetPredicate<T1, T2>() where T1 : class? where T2 : class? {
  //  var property1 = typeof(T1).GetReflectedTypeFullName();
  //  var property2 = typeof(T2).GetReflectedTypeFullName();


  //  var key = GetAssociationKey(selector1, selector2);


  //  return property2.CompareTo(property1) > 0 ? $"{property1}:{property2}" : $"{property2}:{property1}";



  //  var ass = this[key];

  //  Add(key, ass);
  //  return ass;
  //}

}
