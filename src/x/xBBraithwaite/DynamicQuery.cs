using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BBaithwaite {
  public sealed class DynamicQuery {
    // https://github.com/bbraithwaite/RepoWrapper/blob/master/DynamicQuery.cs

    public static string GetInsertQuery(string tableName, object item) {
      PropertyInfo[] props = item.GetType().GetProperties();
      string[] columns = (from p in props select p.Name).Where(s => s != "ID").ToArray();
      return string.Format("INSERT INTO {0} ({1}) OUTPUT inserted.ID VALUES (@{2})", tableName, string.Join(",", columns), string.Join(",@", columns));
    }

    public static string GetUpdateQuery(string tableName, object item) {
      PropertyInfo[] props = item.GetType().GetProperties();
      string[] columns = props.Select(p => p.Name).ToArray();
      var parameters = columns.Select(name => name + "=@" + name).ToList();
      return string.Format("UPDATE {0} SET {1} WHERE ID=@ID", tableName, string.Join(",", parameters));
    }

    public static DynamicQueryResult GetDynamicQuery<T>(string tableName, Expression<Func<T, bool>> expression) {
      var queryProperties = new List<DynamicQueryParameter>();
      var body = (BinaryExpression)expression.Body;
      IDictionary<string, object> expando = new ExpandoObject();
      var builder = new StringBuilder();
      WalkTree(body, ExpressionType.Default, ref queryProperties);
      builder.Append("SELECT * FROM ");
      builder.Append(tableName);
      builder.Append(" WHERE ");
      var loopTo = queryProperties.Count() - 1;
      for (int i = 0; i <= loopTo; i++) {
        DynamicQueryParameter item = queryProperties[i];
        if (!string.IsNullOrEmpty(item.LinkingOperator) && i > 0)
          builder.Append($"{item.LinkingOperator} {item.PropertyName} {item.QueryOperator} @{item.PropertyName} ");
        else
          builder.Append($"{item.PropertyName} {item.QueryOperator} @{item.PropertyName} ");
        expando[item.PropertyName] = item.PropertyValue;
      }
      return new DynamicQueryResult(builder.ToString().TrimEnd(), expando);
    }

     static void WalkTree(BinaryExpression body, ExpressionType linkingType, ref List<DynamicQueryParameter> queryProperties) {
      if (body.NodeType != ExpressionType.AndAlso && body.NodeType != ExpressionType.OrElse) {
        string propertyName = GetPropertyName(body);
        var propertyValue = Expression.Constant(body.Right);
        string opr = body.NodeType.ToSqlString();
        string link = linkingType.ToSqlString();
        queryProperties.Add(new DynamicQueryParameter(link, propertyName, propertyValue.Value, opr));
      } else {
        WalkTree((BinaryExpression)body.Left, body.NodeType, ref queryProperties);
        WalkTree((BinaryExpression)body.Right, body.NodeType, ref queryProperties);
      }
    }

     static string GetPropertyName(BinaryExpression body) {
      string propertyName = body.Left.ToString().Split('.')[1];
      if (body.Left.NodeType == ExpressionType.Convert)
        propertyName = propertyName.Replace(")", string.Empty);
      return propertyName;
    }

  }
}
