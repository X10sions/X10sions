using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MemberTranslators {
  public class AS400DateTimeNowTranslator : IMemberTranslator {

    public virtual Expression Translate(MemberExpression memberExpression)
      => memberExpression.DoMemberTranslate(nameof(DateTime.Now), typeof(DateTime))
      ? new SqlFunctionExpression("CURRENT TIMESTAMP", memberExpression.Type)
      : null;

  }
}