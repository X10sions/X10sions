//using System;

namespace LinqToDB.SqlQuery {
  public static class IValueContainerExtensions {

    //public static ISqlExpression GetDateParmeter(this IValueContainer parameter) {
    //  if (parameter != null && parameter is SqlParameter) {
    //    var p = (SqlParameter)parameter;
    //    p.Type.DataType = DataType.Date;
    //    return p;
    //  }
    //  return null;
    //}

    //public static ISqlExpression GetParmeter(this IValueContainer parameter, Type type) {
    //  if (type != null && parameter != null) {
    //    if (parameter is SqlValue) {
    //      if (((SqlValue)parameter).SystemType == null)
    //        return new SqlValue(type, parameter.Value);
    //    } else if (parameter is SqlParameter) {
    //      var p = (SqlParameter)parameter;
    //      p.SystemType = p.SystemType ?? type;
    //      return p;
    //    }
    //  }
    //  return null;
    //}

  }
}