using Common.Attributes;
using System.Linq.Expressions;

namespace CommonOrm;

//  public interface IOrmClass<T> where T : class {
//  }
//, TDiscriminator

public interface xIOrmClass<T>
  : IHaveDiscriminatorExpression<T>
  , IHavePrimaryKeyExpression<T>
  , IHaveQueryFilterExpression<T>
  , IHaveUniqueKeyExpressions<T>
  where T : class {

  string? SchemaName { get; }
  string TableName { get; }

  Dictionary<string, OrmProperty<T>> Properties { get; }
  [ToDo("Not Used Yet")] Dictionary<string, OrmJoinClause<T>> AssociationJoins_NotUsedYet { get; }

}

public static class IOrmClassExtensions {

  [ToDo("Not Used Yet")]
  public static void AddToAssociationJoins_NotUsedYet<T, TOther>(this xIOrmClass<T> ormClass,
    Expression<Func<T, TOther>> propertyExpression,
    Expression<Func<T, TOther, bool>> predicateExpression,
    OrmJoinType joinType = OrmJoinType.Inner) where T : class {
    var name = GetName(propertyExpression);
    var joinClause = new OrmJoinClause<T>();
    joinClause.Init(propertyExpression, predicateExpression, joinType);
    ormClass.AssociationJoins_NotUsedYet.Add(name, joinClause);
  }


  [ToDo("Not Used Yet")]
  public static void AddToAssociationJoins_NotUsedYet<T, TOther>(this xIOrmClass<T> ormClass,
    Expression<Func<T, IEnumerable<TOther>>> propertyExpression,
    Expression<Func<T, TOther, bool>> predicateExpression,
    OrmJoinType joinType = OrmJoinType.Left) where T : class {
    var name = GetName(propertyExpression);
    var joinClause = new OrmJoinClause<T>();
    joinClause.Init(propertyExpression, predicateExpression, joinType);
    ormClass.AssociationJoins_NotUsedYet.Add(name, joinClause);
  }

  public static void AddUniqueKeyExpression<T>(this xIOrmClass<T> ormClass, Expression<Func<T, object>> value) where T : class => ormClass.UniqueKeyExpressions.Add(value);

  public static string? GetName<TSource, TField>(Expression<Func<TSource, TField>> Field) => (Field.Body as MemberExpression ?? ((UnaryExpression)Field.Body).Operand as MemberExpression)?.Member.Name;

  public static List<string> GetPrimaryKeyExpressionNames<T>(this xIOrmClass<T> ormClass) where T : class => LinqHelper.GetArgumentsMemberNames(ormClass.PrimaryKeyExpression).ToList();

  //public Expression<Func<T, object>> GetPrimaryKeyExpression() {
  //  return PrimaryKeyExpression;
  //  var qry = from x in Properties
  //            where x.Value.PrimaryKeyOrder.HasValue
  //            orderby x.Value.PrimaryKeyOrder
  //            select x.Value.PropertyExpression;
  //  return qry.Combine();
  //}

  //public void SetPrimaryKeyExpression(Expression<Func<T, object>> value) {
  //  PrimaryKeyExpression = value;
  //  var i = 0;
  //  foreach (var keyName in GetPrimaryKeyExpressionNames()) {
  //    i++;
  //    var propertyExpression = LinqHelper.GetSelectExpression<T>(keyName);
  //    var property = SetProperty(propertyExpression);
  //    property.SetPrimaryKeyOrder(i);
  //  }
  //}

  public static xIOrmClass<T> SetDiscriminator<T>(this xIOrmClass<T> ormClass, Expression<Func<T, object>> column, Type? defaultType, Dictionary<object, Type>? values = null) where T : class {
    ormClass.Discriminator = new OrmDiscriminator<T, object>(column, defaultType, values);
    return ormClass;
  }

  public static OrmProperty<T> SetProperty<T>(this xIOrmClass<T> ormClass, OrmProperty<T> property) where T : class {
    //var name = LinqHelper.GetArgumentMemberInfo(property.PropertyExpression, 0).Name;
    ormClass.Properties.GetOrAdd(property.Name, () => property);
    return property;
  }

  public static OrmProperty<T> SetProperty<T>(this xIOrmClass<T> ormClass, Expression<Func<T, object>> propertyExpression) where T : class => ormClass.SetProperty(new OrmProperty<T>(ormClass, propertyExpression));

  public static xIOrmClass<T> SetQueryFilter<T>(this xIOrmClass<T> ormClass, Expression<Func<T, bool>> filterExpression) where T : class {
    ormClass.QueryFilter = filterExpression;
    return ormClass;
  }

}