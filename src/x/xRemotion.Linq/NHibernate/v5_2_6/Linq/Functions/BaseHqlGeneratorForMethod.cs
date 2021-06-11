using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;

namespace NHibernate_v5_2_6.Linq.Functions
{
    public abstract class BaseHqlGeneratorForMethod : IHqlGeneratorForMethod
    {
        public IEnumerable<MethodInfo> SupportedMethods { get; protected set; }

        public abstract HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
    }
}