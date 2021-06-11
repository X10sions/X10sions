using Remotion.Linq.Clauses.ResultOperators;

namespace NHibernate_v5_2_6.Linq.Visitors.ResultOperatorProcessors
{
	public class ProcessOfType : IResultOperatorProcessor<OfTypeResultOperator>
	{
		public void Process(OfTypeResultOperator resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree)
		{
			var source = queryModelVisitor.Model.SelectClause.GetOutputDataInfo().ItemExpression;

			var expression = new HqlGeneratorExpressionVisitor(queryModelVisitor.VisitorParameters)
				.BuildOfType(source, resultOperator.SearchedItemType);

			tree.AddWhereClause(expression);
		}
	}
}