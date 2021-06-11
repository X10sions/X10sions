//using LinqToDB.SqlProvider;
//using LinqToDB.SqlQuery;

//namespace LinqToDB.DataProvider.Access {
//  public class AccessSqlOptimizer : BasicSqlOptimizer {
//    public AccessSqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags) { }

//    public override SqlStatement Finalize(SqlStatement statement, bool inlineParameters) {
//      base.Finalize(statement, inlineParameters);
//      switch (statement.QueryType) {
//        case QueryType.Delete:
//          return GetAlternativeDelete((SqlDeleteStatement)statement);
//        default:
//          return statement;
//      }
//    }

//    public override bool ConvertCountSubQuery(SelectQuery subQuery) => !subQuery.Where.IsEmpty;
//  }

//}