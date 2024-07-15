using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Common;
using System.Linq.Expressions;

namespace System.Data.Linq;

internal static class Funcletizer {

  internal static Expression Funcletize(Expression expression) {
    return new Localizer(new LocalMapper().MapLocals(expression)).Localize(expression);
  }

  class Localizer : ExpressionVisitor {
    Dictionary<Expression, bool> locals;

    internal Localizer(Dictionary<Expression, bool> locals) {
      this.locals = locals;
    }

    internal Expression Localize(Expression expression) {
      return this.Visit(expression);
    }

    internal override Expression Visit(Expression exp) {
      if (exp == null) {
        return null;
      }
      if (this.locals.ContainsKey(exp)) {
        return MakeLocal(exp);
      }
      if (exp.NodeType == (ExpressionType)InternalExpressionType.Known) {
        return exp;
      }
      return base.Visit(exp);
    }

    private static Expression MakeLocal(Expression e) {
      if (e.NodeType == ExpressionType.Constant) {
        return e;
      } else if (e.NodeType == ExpressionType.Convert || e.NodeType == ExpressionType.ConvertChecked) {
        UnaryExpression ue = (UnaryExpression)e;
        if (ue.Type == typeof(object)) {
          Expression local = MakeLocal(ue.Operand);
          return (e.NodeType == ExpressionType.Convert) ? Expression.Convert(local, e.Type) : Expression.ConvertChecked(local, e.Type);
        }
        // convert a const null
        if (ue.Operand.NodeType == ExpressionType.Constant) {
          ConstantExpression c = (ConstantExpression)ue.Operand;
          if (c.Value == null) {
            return Expression.Constant(null, ue.Type);
          }
        }
      }
      return Expression.Invoke(Expression.Constant(Expression.Lambda(e).Compile()));
    }
  }
  class DependenceChecker : ExpressionVisitor {
    HashSet<ParameterExpression> inScope = new HashSet<ParameterExpression>();
    bool isIndependent = true;

    /// <summary>
    /// This method returns 'true' when the expression doesn't reference any parameters 
    /// from outside the scope of the expression.
    /// </summary>
    static public bool IsIndependent(Expression expression) {
      var v = new DependenceChecker();
      v.Visit(expression);
      return v.isIndependent;
    }
    internal override Expression VisitLambda(LambdaExpression lambda) {
      foreach (var p in lambda.Parameters) {
        this.inScope.Add(p);
      }
      return base.VisitLambda(lambda);
    }
    internal override Expression VisitParameter(ParameterExpression p) {
      this.isIndependent &= this.inScope.Contains(p);
      return p;
    }
  }

  class LocalMapper : ExpressionVisitor {
    bool isRemote;
    Dictionary<Expression, bool> locals;

    internal Dictionary<Expression, bool> MapLocals(Expression expression) {
      this.locals = new Dictionary<Expression, bool>();
      this.isRemote = false;
      this.Visit(expression);
      return this.locals;
    }

    internal override Expression Visit(Expression expression) {
      if (expression == null) {
        return null;
      }
      bool saveIsRemote = this.isRemote;
      switch (expression.NodeType) {
        case (ExpressionType)InternalExpressionType.Known:
          return expression;
        case (ExpressionType)ExpressionType.Constant:
          break;
        default:
          this.isRemote = false;
          base.Visit(expression);
          if (!this.isRemote
              && expression.NodeType != ExpressionType.Lambda
              && expression.NodeType != ExpressionType.Quote
              && DependenceChecker.IsIndependent(expression)) {
            this.locals[expression] = true; // Not 'Add' because the same expression may exist in the tree twice. 
          }
          break;
      }
      if (typeof(ITable).IsAssignableFrom(expression.Type) ||
          typeof(DataContext).IsAssignableFrom(expression.Type)) {
        this.isRemote = true;
      }
      this.isRemote |= saveIsRemote;
      return expression;
    }
    internal override Expression VisitMemberAccess(MemberExpression m) {
      base.VisitMemberAccess(m);
      this.isRemote |= (m.Expression != null && typeof(ITable).IsAssignableFrom(m.Expression.Type));
      return m;
    }
    internal override Expression VisitMethodCall(MethodCallExpression m) {
      base.VisitMethodCall(m);
      this.isRemote |= m.Method.DeclaringType == typeof(System.Data.Linq.Provider.DataManipulation)
                    || Attribute.IsDefined(m.Method, typeof(FunctionAttribute));
      return m;
    }
  }
}
