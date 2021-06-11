using IQToolkit.Data.Ado;
using IQToolkit.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit.Data.MySqlClient {

  public class MySqlLanguage : QueryLanguage {
    SqlDbTypeSystem typeSystem = new SqlDbTypeSystem();

    public MySqlLanguage() { }

    public override QueryTypeSystem TypeSystem => typeSystem;
    public override bool AllowsMultipleCommands => false;
    public override bool AllowDistinctInAggregates => true;
    public override string Quote(string name) => "`" + name + "`";
    private static readonly char[] splitChars = new char[] { '.' };

    public override Expression GetGeneratedIdExpression(MemberInfo member) => new FunctionExpression(TypeHelper.GetMemberType(member), "LAST_INSERT_ID()", null);
    public override Expression GetRowsAffectedExpression(Expression command) => new FunctionExpression(typeof(int), "ROW_COUNT()", null);

    public override bool IsRowsAffectedExpressions(Expression expression) {
      var fex = expression as FunctionExpression;
      return fex != null && fex.Name == "ROW_COUNT()";
    }

    public override QueryLinguist CreateLinguist(QueryTranslator translator) => new MySqlLinguist(this, translator);

    class MySqlLinguist : QueryLinguist {
      public MySqlLinguist(MySqlLanguage language, QueryTranslator translator) : base(language, translator) {
      }

      public override Expression Translate(Expression expression) {
        // fix up any order-by's
        expression = OrderByRewriter.Rewrite(Language, expression);
        expression = base.Translate(expression);
        expression = UnusedColumnRemover.Remove(expression);
        //expression = DistinctOrderByRewriter.Rewrite(expression);
        return expression;
      }
      public override string Format(Expression expression) => MySqlFormatter.Format(expression, Language);
    }

    public static readonly QueryLanguage Default = new MySqlLanguage();
  }
}