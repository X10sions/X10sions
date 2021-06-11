using System;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkit {
  /// <summary>
  /// Finds the first sub-expression that is of a specified type
  /// </summary>
  public class TypedSubtreeFinder : ExpressionVisitor {
    private readonly Type type;
    private Expression found;

    private TypedSubtreeFinder(Type type) {
      this.type = type;
    }

    public static Expression Find(Expression expression, Type type) {
      var finder = new TypedSubtreeFinder(type);
      finder.Visit(expression);
      return finder.found;
    }

    public override Expression Visit(Expression exp) {
      var node = base.Visit(exp);
      // remember the first sub-expression that is of an appropriate type
      if (found == null && node != null && type.GetTypeInfo().IsAssignableFrom(node.Type.GetTypeInfo())) {
        found = node;
      }
      return node;
    }
  }
}