using System.Linq.Expressions;

namespace NHibernate_v5_2_6.Linq.NestedSelects
{
	class ExpressionHolder
	{
		public int Tuple { get; set; }
		public Expression Expression { get; set; }
	}
}