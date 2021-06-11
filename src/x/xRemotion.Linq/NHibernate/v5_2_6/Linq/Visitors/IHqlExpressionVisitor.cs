using System.Linq.Expressions;
using NHibernate.Hql.Ast;

namespace NHibernate_v5_2_6.Linq.Visitors
{
	public interface IHqlExpressionVisitor
	{
		ISessionFactory SessionFactory { get; }

		HqlTreeNode Visit(Expression expression);
	}
}
