using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Remotion.Linq.Clauses;
using xEFCore.xAS400.Query.Sql.Internal;

namespace xEFCore.xAS400.Query.Expressions.Internal {
  public class RowNumberExpression_SqlServer : Expression {
     readonly List<Ordering> _orderings = new List<Ordering>(); public RowNumberExpression_SqlServer([NotNull] IReadOnlyList<Ordering> orderings) {
      Check.NotNull(orderings, nameof(orderings));

      _orderings.AddRange(orderings);
    }
    public override ExpressionType NodeType => ExpressionType.Extension; public override bool CanReduce => false; public override Type Type => typeof(int); public virtual IReadOnlyList<Ordering> Orderings => _orderings; protected override Expression Accept(ExpressionVisitor visitor) {
      Check.NotNull(visitor, nameof(visitor));
      var specificVisitor = visitor as IAS400ExpressionVisitor;
      return specificVisitor != null ? specificVisitor.VisitRowNumber_SqlServer(this) : base.Accept(visitor);
    }
    protected override Expression VisitChildren(ExpressionVisitor visitor) {
      var newOrderings = new List<Ordering>();
      var recreate = false;
      foreach (var ordering in _orderings) {
        var newOrdering = new Ordering(visitor.Visit(ordering.Expression), ordering.OrderingDirection);
        newOrderings.Add(newOrdering);
        recreate |= newOrdering.Expression != ordering.Expression;
      }
      return recreate ? new RowNumberExpression_SqlServer(newOrderings) : this;
    }
    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      return obj.GetType() == GetType() && Equals((RowNumberExpression_SqlServer)obj);
    }

    bool Equals([NotNull] RowNumberExpression_SqlServer other) => _orderings.SequenceEqual(other._orderings); public override int GetHashCode()
         => _orderings.Aggregate(0, (current, ordering) => current + ((current * 397) ^ ordering.GetHashCode()));

  }
}