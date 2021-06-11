using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit.Data.SqlServerCe {

  public class SqlCeLanguage : QueryLanguage {
    SqlDbTypeSystem typeSystem = new SqlDbTypeSystem();

    public SqlCeLanguage() { }

    public override QueryTypeSystem TypeSystem => typeSystem;
    public override bool AllowsMultipleCommands => false;
    public override bool AllowDistinctInAggregates => false;

    public override string Quote(string name) {
      if (name.StartsWith("[") && name.EndsWith("]")) {
        return name;
      } else if (name.IndexOf('.') > 0) {
        return "[" + string.Join("].[", name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)) + "]";
      } else {
        return "[" + name + "]";
      }
    }

    private static readonly char[] splitChars = new char[] { '.' };

    public override Expression GetGeneratedIdExpression(MemberInfo member) => new FunctionExpression(TypeHelper.GetMemberType(member), "@@IDENTITY", null);

    public override QueryLinguist CreateLinguist(QueryTranslator translator) => new SqlCeLinguist(this, translator);

    class SqlCeLinguist : QueryLinguist {
      public SqlCeLinguist(SqlCeLanguage language, QueryTranslator translator)
          : base(language, translator) {
      }

      public override Expression Translate(Expression expression) {
        // fix up any order-by's
        expression = OrderByRewriter.Rewrite(Language, expression);
        expression = base.Translate(expression);
        expression = SkipToNestedOrderByRewriter.Rewrite(Language, expression);
        expression = OrderByRewriter.Rewrite(Language, expression);
        expression = UnusedColumnRemover.Remove(expression);
        expression = RedundantSubqueryRemover.Remove(expression);
        expression = ScalarSubqueryRewriter.Rewrite(Language, expression);
        return expression;
      }

      public override string Format(Expression expression) => expression.FormatSqlCe(Language);
    }

    public static readonly QueryLanguage Default = new SqlCeLanguage();
  }
}