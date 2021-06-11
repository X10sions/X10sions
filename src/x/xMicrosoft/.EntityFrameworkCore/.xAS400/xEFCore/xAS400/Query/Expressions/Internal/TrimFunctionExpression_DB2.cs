using JetBrains.Annotations;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using xEFCore.xAS400.Query.Sql.Internal;

namespace xEFCore.xAS400.Query.Expressions.Internal {

  [DebuggerDisplay("{this.FunctionName}({string.Join(\", \", this.Arguments)})")]
  public class TrimFunctionExpression_DB2 : Expression {

    public TrimFunctionExpression_DB2([NotNull] string functionName, [NotNull] Type returnType)
      : this(functionName, returnType, Enumerable.Empty<Expression>()) {
    }

    public TrimFunctionExpression_DB2([NotNull] string functionName, [NotNull] Type returnType, [NotNull] IEnumerable<Expression> arguments) {
      FunctionName = functionName;
      Type = returnType;
      _arguments = arguments.ToList().AsReadOnly();
    }

    readonly ReadOnlyCollection<Expression> _arguments;

    public virtual string FunctionName { get; [param: NotNull]      set; }
    public virtual IReadOnlyCollection<Expression> Arguments => _arguments;
    public override ExpressionType NodeType => ExpressionType.Extension;
    public override Type Type { get; }
    

    protected override Expression Accept(ExpressionVisitor visitor) {
      Check.NotNull(visitor, nameof(visitor));
      IAS400ExpressionVisitor db2ExpressionVisitor = visitor as IAS400ExpressionVisitor;
      if (db2ExpressionVisitor == null) {
        return base.Accept(visitor);
      }
      return db2ExpressionVisitor.VisitTrimFunction_DB2(this);
    }

    protected override Expression VisitChildren(ExpressionVisitor visitor) {
      ReadOnlyCollection<Expression> readOnlyCollection = visitor.VisitAndConvert(_arguments, nameof(VisitChildren));
      if (readOnlyCollection == _arguments) {
        return this;
      }
      return new TrimFunctionExpression_DB2(FunctionName, Type, readOnlyCollection);
    }
  }

}