using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MemberTranslators {
  public class AS400DateTimeUtcNowTranslator : IMemberTranslator {

    public virtual Expression Translate(MemberExpression memberExpression)
      => memberExpression.DoMemberTranslate(nameof(DateTime.TimeOfDay), typeof(DateTime))
      ? new SqlFunctionExpression("CURRENT TIMESTAMP - CURRENT TIMEZONE", memberExpression.Type)
      : null;

  }
}