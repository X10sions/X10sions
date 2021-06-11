using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using xEFCore.xAS400.Query.Sql.Internal;

namespace xEFCore.xAS400.Query.Expressions.Internal {
  public class DatePartExpression_DB2 : Expression {

    public DatePartExpression_DB2 ([NotNull] string datePart, [NotNull] Type type, [NotNull] IEnumerable<Expression> arguments) {
      DatePart = datePart;
      Type = type;
      _arguments = arguments.ToList().AsReadOnly();
    }

    readonly ReadOnlyCollection<Expression> _arguments;

    public override Type Type {      get;    }
    public override ExpressionType NodeType => ExpressionType.Extension;
    public virtual IReadOnlyCollection<Expression> Argument => _arguments;
    public virtual string DatePart {      get;    }

      protected override Expression Accept(ExpressionVisitor visitor) {
      Check.NotNull(visitor, nameof(visitor));
      IAS400ExpressionVisitor db2ExpressionVisitor = visitor as IAS400ExpressionVisitor;
      if (db2ExpressionVisitor == null) {
        return base.Accept(visitor);
      }
      return db2ExpressionVisitor.VisitDatePartExpression_DB2(this);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) {
      return this;
    }

  }
}