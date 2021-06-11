using JetBrains.Annotations;
using System;
using System.Linq.Expressions;
using xEFCore.xAS400.Query.Sql.Internal;

namespace xEFCore.xAS400.Query.Expressions.Internal {
  public class DayOfYearExpression_DB2 : Expression {

    public DayOfYearExpression_DB2([NotNull] DatePartExpression_DB2 dateExpression,
     [NotNull] Type type, [NotNull] DatePartExpression_DB2 mdyExpression) {
      DateExpression = dateExpression;
      Type = type;
      MDYExpression = mdyExpression;
    }

    public override Type Type { get; }
    public override ExpressionType NodeType => ExpressionType.Extension;
    public virtual DatePartExpression_DB2 MDYExpression { get; }
    public virtual DatePartExpression_DB2 DateExpression { get; }

    protected override Expression Accept(ExpressionVisitor visitor) {
      Check.NotNull(visitor, nameof(visitor));
      IAS400ExpressionVisitor db2ExpressionVisitor = visitor as IAS400ExpressionVisitor;
      if (db2ExpressionVisitor == null) {
        return base.Accept(visitor);
      }
      return db2ExpressionVisitor.VisitDayOfYearExpression_DB2(this);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) {
      return this;
    }

  }
}