using System.Linq.Expressions;

namespace Common.ToSql {
  public static class Evaluator {
    // https://stackoverflow.com/questions/30308124/force-a-net-expression-to-use-current-value

    /// <summary>
    /// Performs evaluation & replacement of independent sub-trees
    /// </summary>
    /// <param name="expression">The root of the expression tree.</param>
    /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
    /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
    public static Expression? PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated) => new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);

    /// <summary>
    /// Performs evaluation & replacement of independent sub-trees
    /// </summary>
    /// <param name="expression">The root of the expression tree.</param>
    /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
    public static Expression? PartialEval(Expression expression) => PartialEval(expression, CanBeEvaluatedLocally);

    private static bool CanBeEvaluatedLocally(Expression expression) => expression.NodeType != ExpressionType.Parameter;

    /// <summary>
    /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
    /// </summary>
    sealed class SubtreeEvaluator : ExpressionVisitor {
      readonly HashSet<Expression> candidates;

      internal SubtreeEvaluator(HashSet<Expression> candidates) {
        this.candidates = candidates;
      }

      internal Expression? Eval(Expression exp) => Visit(exp);

      public override Expression? Visit(Expression node) => node == null ? null : candidates.Contains(node) ? Evaluate(node) : base.Visit(node);

      static Expression Evaluate(Expression e) {
        if (e.NodeType == ExpressionType.Constant) {
          return e;
        }
        var lambda = Expression.Lambda(e);
        var fn = lambda.Compile();
        return Expression.Constant(fn.DynamicInvoke(null), e.Type);
      }
    }

    /// <summary>
    /// Performs bottom-up analysis to determine which nodes can possibly
    /// be part of an evaluated sub-tree.
    /// </summary>
    sealed class Nominator : ExpressionVisitor {
      readonly Func<Expression, bool> fnCanBeEvaluated;
      HashSet<Expression> candidates;
      bool cannotBeEvaluated;

      internal Nominator(Func<Expression, bool> fnCanBeEvaluated) {
        this.fnCanBeEvaluated = fnCanBeEvaluated;
      }

      internal HashSet<Expression> Nominate(Expression expression) {
        candidates = new HashSet<Expression>();
        Visit(expression);
        return candidates;
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

  public static class EvaluatorExtensions {
    public static IQueryable<T> Simplify<T>(this IQueryable<T> query) => new Query<T>(query.Provider, Evaluator.PartialEval(query.Expression));
  }
}
