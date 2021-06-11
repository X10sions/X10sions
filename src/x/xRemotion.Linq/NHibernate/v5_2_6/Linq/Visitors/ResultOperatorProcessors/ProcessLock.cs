namespace NHibernate_v5_2_6.Linq.Visitors.ResultOperatorProcessors
{
	internal class ProcessLock : IResultOperatorProcessor<LockResultOperator>
	{
		public void Process(LockResultOperator resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree)
		{
			var alias = queryModelVisitor.VisitorParameters.QuerySourceNamer.GetName(resultOperator.QuerySource);
			tree.AddAdditionalCriteria((q, p) => q.SetLockMode(alias, (LockMode) resultOperator.LockMode.Value));
		}
	}
}
