using NHibernate.Hql.Ast;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate_v5_2.Linq.Functions {
  public interface IHqlGeneratorForProperty {
    IEnumerable<MemberInfo> SupportedProperties { get; }
    HqlTreeNode BuildHql(MemberInfo member, Expression expression, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
  }
}