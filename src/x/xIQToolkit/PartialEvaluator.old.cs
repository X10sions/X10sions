using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit {

  /// <summary>
  /// Rewrites an expression tree so that locally isolatable sub-expressions are evaluated
  /// and converted into ConstantExpression nodes.
  /// </summary>
  /// <see cref="https://github.com/mattwar/iqtoolkit/blob/master/src/IQToolkit/PartialEvaluator.cs"/>
  public static class PartialEvaluator {
    /// <summary>
    /// Performs evaluation and replacement of independent sub-trees
    /// </summary>
    /// <param name="expression">The root of the expression tree.</param>
    /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
    public static Expression? Eval(Expression expression) => Eval(expression, null, null);

    /// <summary>
    /// Performs evaluation and replacement of independent sub-trees
    /// </summary>
    /// <param name="expression">The root of the expression tree.</param>
    /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
    /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
    public static Expression? Eval(Expression expression, Func<Expression, bool> fnCanBeEvaluated) => Eval(expression, fnCanBeEvaluated, null);

    /// <summary>
    /// Performs evaluation and replacement of independent sub-trees
    /// </summary>
    /// <param name="expression">The root of the expression tree.</param>
    /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
    /// <param name="fnPostEval">A function to apply to each newly formed <see cref="ConstantExpression"/>.</param>
    /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
    public static Expression? Eval(Expression expression, Func<Expression, bool>? fnCanBeEvaluated, Func<ConstantExpression, Expression>? fnPostEval) {
      if (fnCanBeEvaluated == null)
        fnCanBeEvaluated = CanBeEvaluatedLocally;
      return SubtreeEvaluator.Eval(Nominator.Nominate(fnCanBeEvaluated, expression), fnPostEval, expression);
    }

    private static bool CanBeEvaluatedLocally(Expression expression) {
      return expression.NodeType != ExpressionType.Parameter;
    }

    /// <summary>
    /// Evaluates and replaces sub-trees when first candidate is reached (top-down)
    /// </summary>
    sealed class SubtreeEvaluator : ExpressionVisitor {
      readonly HashSet<Expression> candidates;
      readonly Func<ConstantExpression, Expression> onEval;

      private SubtreeEvaluator(HashSet<Expression> candidates, Func<ConstantExpression, Expression>? onEval) {
        this.candidates = candidates;
        this.onEval = onEval;
      }

      internal static Expression? Eval(HashSet<Expression> candidates, Func<ConstantExpression, Expression>? onEval, Expression exp) => new SubtreeEvaluator(candidates, onEval).Visit(exp);

      public override Expression? Visit(Expression exp) {
        if (exp == null) {
          return null;
        }
        if (candidates.Contains(exp)) {
          return Evaluate(exp);
        }
        return base.Visit(exp);
      }

      protected override Expression? VisitConditional(ConditionalExpression c) {
        // if the conditional test can be evaluated locally, rewrite expression
        // to the valid case
        if (candidates.Contains(c.Test)) {
          var test = Evaluate(c.Test);
          if (test is ConstantExpression && ((ConstantExpression)test).Type == typeof(bool)) {
            return (bool)((ConstantExpression)test).Value ? Visit(c.IfTrue) : Visit(c.IfFalse);
          }
        }
        return base.VisitConditional(c);
      }

      private Expression PostEval(ConstantExpression e) {
        if (this.onEval != null) {
          return this.onEval(e);
        }
        return e;
      }

      private Expression Evaluate(Expression e) {
        var type = e.Type;

        if (e.NodeType == ExpressionType.Convert) {
          // check for unnecessary convert & strip them
          var u = (UnaryExpression)e;
          if (TypeHelper.GetNonNullableType(u.Operand.Type) == TypeHelper.GetNonNullableType(type)) {
            e = ((UnaryExpression)e).Operand;
          }
        }

        if (e.NodeType == ExpressionType.Constant) {
          // in case we actually threw out a nullable conversion above, simulate it here
          // don't post-eval nodes that were already constants
          if (e.Type == type) {
            return e;
          } else if (TypeHelper.GetNonNullableType(e.Type) == TypeHelper.GetNonNullableType(type)) {
            return Expression.Constant(((ConstantExpression)e).Value, type);
          }
        }

        var me = e as MemberExpression;
        if (me != null) {
          // member accesses off of constant's are common, and yet since these partial evals
          // are never re-used, using reflection to access the member is faster than compiling
          // and invoking a lambda
          var ce = me.Expression as ConstantExpression;
          if (ce != null) {
            return this.PostEval(Expression.Constant(me.Member.GetValue(ce.Value), type));
          }
        }

        if (type.GetTypeInfo().IsValueType) {
          e = Expression.Convert(e, typeof(object));
        }

        var lambda = Expression.Lambda<Func<object>>(e);
#if NOREFEMIT
                Func<object> fn = ExpressionEvaluator.CreateDelegate(lambda);
#else
        var fn = lambda.Compile();
#endif
        return this.PostEval(Expression.Constant(fn(), type));
      }
    }

    /// <summary>
    /// Performs bottom-up analysis to determine which nodes can possibly
    /// be part of an evaluated sub-tree.
    /// </summary>
    sealed class Nominator : ExpressionVisitor {
      Func<Expression, bool> fnCanBeEvaluated;
      HashSet<Expression> candidates;
      bool cannotBeEvaluated;

      private Nominator(Func<Expression, bool> fnCanBeEvaluated) {
        this.candidates = new HashSet<Expression>();
        this.fnCanBeEvaluated = fnCanBeEvaluated;
      }

      internal static HashSet<Expression> Nominate(Func<Expression, bool> fnCanBeEvaluated, Expression expression) {
        var nominator = new Nominator(fnCanBeEvaluated);
        nominator.Visit(expression);
        return nominator.candidates;
      }

      protected override Expression VisitConstant(ConstantExpression c) {
        return base.VisitConstant(c);
      }
      public override Expression? Visit(Expression expression) {
        if (expression != null) {
          var saveCannotBeEvaluated = cannotBeEvaluated;
          cannotBeEvaluated = false;
          base.Visit(expression);
          if (!cannotBeEvaluated) {
            if (fnCanBeEvaluated(expression)) {
              candidates.Add(expression);
            } else {
              cannotBeEvaluated = true;
            }
          }
          cannotBeEvaluated |= saveCannotBeEvaluated;
        }
        return expression;
      }
    }
  }
}