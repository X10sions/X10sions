using Remotion.Linq.Clauses.ResultOperators;

namespace NHibernate_v5_2_6.Linq.ResultOperators
{
	public class NonAggregatingGroupBy : ClientSideTransformOperator
	{
		public NonAggregatingGroupBy(GroupResultOperator groupBy)
		{
			GroupBy = groupBy;
		}

		public GroupResultOperator GroupBy { get; private set; }
	}
}