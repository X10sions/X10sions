using Common.Data.Assocations;
using System.Linq.Expressions;

namespace Common.Data.Assocations;

public static class IAssociationJoinExtensions {
  public static bool CanBeNull(this IAssociationJoin aj) => aj.Selector.IsPropertyNullable();

}