using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Extensions;
using LinqToDB.Mapping;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace xLinqToDB.DataProvider.DB2iSeries.V3_2_3.TB.xMapping {
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
  public class DB2iSeriesColumnAttribute_TB : ColumnAttribute {
    public int LengthAllocate { get; set; }
  }

  public class xAssociationDescriptor_TB {

    public MemberInfo MemberInfo { get; set; }
    public string[] ThisKey { get; set; }
    public string[] OtherKey { get; set; }
    public string ExpressionPredicate { get; set; }
    public string Storage { get; set; }
    public bool CanBeNull { get; set; }
    public xAssociationDescriptor_TB(Type type, MemberInfo memberInfo__1, string[] thisKey__2, string[] otherKey__3, string expressionPredicate__4, string storage__5, bool canBeNull__6) {
      if ((object)memberInfo__1 == null) {
        throw new ArgumentNullException("MemberInfo");
      }
      if (thisKey__2 == null) {
        throw new ArgumentNullException("ThisKey");
      }
      if (otherKey__3 == null) {
        throw new ArgumentNullException("OtherKey");
      }
      if (thisKey__2.Length == 0 && expressionPredicate__4.IsNullOrEmpty()) {
        throw new ArgumentOutOfRangeException("ThisKey", $"Association '{type.Name}.{memberInfo__1.Name}' does not define keys.");
      }
      if (thisKey__2.Length != otherKey__3.Length) {
        throw new ArgumentException($"Association '{type.Name}.{memberInfo__1.Name}' has different number of keys for parent and child objects.");
      }
      MemberInfo = memberInfo__1;
      ThisKey = thisKey__2;
      OtherKey = otherKey__3;
      ExpressionPredicate = expressionPredicate__4;
      Storage = storage__5;
      CanBeNull = canBeNull__6;
    }

    public static string[] ParseKeys(string keys) {
      if (keys != null) {
        return keys.Replace(" ", "").Split(',');
      }
      return Array<string>.Empty;
    }

    public LambdaExpression GetPredicate() {
      if (string.IsNullOrEmpty(ExpressionPredicate)) {
        return null;
      }
      var type = MemberInfo.DeclaringType;
      if ((object)type == null) {
        throw new ArgumentException($"Member '{MemberInfo.Name}' has no declaring type");
      }
      var members = ReflectionExtensions.GetStaticMembersEx(type, ExpressionPredicate);
      if (members.Length == 0) {
        throw new LinqToDBException($"Static member '{ExpressionPredicate}' for type '{type.Name}' not found");
      }
      if (members.Length > 1) {
        throw new LinqToDBException($"Ambiguous members '{ExpressionPredicate}' for type '{type.Name}' has been found");
      }
      Expression predicate = null;
      var propInfo = members[0] as PropertyInfo;
      if ((object)propInfo != null) {
        var value2 = (propInfo.GetValue(null, null));
        if (value2 == null) {
          return null;
        }
        predicate = (value2 as Expression);
        if (predicate == null) {
          throw new LinqToDBException($"Property '{ExpressionPredicate}' for type '{type.Name}' should return expression");
        }
      } else {
        var method = members[0] as MethodInfo;
        if ((object)method != null) {
          if (method.GetParameters().Length > 0) {
            throw new LinqToDBException($"Method '{ExpressionPredicate}' for type '{type.Name}' should have no parameters");
          }
          var value = (method.Invoke(null, Array<object>.Empty));
          if (value == null) {
            return null;
          }
          predicate = (value as Expression);
          if (predicate == null) {
            throw new LinqToDBException($"Method '{ExpressionPredicate}' for type '{type.Name}' should return expression");
          }
        }
      }
      if (predicate == null) {
        throw new LinqToDBException($"Member '{ExpressionPredicate}' for type '{type.Name}' should be static property or method");
      }
      var lambda = predicate as LambdaExpression;
      if (lambda == null || lambda.Parameters.Count != 2) {
        throw new LinqToDBException("Invalid predicate expression");
      }
      return lambda;
    }
  }
}
