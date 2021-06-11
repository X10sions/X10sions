namespace NHibernate_v5_2_6.Linq.Visitors.ResultOperatorProcessors
{
	public interface IResultOperatorProcessor<T>
	{
		void Process(T resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree);
	}
}