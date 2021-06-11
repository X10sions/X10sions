using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MemberTranslators {
  public class AS400DateTimeDateTranslator : IMemberTranslator {

    public virtual Expression Translate(MemberExpression memberExpression)
      => memberExpression.DoMemberTranslate(nameof(DateTime.Date), typeof(DateTime), typeof(DateTimeOffset))
      ? new ExplicitCastExpression(new SqlFunctionExpression("Date", memberExpression.Type, new[] { memberExpression.Expression }), typeof(DateTime))
      : null;

  }
}