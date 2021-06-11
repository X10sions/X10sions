using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;

namespace NHibernate_v5_2_6.Linq.Functions
{
    public abstract class BaseHqlGeneratorForProperty : IHqlGeneratorForProperty
    {
        public IEnumerable<MemberInfo> SupportedProperties { get; protected set; }
        public abstract HqlTreeNode BuildHql(MemberInfo member, Expression expression, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
    }
}