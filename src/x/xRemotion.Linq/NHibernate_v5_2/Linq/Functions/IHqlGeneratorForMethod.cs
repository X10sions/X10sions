using NHibernate.Hql.Ast;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate_v5_2.Linq.Functions {
  public interface IHqlGeneratorForMethod {
    IEnumerable<MethodInfo> SupportedMethods { get; }
    HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor);
  }
}