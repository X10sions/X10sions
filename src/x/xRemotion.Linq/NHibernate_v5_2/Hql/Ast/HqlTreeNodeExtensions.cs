using NHibernate.Hql.Ast;

namespace NHibernate_v5_2.Hql.Ast {
  public static class HqlTreeNodeExtensions {

    internal static HqlExpression ToArithmeticExpression(this HqlTreeNode node) {
      var hqlBooleanExpression = node as HqlBooleanExpression;
      if (hqlBooleanExpression != null) {
        var builder = new HqlTreeBuilder();

        return builder.Case(new[] { builder.When(hqlBooleanExpression, builder.True()) }, builder.False());
      }

      return (HqlExpression)node;
    }

  }
}
