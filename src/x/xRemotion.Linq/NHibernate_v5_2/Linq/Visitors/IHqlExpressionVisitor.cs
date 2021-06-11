using NHibernate;
using NHibernate.Hql.Ast;
using System.Linq.Expressions;

namespace NHibernate_v5_2.Linq.Visitors {
  public interface IHqlExpressionVisitor {
    ISessionFactory SessionFactory { get; }
    HqlTreeNode Visit(Expression expression);
  }
}