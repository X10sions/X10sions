using Remotion.Linq.EagerFetching;

namespace NHibernate_v5_2_6.Linq.Visitors.ResultOperatorProcessors
{
    public class ProcessFetchMany : ProcessFetch, IResultOperatorProcessor<FetchManyRequest>
    {
        public void Process(FetchManyRequest resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree)
        {
            base.Process(resultOperator, queryModelVisitor, tree);
        }
    }
}