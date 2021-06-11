using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace xEFCore.xAS400.Query.ExpressionTranslators.Internal.MemberTranslators {
  public class AS400StringLengthTranslator : IMemberTranslator {

    public virtual Expression Translate(MemberExpression memberExpression)
      => memberExpression.DoMemberTranslate(nameof(string.Length), typeof(string))
      ? new ExplicitCastExpression(new SqlFunctionExpression("LENGTH", memberExpression.Type, new[] { memberExpression.Expression }), typeof(int))
      : null;

  }
}