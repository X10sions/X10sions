using System.Linq.Expressions;
using System.Reflection;

namespace Common.Data.Assocations;

public interface IAssociationJoin {
  LambdaExpression Selector { get; }
  LambdaExpression Predicate { get; }
}

public interface IAssociationJoin<T1, T2> : IAssociationJoin where T1 : class? where T2 : class? {
  new Expression<Func<T1, T2, bool>> Predicate { get; }
}

public interface IAssociationJoinOneToOne<T1, T2> : IAssociationJoin<T1, T2> where T1 : class? where T2 : class? {
  new Expression<Func<T1, T2?>> Selector { get; }
}

public interface IAssociationJoinManyToOne<T1, T2> : IAssociationJoin<T1, T2> where T1 : class? where T2 : class? {
  new Expression<Func<T1, IEnumerable<T2>>> Selector { get; }
}

public static class IAssociationJoinExtensions {
  public static bool IsMany(this IAssociationJoin aj) => aj.Selector.IsIEnumerable();
  public static bool CanBeNull(this IAssociationJoin aj) => aj.Selector.IsPropertyNullable();
  public static PropertyInfo GetPropertyInfo(this IAssociationJoin aj) => aj.Selector.GetMemberInfo().GetPropertyInfo();
  public static MemberInfo GetMemberInfo(this IAssociationJoin aj) => aj.Selector.GetMemberInfo();
}
