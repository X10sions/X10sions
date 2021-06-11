using System;
using System.Linq.Expressions;

namespace xLinqToDB.DataProvider.DB2iSeries.V2_9 {
  public abstract class TypeCreatorBase {
    public Type Type;

    protected Func<T, object> GetCreator<T>() {
      var ctor = Type.GetConstructor(new[] { typeof(T) });
      var parm = Expression.Parameter(typeof(T));
      var expr = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.New(ctor, parm), typeof(object)), parm);
      return expr.Compile();
    }

    protected Func<T, object> GetCreator<T>(Type paramType) {
      var ctor = Type.GetConstructor(new[] { paramType });
      if (ctor == null)
        return null;
      var parm = Expression.Parameter(typeof(T));
      var expr = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.New(ctor, Expression.Convert(parm, paramType)), typeof(object)), parm);
      return expr.Compile();
    }

    public static implicit operator Type(TypeCreatorBase typeCreator) => typeCreator.Type;

    public bool IsSupported => Type != null;

    public object GetNullValue() => Type.GetNullValue();

  }
}