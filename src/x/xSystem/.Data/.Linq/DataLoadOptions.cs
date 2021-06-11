using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Linq {
  public sealed class DataLoadOptions {
    private static class Searcher {
      private class Visitor : SqlClient.ExpressionVisitor {
        internal MemberInfo MemberInfo;

        internal override Expression VisitMemberAccess(MemberExpression m) {
          MemberInfo = m.Member;
          return base.VisitMemberAccess(m);
        }

        internal override Expression VisitMethodCall(MethodCallExpression m) {
          Visit(m.Object);
          using(var enumerator = m.Arguments.GetEnumerator()) {
            if(enumerator.MoveNext()) {
              var current = enumerator.Current;
              Visit(current);
              return m;
            }
            return m;
          }
        }
      }

      internal static MemberInfo MemberInfoOf(LambdaExpression lambda) {
        var visitor = new Visitor();
        visitor.VisitLambda(lambda);
        return visitor.MemberInfo;
      }
    }

    private class SubqueryValidator : System.Data.Linq.SqlClient.ExpressionVisitor {
      private bool isTopLevel = true;

      internal override Expression VisitMethodCall(MethodCallExpression m) {
        var flag = isTopLevel;
        try {
          if(isTopLevel && !SubqueryRules.IsSupportedTopLevelMethod(m.Method)) {
            throw System.Data.Linq.Error.SubqueryDoesNotSupportOperator(m.Method.Name);
          }
          isTopLevel = false;
          return base.VisitMethodCall(m);
        } finally {
          isTopLevel = flag;
        }
      }
    }

    private bool frozen;

    private Dictionary<MetaPosition, MemberInfo> includes = new Dictionary<MetaPosition, MemberInfo>();

    private Dictionary<MetaPosition, LambdaExpression> subqueries = new Dictionary<MetaPosition, LambdaExpression>();

    internal bool IsEmpty {
      get {
        if(includes.Count == 0) {
          return subqueries.Count == 0;
        }
        return false;
      }
    }

    public void LoadWith<T>(Expression<Func<T, object>> expression) {
      if(expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      var loadWithMemberInfo = GetLoadWithMemberInfo(expression);
      Preload(loadWithMemberInfo);
    }

    public void LoadWith(LambdaExpression expression) {
      if(expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      var loadWithMemberInfo = GetLoadWithMemberInfo(expression);
      Preload(loadWithMemberInfo);
    }

    public void AssociateWith<T>(Expression<Func<T, object>> expression) {
      if(expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      AssociateWithInternal(expression);
    }

    public void AssociateWith(LambdaExpression expression) {
      if(expression == null) {
        throw System.Data.Linq.Error.ArgumentNull("expression");
      }
      AssociateWithInternal(expression);
    }

    private void AssociateWithInternal(LambdaExpression expression) {
      var expression2 = expression.Body;
      while(expression2.NodeType == ExpressionType.Convert || expression2.NodeType == ExpressionType.ConvertChecked) {
        expression2 = ((UnaryExpression)expression2).Operand;
      }
      var lambdaExpression = Expression.Lambda(expression2, expression.Parameters.ToArray());
      var association = Searcher.MemberInfoOf(lambdaExpression);
      Subquery(association, lambdaExpression);
    }

    internal bool IsPreloaded(MemberInfo member) {
      if(member == null) {
        throw System.Data.Linq.Error.ArgumentNull("member");
      }
      return includes.ContainsKey(new MetaPosition(member));
    }

    internal static bool ShapesAreEquivalent(DataLoadOptions ds1, DataLoadOptions ds2) {
      if(ds1 != ds2 && ((ds1 != null && !ds1.IsEmpty) || !(ds2?.IsEmpty ?? true))) {
        if(ds1 == null || ds2 == null || ds1.includes.Count != ds2.includes.Count) {
          return false;
        }
        foreach(var key in ds2.includes.Keys) {
          if(!ds1.includes.ContainsKey(key)) {
            return false;
          }
        }
      }
      return true;
    }

    internal LambdaExpression GetAssociationSubquery(MemberInfo member) {
      if(member == null) {
        throw System.Data.Linq.Error.ArgumentNull("member");
      }
      LambdaExpression value = null;
      subqueries.TryGetValue(new MetaPosition(member), out value);
      return value;
    }

    internal void Freeze() => frozen = true;

    internal void Preload(MemberInfo association) {
      if(association == null) {
        throw System.Data.Linq.Error.ArgumentNull("association");
      }
      if(frozen) {
        throw System.Data.Linq.Error.IncludeNotAllowedAfterFreeze();
      }
      includes.Add(new MetaPosition(association), association);
      ValidateTypeGraphAcyclic();
    }

    private void Subquery(MemberInfo association, LambdaExpression subquery) {
      if(frozen) {
        throw System.Data.Linq.Error.SubqueryNotAllowedAfterFreeze();
      }
      subquery = (LambdaExpression)Funcletizer.Funcletize(subquery);
      ValidateSubqueryMember(association);
      ValidateSubqueryExpression(subquery);
      subqueries[new MetaPosition(association)] = subquery;
    }

    private static MemberInfo GetLoadWithMemberInfo(LambdaExpression lambda) {
      var expression = lambda.Body;
      if(expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)) {
        expression = ((UnaryExpression)expression).Operand;
      }
      var memberExpression = expression as MemberExpression;
      if(memberExpression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter) {
        return memberExpression.Member;
      }
      throw System.Data.Linq.Error.InvalidLoadOptionsLoadMemberSpecification();
    }

    private void ValidateTypeGraphAcyclic() {
      IEnumerable<MemberInfo> enumerable = includes.Values;
      var num = 0;
      for(var i = 0; i < includes.Count; i++) {
        var hashSet = new HashSet<Type>();
        foreach(var item in enumerable) {
          hashSet.Add(GetIncludeTarget(item));
        }
        var list = new List<MemberInfo>();
        var flag = false;
        foreach(var item2 in enumerable) {
          if(hashSet.Where(delegate (Type et) {
            if(!et.IsAssignableFrom(item2.DeclaringType)) {
              return item2.DeclaringType.IsAssignableFrom(et);
            }
            return true;
          }).Any()) {
            list.Add(item2);
          } else {
            num++;
            flag = true;
            if(num == includes.Count) {
              return;
            }
          }
        }
        if(!flag) {
          throw System.Data.Linq.Error.IncludeCycleNotAllowed();
        }
        enumerable = list;
      }
      throw new InvalidOperationException("Bug in ValidateTypeGraphAcyclic");
    }

    private static Type GetIncludeTarget(MemberInfo mi) {
      Type memberType = TypeSystem.GetMemberType(mi);
      if(memberType.IsGenericType) {
        return memberType.GetGenericArguments()[0];
      }
      return memberType;
    }

    private static void ValidateSubqueryMember(MemberInfo mi) {
      Type memberType = TypeSystem.GetMemberType(mi);
      if(memberType == null) {
        throw System.Data.Linq.Error.SubqueryNotSupportedOn(mi);
      }
      if(!typeof(IEnumerable).IsAssignableFrom(memberType)) {
        throw System.Data.Linq.Error.SubqueryNotSupportedOnType(mi.Name, mi.DeclaringType);
      }
    }

    private static void ValidateSubqueryExpression(LambdaExpression subquery) {
      if(!typeof(IEnumerable).IsAssignableFrom(subquery.Body.Type)) {
        throw System.Data.Linq.Error.SubqueryMustBeSequence();
      }
      new SubqueryValidator().VisitLambda(subquery);
    }
  }

}