using System;
using Remotion.Linq.Clauses;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using xEFCore.xAS400.Query.Sql.Internal;

namespace xEFCore.xAS400.Query.Expressions.Internal {
  public class RowNumberExpression_DB2 : Expression {

    public RowNumberExpression_DB2([NotNull] IReadOnlyList<Ordering> orderings) {
      Check.NotNull(orderings, nameof(orderings));
      _orderings.AddRange(orderings);
    }

    readonly List<Ordering> _orderings = new List<Ordering>();
    public override ExpressionType NodeType => ExpressionType.Extension;
    public override bool CanReduce => false;
    public override Type Type => typeof(int);
    public virtual IReadOnlyList<Ordering> Orderings => _orderings;

    protected override Expression Accept(ExpressionVisitor visitor) {
      Check.NotNull(visitor, nameof(visitor));
      IAS400ExpressionVisitor db2ExpressionVisitor = visitor as IAS400ExpressionVisitor;
      if (db2ExpressionVisitor == null) {
        return base.Accept(visitor);
      }
      return db2ExpressionVisitor.VisitRowNumber_DB2(this);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) {
      List<Ordering> list = new List<Ordering>();
      bool flag = false;
      foreach (Ordering ordering2 in _orderings) {
        Ordering ordering = new Ordering(visitor.Visit(ordering2.Expression), ordering2.OrderingDirection);
        list.Add(ordering);
        flag |= (ordering.Expression != ordering2.Expression);
      }
      if (!flag) {
        return this;
      }
      return new RowNumberExpression_DB2(list);
    }

    public override bool Equals(object obj) {
      if (obj == null) {
        return false;
      }
      if (this == obj) {
        return true;
      }
      if (obj.GetType() == base.GetType()) {
        return Equals((RowNumberExpression_DB2 )obj);
      }
      return false;
    }

     bool Equals([NotNull] RowNumberExpression_DB2 other) {
      return _orderings.SequenceEqual(other._orderings);
    }

    public override int GetHashCode() {
      return _orderings.Aggregate(0, (int current, Ordering ordering) => current + (current * 397 ^ ordering.GetHashCode()));
    }

  }
}