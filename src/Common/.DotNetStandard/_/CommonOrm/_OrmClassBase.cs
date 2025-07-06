using Common.Attributes;
using System.Linq.Expressions;

namespace CommonOrm {
  public class x_OrmClassBase<T> : xIOrmClass<T> where T : class {

    public x_OrmClassBase(string tableName, Expression<Func<T, object>> primaryKeyExpression, string? schemaName = null) {
      type = typeof(T);
      TableName = tableName ?? type.Name;
      PrimaryKeyExpression = primaryKeyExpression;
      SchemaName = schemaName;
    }
    Type type;


    public string DatabaseName { get; set; }
    public string TableName { get; set; }
    public string? SchemaName { get; set; }

    //_TableAttribute tableAttribute;
    //_TableAttribute TableAttribute => tableAttribute ?? new _TableAttribute(type);

    //class _TableAttribute {
    //  public _TableAttribute(Type type) {
    //    var tableAttr = type.GetCustomAttributes(true).FirstOrDefault(x => string.Equals(x.GetType().Name, "tableattribute", StringComparison.OrdinalIgnoreCase));
    //    if (tableAttr != null) {
    //      var tableAttrType= tableAttr.GetType();
    //      var x = tableAttrType.GetPropertyValueAs<typeof(attr),string>("",tableAttrType);
    //      var tableAttrProps = tableAttr.GetType().GetProperties();
    //    var   xTableName = tableAttrProps.get.FirstOrDefault(x => string.Equals(x.Name, "Name", StringComparison.OrdinalIgnoreCase)).GetValue(;
    //      SchemaName = tableAttrProps.FirstOrDefault(x => string.Equals(x.Name, "schema", StringComparison.OrdinalIgnoreCase));
    //    }
    //  }

    // public string TableName { get;set;}
    //  public string SchemaName { get;set;}
    //}


    public Dictionary<string, OrmProperty<T>> Properties { get; } = new Dictionary<string, OrmProperty<T>>();
    public Expression<Func<T, object>> PrimaryKeyExpression { get; set; }
    public List<Expression<Func<T, object>>> UniqueKeyExpressions { get; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, bool>> QueryFilter { get; set; }
    public OrmDiscriminator<T, object> Discriminator { get; set; }

    [ToDo("Not Used Yet")] public Dictionary<string, OrmJoinClause<T>> AssociationJoins_NotUsedYet => new Dictionary<string, OrmJoinClause<T>>();

  }
}