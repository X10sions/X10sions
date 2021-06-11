using NHibernate.Hql.Ast;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate_v5_2.Linq.Functions {
  public abstract class BaseHqlGeneratorForMethod : IHqlGeneratorForMethod {
    public IEnumerable<MethodInfo> SupportedMethods { get; protected set; }

    public abstract HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
  }
}