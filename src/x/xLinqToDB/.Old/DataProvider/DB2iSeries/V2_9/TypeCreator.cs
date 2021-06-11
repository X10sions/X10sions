using System;
using System.Linq.Expressions;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9 {

  public class TypeCreator : TypeCreatorBase {
    private Func<object> _creator;
    public dynamic CreateInstance() {
      if (_creator == null) {
        var expr = Expression.Lambda<Func<object>>(Expression.Convert(Expression.New(Type), typeof(object)));
        _creator = expr.Compile();
      }
      return _creator();
    }
  }

  public class TypeCreator<T> : TypeCreator {
    private Func<T, object> _creator;
    public dynamic CreateInstance(T value) => (_creator ?? (_creator = GetCreator<T>()))(value);
  }

  public class TypeCreator<T1, T> : TypeCreator<T1> {
    private Func<T, object> _creator;
    public dynamic CreateInstance(T value) => (_creator ?? (_creator = GetCreator<T>()))(value);
  }

  public class TypeCreator<T1, T2, T> : TypeCreator<T1, T2> {
    private Func<T, object> _paramCreator;
    public dynamic CreateInstance(T value) => (_paramCreator ?? (_paramCreator = GetCreator<T>()))(value);
  }
}
